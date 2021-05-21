\ ---
\ subject: tools/words for nrf24l01 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   nrf24 start: ) here dup hex.

include spi1.fs
include nrf24l01.fs

0 variable nrf24_pmodel

\ check if nrf24l01 is there, needs nrf24.pininit before
: nrf24.isConnected ( -- bool )
    $03 nrf24_read_register drop
    dup 0 > swap 4 < and \ should be between 1 and 3 if nrf24l01 on spi 
;

\ set dynamic payload on all pipes
\ : nrf24.setDynPayload ( -- )
\    $1d nrf24_read_register drop        \ get FEATURE
\    $4 or $1d nrf24_write_register drop \ set EN_DPL on FEATURE
\    $3f $1c nrf24_write_register drop   \ set dynPayload on all pipes 
\ ;


\ initializes nrf24 with basic settings
: nrf24.init
    \ hardware setup
    nrf24_pininit
    
    5 ms \ delay for settle nrf24
    
    \ check if there
    nrf24.isConnected
    not if
        CR ." no nRF24L01 found!"
        exit
    then
    
    \ setRetries 
    $f 4 lshift \ delay 4ms
    $f or       \ retransmits 15
    $04 nrf24_write_register drop \ SETUP_RETR
    
    \ setDataRate
    $06 nrf24_read_register drop \ RF_SETUP
    $08 $20 or not and \ mask RF_DR_HIGH and RF_DR_LOW (p-model)
    \ $00 or ( 1 MBPS )
    \ $08 or ( 2 MBPS )
    \ $20 or ( 250kBPS only p-model )
    dup $06 nrf24_write_register drop
    $06 nrf24_read_register drop
    <> if     \ set speed failed
        CR ." setup nRF24 speed failed"
        exit
    then

    \ detect p-model and set features
    $1d nrf24_read_register drop
    \ toggle feature
    $73 $50 nrf24_write_register drop  \ toggle features 
    \ read feature

    $1d nrf24_read_register drop
    dup -rot \ save actual features
    \ if old features and actual features same then p-model
    = if
         1 nrf24_pmodel !
    then
    
    0<> if \ features set, take care
        nrf24_pmodel @ if
            $73 $50 nrf24_write_register drop \ toggle features 
        then
        $0 $1d nrf24_write_register drop
    then
    \ features is now 0

    \ setDatarate to 250kBPS if p-model
    nrf24_pmodel @ if
        $06 nrf24_read_register drop \ RF_SETUP
        $08 not and $20 or ( 250kBPS only p-model )
        $06 nrf24_write_register drop
    then
    
    $04 $1d nrf24_write_register drop \ set EN_DPL on FEATURE
    $3f $1c nrf24_write_register drop \ set dynPayload on all pipes 

    \ $00 $1c nrf24_write_register drop \ set DYNPD 0
    
    $3f $01 nrf24_write_register drop \ set EN_AA - auto ack on all pipes
    
    $03 $02 nrf24_write_register drop \ set EN_RXADDR - RX pipe 1+2
    
    \ set payload size, statisch 32 byte for all 6 pipes
    6 0 do
        32      \ 32 bytes
        $11 i + \ calc RX_PW_Px register
        nrf24_write_register drop
    loop

    \ address width 5 bytes
    $03 $03 nrf24_write_register drop \ set SETUP_AW
    
    \ channel address (max 125)
    100 $05 nrf24_write_register drop \ set RF_CH
    
    \ reset RX and TX fifo also retransmits 
    $70 $07 nrf24_write_register drop \ set STATUS
    
    \ switch off irqs, power down , crc 16bit, 
    $7C $00 nrf24_write_register drop

    \ set Addr to "1SNSR"
    s" 1SNSR" 2dup
    $0a nrf24_write_registers drop \ write to RX_ADDR_P0
    $10 nrf24_write_registers drop \ write to TX_ADDR

    \ stop listen
    0 nrf24_read_register drop
    $01 not and \ reset CONFIG.PRIM_RX
    0 nrf24_write_register drop
;

\ print out registers
: nrf24.stat. ( -- )
    CR ." === NRF24L01" nrf24_pmodel @ if [char] + emit then ."  ==="
    CR ." STATUS:     "
    $07 nrf24_read_register drop dup h.2
    CR ."     RX_DR: " dup 6 rshift 1 and .
    ." TX_DS: " dup 5 rshift 1 and .
    ." MAX_RT: "  dup 4 rshift 1 and .
    ." RX_P_NO: " dup 1 rshift 7 and .
    ." TX_FULL: " 1 and .
    
    CR ." SETUP_AW:   " 
    $03 nrf24_read_register drop dup h.2
    dup 0 > swap 4 < and not
    if
        CR ." no NRF24L01 found!"
        exit
    then

    CR ." CONFIG:     "
    $00 nrf24_read_register drop h.2
    
    CR ." EN_AA:      "
    $01 nrf24_read_register drop h.2

    CR ." EN_RXADDR:  "
    $02 nrf24_read_register drop h.2

    CR ." SETUP_RETR: "
    $04 nrf24_read_register drop h.2

    CR ." RF_CH:      "
    $05 nrf24_read_register drop h.2

    CR ." RF_SETUP:   "
    $06 nrf24_read_register drop h.2

    CR ." OBSERVE_TX: "
    $08 nrf24_read_register drop h.2

    CR ." RPD (CD):   "
    $09 nrf24_read_register drop h.2

    \ RX Addrs
    6 0 do
        CR ." RX_ADDR_P" i .digit emit ." : "
        i 2 < if \ special treatment P0/P1
            nrf24_buf 5 $0a i + nrf24_read_registers drop
            5 0 do nrf24_buf i + c@ h.2 loop
            SPACE [char] " emit
            5 0 do nrf24_buf i + c@ 32 max 126 min emit loop
            [char] " emit
        else
            $0a i + nrf24_read_register drop h.2
        then

    loop

    \ TX Addr
    CR ." TX_ADDR:    "
    nrf24_buf 5 $0a i + nrf24_read_registers drop
    5 0 do nrf24_buf i + c@ h.2 loop
    SPACE [char] " emit
    5 0 do nrf24_buf i + c@ 32 max 126 min emit loop
    [char] " emit

    CR ." RX_PW_P0-5: "
    6 0 do
        $11 i + nrf24_read_register drop h.2 space
    loop
    
    CR ." FIFO_STATUS:"
    $17 nrf24_read_register drop dup h.2
    CR ."     TX_REUSE: " dup 6 rshift 1 and .
    ." TX_FULL: " dup 5 rshift 1 and .
    ." TX_EMPTY: " dup 4 rshift 1 and .
    ." RX_FULL: " dup 1 rshift 1 and .
    ." RX_EMPTY: " 1 and .
    
    CR ." DYNPD:      "
    $1C nrf24_read_register drop h.2

    CR ." FEATURE:    "
    $1D nrf24_read_register drop h.2

;

: nrf24.buf.
    nrf24_buf
    32 0 do
        i 8 mod 0= if CR then
        dup i + c@ h.2 space
    loop
    drop
;

(   nrf24   end: ) here dup hex.
(   nrf24  size: ) swap - hex.

\ CR
\ nrf24_pininit
\ nrf24.stat.
\ CR CR 
nrf24.init
CR nrf24.stat.

