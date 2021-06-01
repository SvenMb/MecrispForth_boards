\ ---
\ subject: driver/words for w25q16 via spi1 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: should work with w25q16 - w25q32 - w25q64 - w25q128
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   w25q start: ) here dup hex.

[ifndef] spi1-init
include spi1.fs
[then]

PB0 constant w25q.CS

\ 1 sector buffer, you will need this anyway
$100 buffer: w25q.Buf

\ activate CS on w25qxx
: +w25q.CS ( -- )
    w25q.CS ioc!
;

: -w25q.CS ( -- )
    w25q.CS ios!
;

: w25q.init ( -- )
    spi1-init
    
    omode-pp w25q.CS io-mode!
    -w25q.CS
;

: w25q.Status ( -- status.low )
    +w25q.CS
    $05 >spi1 spi1>
    -w25q.CS
;

: w25q.Statush ( -- status.high )
    +w25q.CS
    $35 >spi1 spi1>
    -w25q.CS
;

: w25q.PowerDown ( -- )
    +w25q.CS
    $B9 >spi1 
    -w25q.CS
;

: w25q.PowerUP ( -- DeviceId )
    +w25q.CS
    $ab >spi1 0 >spi1 0 >spi1 0 >spi1
    spi1>
    -w25q.CS
;

\ ManufactureId $ef for Winbond
\ DeviceId:
\ $14 for w25q16
\ $15 for w25q32
\ $16 for w25q64
\ $17 for w25q128
: w25q.manId ( -- ManufactureId DeviceId )
    +w25q.CS
    $90 >spi1 0 >spi1 0 >spi1 0 >spi1
    spi1> spi1>
    -w25q.CS
;

\ get 64bit unique ID
: w25q.uniqId ( -- id_high id_low )
    +w25q.CS
    $4b >spi1 0 >spi1 0 >spi1 0 >spi1 0 >spi1
    0
    8 0 do
        spi1>
        i 4 mod 3 <> if
            or 8 lshift
        else
            or 0
        then
    loop
    drop
    -w25q.CS
;

\ get jedec Device Id
: w25q.jedecID ( -- ManufacturerId JedecDevId )
    +w25q.CS
    $9f >spi1
    spi1>
    spi1> 8 lshift
    spi1> or
    -w25q.CS
;

\ wait to finish command
: w25q.notBusy ( -- )
    +w25q.CS
    $05 >spi1
    begin
        0 >spi1>
        1 and \ check busy bit
    while
            pause
    repeat        
    -w25q.CS
;

\ enable writing
: w25q.WRen ( -- )
    +w25q.CS
    $06 >spi1
    -w25q.CS
    w25q.notBusy
;

\ disable writing
: w25q.WRdis ( -- )
    +w25q.CS
    $04 >spi1
    -w25q.CS
    w25q.notBusy
;

\ initialize reading from c-addr on flash
\ should by inside an $100 sector
: w25q.rdFrom ( c-addr -- )
    +w25q.CS
    $03 >spi1
    dup 16 rshift >spi1
    dup 8 rshift $ff and >spi1
    $ff and >spi1
;

\ use spi1> for reading bytes 

\ initialize writeing from c-addr on flash 
: w25q.wrFrom ( c-addr -- )
    w25q.WRen
    +w25q.CS
    $02 >spi1
    dup 16 rshift >spi1
    dup 8 rshift $ff and >spi1
    $ff and >spi1
;

\ use >spi1 for writing bytes max to next sector border!

\ close current transfer after w25q.rdfrom or w25q.wrFrom
: w25q.close ( -- )
    -w25q.CS
    w25q.notBusy
    w25q.WRdis
;    

\ erase one 4k ($1000 bytes) flash sector
\ c-addr! but still full sector erased
: w25q.4kErase ( c-addr -- )
    w25q.WRen
    +w25q.CS
    $20 >spi1
    dup 16 rshift >spi1
    dup 8 rshift $ff and >spi1
    $ff and >spi1    
    -w25q.CS
    w25q.notbusy
    w25q.WRdis
;

\ erase one 32k ($8000 bytes) flash sector
\ c-addr! but still full sector erased
: w25q.32kErase ( c-addr -- )
    w25q.WRen
    +w25q.CS
    $52 >spi1
    dup 16 rshift >spi1
    dup 8 rshift $ff and >spi1
    $ff and >spi1    
    -w25q.CS
    w25q.notbusy
    w25q.WRdis
;

\ erase one 64k ($10000 bytes) flash sector
\ c-addr! but still full sector erased
: w25q.64kErase ( c-addr -- )
    w25q.WRen
    +w25q.CS
    $d8 >spi1
    dup 16 rshift >spi1
    dup 8 rshift $ff and >spi1
    $ff and >spi1    
    -w25q.CS
    w25q.notbusy
    w25q.WRdis
;

\ erase complete chip (to $ff)
: w25q.chipErase
    w25q.WRen
    +w25q.CS
    $c7 >spi1
    -w25q.CS
    w25q.notbusy
    w25q.WRdis
;

\ print out a $100 byte (256) sector
\ trashes w25q.Buf!
\ c-addr but dumpes full 256 byte sector 
: w25q.dumpSec ( c-addr -- )
    $ff bic dup w25q.rdFrom
    $100 0 do
        i $10 mod 0= if
            CR dup i + hex. space \ print address
        then
        spi1> \ get next byte
        dup w25q.Buf i $10 mod + c! \ save into buffer for ascii print
        h.2 space \ print hex
        i $10 mod 7 = if space then \ extra space after 8 hex values
        i $10 mod $F = if     \ asci printout
            $10 0 do
                w25q.Buf i + c@
                $20 max $7E min \ only printable chars
                emit
            loop
        then
    loop
    w25q.close
    drop
;

(   w25q   end: ) here dup hex.
(   w25q  size: ) swap - hex.










