\ ---
\ subject: hw driver/words for nrf24l01 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   nrf24init start: ) here dup hex.

[ifndef] spi1-init
include spi1.fs
[then]

\ you will need a buffer anyway...
$20 buffer: nrf24.buf

\ stat about dev
0 variable nrf24.stat
\ 7:1 reserved
\ 0 P-model

\ extra pins for nrf24  
PB7 constant nrf24.CS
PB6 constant nrf24.CE
PB8 constant nrf24.IRQ

\ initialize nrf24
: nrf24.pininit ( -- )
    \ asuming spi1
    spi1-init
    
    \ asuming GPIOBEN already set
    omode-pp nrf24.CS io-mode!  \ PB12 -> output for CS
    nrf24.CS ios!               \ deselect spi2-touch

    omode-pp nrf24.CE io-mode!
    nrf24.CE ioc! \ set CE to low(!)

    \ clean buffer
    nrf24.buf $20 $00 fill
;

\ select spi to nrf24
: +nrf24
    nrf24.CS ioc! \ select nrf24 spi (CS low)
;

\ deselect spi to nrf24
: -nrf24
    nrf24.CS ios! \ deselect nrf24 spi (CS high)
;

\ power up
: +nrf24.CE
    nrf24.CE ios!
;

\ power down
: -nrf24.CE
    nrf24.CE ioc!
;

\ ==== basic command interface to nRF24 ====
\ just send 1 byte cmd
: nrf24.cmd ( cmd -- status )
    +nrf24
    >spi1> 
    -nrf24
;

\ send cmd and send 1 byte
: nrf24.w_cmd ( byte cmd -- status )
    +nrf24
    >spi1> \ send cmd, get status
    swap   \ STATUS back
    >spi1  \ send value
    -nrf24
;

\ send cmd and buffer (with len bytes)
: nrf24.wb_cmd ( buffer len cmd -- status )
    +nrf24
    >spi1> \ send cmd, get STATUS
    >r     \ STATUS to return stack
    0 do
        dup i + c@
        >spi1
    loop
    -nrf24
    drop \ drop buffer
    r>   \ get STATUS back
;

\ send cmd and get 1 byte
: nrf24.r_cmd ( cmd -- byte status )
    +nrf24
    >spi1> \ send cmd get STATUS
    spi1>  \ Value
    -nrf24
    swap
;

\ send cmd and get len bytes to buffer 
: nrf24.rb_cmd ( buffer len cmd -- status )
    +nrf24
    >spi1> \ send cmd, get STATUS
    >r     \ save STATUS to return stack
    0 do            \ len times
        spi1>       \ get byte
        over i + c!  \ save byte into buffer
    loop
    -nrf24
    drop    \ drop buffer
    r>      \ get STATUS back
;

