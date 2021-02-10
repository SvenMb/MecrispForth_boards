\ ---
\ subject: driver/words for xpr2046 touch on stm32f407vet6 black board 
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   touch start: ) here dup hex.

\ starting SPI part
SPI2 $08 + constant SPI2-SR
SPI2 $0C + constant SPI2-DR

\ Port alternate function
: set-alternate ( af# pin# baseAddr -- )
    >R dup 8 < if 
        4 * lshift R> $20 + 
    else 
        8 - 4 * lshift R> $24 + 
    then
    bis!
;

\ initialize spi and touch pins
: touch-Init ( -- )  
    $06 RCC_AHB1ENR bis!    \ set GPIOAEN + GPIOBEN
    omode-pp PB12 io-mode!  \ PB12 -> output for T_CS
    imode-high PC5 io-mode! \ PC5 -> input for T_PEN (Touch interupt)
    PB12 ios!               \ deselect spi2-touch
    omode-af-pp PB13 io-mode!
    omode-af-pp PB14 io-mode!
    omode-af-pp PB15 io-mode!
    5 13 GPIOB set-alternate      \ PB13 -> Alternate function: %0101: AF5 (SPI2)
    5 14 GPIOB set-alternate      \ PB14 -> Alternate function: %0101: AF5 (SPI2) 
    5 15 GPIOB set-alternate      \ PB15 -> Alternate function: %0101: AF5 (SPI2) 
    1 14 lshift RCC_APB1ENR bis!  \ setSPI2EN
    $04 SPI2 bis!   \ MSTR in SPI_CR1
    $28 SPI2 bis!   \ spi speed 1/256 BR0:2  in SPI_CR1
    8 bit SPI2 bis! \ SSI  in SPI_CR1
    9 bit SPI2 bis! \ SSM  in SPI_CR1
    6 bit SPI2 bis! \ SPE - enable  in SPI_CR1
;

\ select touch
: +spi2
    PB12 ioc! \ set PB12 low
;

\ deselect touch
: -spi2
    PB12 ios! \ set PB12 high
;


\ send to touch
: >spi2 ( byte -- )
    begin 2 SPI2-SR bit@ not while pause repeat         \ wait for TXE
    SPI2-DR h!                                          \ send byte
;

\ read from touch
: spi2> (  --  byte )
    SPI2-DR h@ drop  \ clearing OVR flag
    $FF  >spi2       \ send dummy (no read without write)
    begin 1 SPI2-SR bit@ not while pause repeat \ wait for RXNE
    SPI2-DR h@       \ fetch data
;

\ end SPI part

\ start xpt2046 part 

\ T_PEN active?
: Touch? ( -- b )
    PC5 io@ not
;

\ adjust bit shifting touch chip
: TPread ( cmd -- x )
    +spi2
    2 ms
    >spi2 
    spi2> drop 
    spi2>
    spi2>
    -spi2
    1 ms
    3 rshift $1f and swap
    5 lshift $FE0 and or
;

\ read raw xy data, adjust rotation
: TPxy ( -- x y / 0 )
    $90 TPread \ X
    $FFF swap -
    4 lshift
    $D0 TPread \ Y
    $FFF swap -
    4 lshift
;

\ end xpt2046 part

\ start filter part
3 constant tallsamp \ # of samples samples
1 constant tusesamp \ used samples for averaging (lower and higher values will be thrown away) 

tallsamp 2* buffer: tbufx \ buffer for halfwords x sampl
tallsamp 2* buffer: tbufy \ buffer for halfwords y sampl

\ fill with test data 
\ : fillbuf ( buffer len - - )
\    0 do dup i 2* + i swap h! loop drop
\ ;

\ clean buffer 
\ : cleanbuf ( buffer len - - )
\    0 do dup i 2* + 0 swap h! loop drop
\ ;

\ sort halfwords for samples
: hsort ( buffer len - buffer len )
    begin
        false -rot
        dup 1- 0 do
            over i 2* +
            >r           \ addr to return stack
            r@ h@        \ get first cell
            r@ 2+ h@     \ get second cell
            > dup if     \ if first cell is larger then second
                r@ h@    \ get first cell
                r@ 2+ h@ \ get second cell
                r@ h!    \ write second cell as first
                r@ 2+ h! \ write first cell as second
            then
            rdrop \ balance return stack
            >r rot r> or -rot \ addup sort flag
        loop
        rot not \ get sort flag
    until
;

\ build average 
: hsamp ( buffer len sam - n )
    >r     \ put sample size on r-stack
    r@ - $ffe and + \ calc startaddr of samples with halfwords
    0 swap \ put starting value on stack
    dup r@ 2* + \ calc last addr of samples
    swap
    do
        i h@ +
    2 +loop
    r> / \ divide 
;

\ returns filtered x y data
: touchxy ( - rx ry )
    tallsamp 0
    do
        TPxy
        tbufy i 2* + h!
        tbufx i 2* + h!
    loop
    tbufx tallsamp hsort
    tusesamp hsamp
    tbufy tallsamp hsort
    tusesamp hsamp
;

\ end filter part



\ waits for touch (or key via console) and then returns filtered xy data
: wait_touch ( - - rx ry )
    begin
        Touch?
        key? or
    until
    touchxy
;

\ translate to tft pixel positions
\ this value only work for my device at a very specific temp, so start calibtouch befor using touch 
190 variable ax
7800 variable bx
250 variable ay
50 variable by

\ translate raw xy to tft panel xy
: trxy ( rx ry -- tx ty valid )
    by @ - ay @ /
    swap
    bx @ - ax @ /
    swap
    \ check boundaries
    dup 240 <
    over 0< not and
    2 pick dup
    320 < swap
    0< not and
    and
;

\ define strings for calibtouch
: s_cross s" touch cross" ;
: s_release s" release" ;

\ calibrate touch screen
: calibTouch ( -- )
    clear
    tft-on

    0 0 40 40 line
    40 0 0 40 line
    20 20 15 circle
    s_cross 120 116 drawstring
    wait_touch 2drop
    300 ms
    wait_touch
    swap
    CR ." x:" dup .
    swap
    ." y:" dup . 
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms
    
    319 0 over 40 - 40 line
    279 0 over 40 + 40 line
    299 20 15 circle
    s_cross 120 116 drawstring
    wait_touch 2drop
    300 ms
    wait_touch
    swap
    CR ." x:" dup .
    swap
    ." y:" dup . 
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms

    drop \ I don't use that y-value
    rot dup -rot
    - 279 / dup ax !
    ." ax:" dup .
    20 * - bx !
    ." bx:" dup .

    0 239 40 over 40 - line
    0 199 40 over 40 + line
    20 219 15 circle
    s_cross 120 116 drawstring
    wait_touch 2drop
    300 ms
    wait_touch
    swap
    CR ." x:" dup .
    swap
    ." y:" dup . 
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms

    swap drop over
    - 199 / dup ay !
    ." ay:" dup .
    20 * - by !
    ." by:" dup .

    \ 319 239 over 40 - over 40 - line
    \ 279 239 over 40 + over 40 - line
    \ 299 219 15 circle
    \ s_cross 120 116 drawstring
    \ wait_touch
    \ clear
    \ s_release 120 116 drawstring
    \ begin Touch? not key? or until
    \ 500 ms

    clear
;

\ small test programm
: testTouch ( -- )
    tft-on
    begin
        wait_touch trxy
        if
            putpixel
        else
            2drop CR ."  out!"
        then
        key?
    until
;

( touch end:   ) here dup hex.
( touch size:  ) swap - hex.
