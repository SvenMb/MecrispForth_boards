\ ---
\ subject: tools/words for nrf24l01 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   nrf24 start: ) here dup hex.

[ifndef] spi1-init 
include spi1.fs
[then]
[ifndef] nrf24.cmd
include nrf24l01.fs
[then]

\ check if nrf24l01 is there, needs nrf24.pininit before
: nrf24.isConnected ( -- bool )
    nrf24.SETUP_AW nrf24.r_cmd drop
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
    nrf24.pininit
    
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
    nrf24.SETUP_RETR nrf24.w_register drop \ SETUP_RETR
    \ setDataRate
    nrf24.RF_SETUP nrf24.r_register drop \ RF_SETUP

    $08 $20 or not and \ mask RF_DR_HIGH and RF_DR_LOW (p-model)
    \ $00 or ( 1 MBPS )
    \ $08 or ( 2 MBPS )
    \ $20 or ( 250kBPS only p-model )
    dup nrf24.RF_SETUP nrf24.w_register drop
    nrf24.RF_SETUP nrf24.r_register drop
    <> if     \ set speed failed
        CR ." setup nRF24 speed failed"
        exit
    then

    \ detect p-model and set features
    nrf24.FEATURE nrf24.r_register drop
    
    \ toggle feature
    nrf24.toggle_feature drop  \ toggle features 

    \ read feature
    nrf24.FEATURE nrf24.r_register drop
    dup -rot \ save actual features
    \ if old features and actual features same then p-model
    = if
        $01 nrf24.stat cbis!
    else
        $01 nrf24.stat cbic!
    then
    
    0<> if \ features set, take care
        $01 nrf24.stat cbit@ if
            nrf24.toggle_feature drop \ toggle features 
        then
        $0 nrf24.FEATURE nrf24.w_register drop
    then
    \ features is now 0

    \ setDatarate to 250kBPS if p-model
    $01 nrf24.stat cbit@ if
        nrf24.RF_SETUP nrf24.r_register drop \ RF_SETUP
        $08 not and $20 or ( 250kBPS only p-model )
        nrf24.RF_SETUP nrf24.w_register drop
    then
    
    $04 nrf24.FEATURE nrf24.w_register drop \ set EN_DPL on FEATURE
    $3f nrf24.DYNPD nrf24.w_register drop \ set dynPayload on all pipes 

    $3f nrf24.EN_AA nrf24.w_register drop \ set EN_AA - auto ack on all pipes
    
    $03 nrf24.EN_RXADDR nrf24.w_register drop \ set EN_RXADDR - RX pipe 1+2
    
    \ set payload size, statisch 32 byte for all 6 pipes
    6 0 do
        32      \ 32 bytes
        nrf24.RX_PW_P0 i + \ calc RX_PW_Px register
        nrf24.w_register drop
    loop

    \ address width 5 bytes
    $03 nrf24.SETUP_AW nrf24.w_register drop \ set SETUP_AW
    
    \ channel address (max 125)
    100 nrf24.RF_CH nrf24.w_register drop \ set RF_CH
    
    \ reset RX and TX fifo also retransmits 
    $70 nrf24.STATUS nrf24.w_register drop \ set STATUS
    
    \ switch off irqs, power down , crc 16bit, 
    $7C nrf24.CONFIG nrf24.w_register drop

    \ set Addr to "1SNSR"
    s" 1SNSR" 2dup
    nrf24.RX_ADDR_P0 nrf24.w_registers drop \ write to RX_ADDR_P0
    nrf24.TX_ADDR nrf24.w_registers drop    \ write to TX_ADDR

    \ stop listen
    nrf24.CONFIG nrf24.r_register drop
    $01 not and \ reset CONFIG.PRIM_RX
    nrf24.CONFIG nrf24.w_register drop
;