\ start public:
\ ==== Register names of nRF24
$00 constant nrf24.CONFIG
\ 7 reserved 
\ 6 MASK_RX_DR
\ 5 MASK_TX_DS
\ 4 MASK_MAX_RT
\ 3 EN_CRC
\ 2 CRCO
\ 1 PWR_UP
\ 0 PRIM_RX
$01 constant nrf24.EN_AA
$02 constant nrf24.EN_RXADDR
$03 constant nrf24.SETUP_AW
\ 7:2 reserved ( all 0 )
\ 1:0 addr length ( +2 )
$04 constant nrf24.SETUP_RETR
\ 7:4 auto retransmit delay ( $F = 4ms )  
$05 constant nrf24.RF_CH
\ 6:0 channel to operate on
$06 constant nrf24.RF_SETUP
\ 7 cont_wave
\ 6 reserved
\ 5 RF_DR_LOW
\ 4 PLL_LOCK
\ 3 RF_DR_HIGH
\ 2:1 RF_PWR
\ 0 obsolete
$07 constant nrf24.STATUS
\ 7 reserved
\ 6 TX_DR
\ 5 TX_DS
\ 4 MAX_RT
\ 3:1 RX_P_NO
\ 0 TX_FULL
$08 constant nrf24.OBSERVE_TX
\ 7:4 PLOS_CNT
\ 3:0 ARC_CNT
$09 constant nrf24.CD
\ 7:1 reserved
\ 0 RPD ( CD - Carrier detect )
$0a constant nrf24.RX_ADDR_P0   \ max 5 bytes
\ $0b constant nrf24.RX_ADDR_P1 \ max 5 bytes
\ $0c constant nrf24.RX_ADDR_P2 \ 1 byte
\ $0d constant nrf24.RX_ADDR_P3 \ 1 byte 
\ $0e constant nrf24.RX_ADDR_P4 \ 1 byte
\ $0f constant nrf24.RX_ADDR_P5 \ 1 byte
$10 constant nrf24.TX_ADDR      \ max 5 bytes
$11 constant nrf24.RX_PW_P0
\ 7:6 reserved
\ 5:0 bytes in RX payload
\ $12 constant nrf24.RX_PW_P1
\ $13 constant nrf24.RX_PW_P2
\ $14 constant nrf24.RX_PW_P3
\ $15 constant nrf24.RX_PW_P4
\ $16 constant nrf24.RX_PW_P5
$17 constant nrf24.FIFO_STATUS
\ 7 reserved
\ 6 TX_REUSE
\ 5 TX_FULL
\ 4 TX_EMPTY
\ 3:2 reserved
\ 1 RX_FULL
\ 0 RX_EMPTY
$1c constant nrf24.DYNPD
\ 7:6 reserved
\ 5:0 bitmap dynamic payload pipes 
$1d constant nrf24.FEATURE
\ 7:3 reserved
\ 2 EN_DPL ( enable dynamic payload )
\ 1 EN_ACK_PAY ( enable auto ack )
\ 0 EN_DYN_ACK ( enable W_TX_PAYLOAD_NOACK )

\ ==== SPI command set nRF24L01 ==== 
\ nrf24 read register 1 byte
: nrf24.r_register ( reg -- byte status)
    nrf24.r_cmd \ inline
;

\ nrf24 read register with multi byte to buffer
: nrf24.r_registers ( buffer len reg -- status )
    nrf24.rb_cmd \ inline
;

\ nrf24 write register 1 byte
: nrf24.w_register ( byte reg -- status )
    $20 or nrf24.w_cmd \ inline
;

\ nrf24 write register with multi byte from buffer
: nrf24.w_registers ( buffer len reg -- status )
    $20 or nrf24.wb_cmd \ inline
;

\ nrf24 read payload to buffer
\ buffer should to be 32 byte, 
: nrf24.r_rx_payload ( buffer len -- status )
    $61 nrf24.rb_cmd \ inline
;

\ nrf24 write payload to buffer
: nrf24.w_tx_payload ( buffer len -- status ) 
    $A0 nrf24.wb_cmd \ inline
;

\ 
: nrf24.flush_tx ( -- status )
    $e1 nrf24.cmd \ inline
;

\
: nrf24.flush_rx ( -- status )
    $e2 nrf24.cmd \ inline
;

\ 
: nrf24.reuse_tx_pl ( -- status )
    $e3 nrf24.cmd \ inline
;


\ read rx payload width from current (top) payload entry
: nrf24.r_rx_pl_wid ( -- width status ) 
    $60 nrf24.r_cmd \ inline
;

\
: nrf24.w_ack_payload ( buffer len #pipe -- status )
    \ %101 max \ max #pipe $5 ??? 
    $a4 or   \ blend in #pipe
    nrf24.wb_cmd \ inline
;

\
: nrf24.w_tx_payload_noack ( buffer len -- status )
    $b0 nrf24.wb_cmd \ inline
;

\ no operation, but get status
: nrf24.nop ( -- status )
    $ff nrf24.cmd \ inline
;

: nrf24.toggle_feature ( -- status )
    $73 $50 nrf24.w_cmd \ inline
;

(   nrf24init   end: ) here dup hex.
(   nrf24init  size: ) swap - hex.