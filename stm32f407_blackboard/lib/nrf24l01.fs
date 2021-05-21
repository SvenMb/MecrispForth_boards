\ ---
\ subject: hw driver/words for nrf24l01 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   nrf24init start: ) here dup hex.

$20 buffer: nrf24_buf

\ extra pins for nrf24  
PB7 constant nrf24-CS
PB6 constant nrf24-CE
PB8 constant nrf24-IRQ

\ initialize nrf24
: nrf24_pininit ( -- )
    \ asuming spi1
    spi1-init
    
    \ asuming GPIOBEN already set
    omode-pp nrf24-CS io-mode!  \ PB12 -> output for CS
    nrf24-CS ios!               \ deselect spi2-touch

    omode-pp nrf24-CE io-mode!
    nrf24-CE ioc! \ set CE to low(!)

    \ clean buffer
    nrf24_buf $20 $00 fill
;

\ select spi to nrf24
: +nrf24
    nrf24-CS ioc! \ select nrf24 spi (CS low)
;

\ deselect spi to nrf24
: -nrf24
    nrf24-CS ios! \ deselect nrf24 spi (CS high)
;

\ power up
: +nrf24.CE
    nrf24-CE ios!
;

\ power down
: -nrf24.CE
    nrf24-CE ioc!
;

\ nrf24 write register
: nrf24_write_register ( byte reg -- status )
    +nrf24
    $20 or >spi1> \ W_REGISTER
    swap          \ STATUS back
    \ CR .s
    >spi1> drop         \ send value
    -nrf24
;

\ nrf24 write register with multi byte from buffer
: nrf24_write_registers ( buffer len reg -- status )
    +nrf24
    $20 or >spi1> \ send W_REGISTER get STATUS
    >r            \ STATUS to return stack
    0 do
        dup i + c@
        >spi1> drop
    loop
    -nrf24
    drop \ drop buffer
    r>   \ get STATUS back
;

\ nrf24 read register
: nrf24_read_register ( reg -- byte status)
    +nrf24
    >spi1> \ send R_REGISTER get STATUS
    spi1>  \ Value
    -nrf24
    swap
;

\ nrf24 read register with multi byte to buffer
: nrf24_read_registers ( buffer len reg -- status )
    +nrf24
    >spi1> \ send R_REGISTER get STATUS
    >r     \ save STATUS to return stack
    0 do            \ len times
        spi1>       \ get byte
        \ CR .s
        over i + c!  \ save byte into buffer
    loop
    -nrf24
    drop    \ drop buffer
    r>      \ get STATUS back
;

(   nrf24init   end: ) here dup hex.
(   nrf24init  size: ) swap - hex.