\ print out registers
: nrf24.stat. ( -- )
    CR ." === NRF24L01" $01 nrf24.stat cbit@ if [char] + emit then ."  ==="
    CR ." STATUS:     "
    $07 nrf24.r_register drop dup h.2
    CR ."     RX_DR: " dup 6 rshift 1 and .
    ." TX_DS: " dup 5 rshift 1 and .
    ." MAX_RT: "  dup 4 rshift 1 and .
    ." RX_P_NO: " dup 1 rshift 7 and .
    ." TX_FULL: " 1 and .
    
    CR ." SETUP_AW:   " 
    $03 nrf24.r_register drop dup h.2
    dup 0 > swap 4 < and not
    if
        CR ." no NRF24L01 found!"
        exit
    then

    CR ." CONFIG:     "
    $00 nrf24.r_register drop h.2
    
    CR ." EN_AA:      "
    $01 nrf24.r_register drop h.2

    CR ." EN_RXADDR:  "
    $02 nrf24.r_register drop h.2

    CR ." SETUP_RETR: "
    $04 nrf24.r_register drop h.2

    CR ." RF_CH:      "
    $05 nrf24.r_register drop h.2

    CR ." RF_SETUP:   "
    $06 nrf24.r_register drop h.2

    CR ." OBSERVE_TX: "
    $08 nrf24.r_register drop h.2

    CR ." RPD (CD):   "
    $09 nrf24.r_register drop h.2

    \ RX Addrs
    6 0 do
        CR ." RX_ADDR_P" i .digit emit ." : "
        i 2 < if \ special treatment P0/P1
            nrf24.buf 5 $0a i + nrf24.r_registers drop
            5 0 do nrf24.buf i + c@ h.2 loop
            SPACE [char] " emit
            5 0 do nrf24.buf i + c@ 32 max 126 min emit loop
            [char] " emit
        else
            $0a i + nrf24.r_register drop h.2
        then

    loop

    \ TX Addr
    CR ." TX_ADDR:    "
    nrf24.buf 5 $0a i + nrf24.r_registers drop
    5 0 do nrf24.buf i + c@ h.2 loop
    SPACE [char] " emit
    5 0 do nrf24.buf i + c@ 32 max 126 min emit loop
    [char] " emit

    CR ." RX_PW_P0-5: "
    6 0 do
        $11 i + nrf24.r_register drop h.2 space
    loop
    
    CR ." FIFO_STATUS:"
    $17 nrf24.r_register drop dup h.2
    CR ."     TX_REUSE: " dup 6 rshift 1 and .
    ." TX_FULL: " dup 5 rshift 1 and .
    ." TX_EMPTY: " dup 4 rshift 1 and .
    ." RX_FULL: " dup 1 rshift 1 and .
    ." RX_EMPTY: " 1 and .
    
    CR ." DYNPD:      "
    $1C nrf24.r_register drop h.2

    CR ." FEATURE:    "
    $1D nrf24.r_register drop h.2

;

: nrf24.buf. ( -- )
    nrf24.buf
    32 0 do
        i 8 mod 0= if CR then
        dup i + c@ h.2 space
    loop
    drop
;

\ switch on!
: nrf24.powerUp ( -- )
    nrf24.CONFIG nrf24.r_register drop  \ read CONFIG
    dup $03 and not if  \ if not already powerup
        $02 or      \ set PWR_UP
        nrf24.CONFIG nrf24.w_register drop  \ write CONFIG
        5 ms \ power up takes time
    else
        drop
    then
;

\ switch off for config
: nrf24.powerDown ( -- )
    nrf24.CONFIG nrf24.r_register drop  \ read CONFIG
    dup $03 and if \ if not already TX and powerup
        $02 bic    \ reset PWR_UP
        nrf24.CONFIG nrf24.w_register drop  \ write CONFIG
    else
        drop
    then
;


: nrf24.send ( buffer len -- status )
    -nrf24.CE   \ make sure CE is low (not TX)

    \ wait until free space in TX-queue
    begin
        nrf24.STATUS nrf24.r_register drop
        $01 and
    while
            pause
    repeat

    \ clear TX_DS (ack) if set
    nrf24.STATUS nrf24.r_register drop
    $20 and if
        $20 nrf24.STATUS nrf24.w_register drop  \ reset TX_DS
    then

    nrf24.powerUp

    \ check if in TX mode
    nrf24.CONFIG nrf24.r_register drop  \ read CONFIG
    dup $01 and if \ if not already TX and powerup
        $01 bic     \ reset PRIM_RX
        nrf24.CONFIG nrf24.w_register drop  \ write CONFIG
        5 ms \ switching to TX takes time?
    else
        drop
    then
    
    nrf24.flush_tx drop                 \ flush

    nrf24.w_tx_payload drop \ write payload

    +nrf24.CE     \ SEND!

    10 us

    -nrf24.CE     \ done
;

(   nrf24   end: ) here dup hex.
(   nrf24  size: ) swap - hex.

\ just for testing
CR
nrf24.pininit
nrf24.stat.
CR CR 
nrf24.init
CR nrf24.stat.

