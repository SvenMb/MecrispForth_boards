\ ---
\ subject: driver/words for spi1 on stm32f407vet6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   spi1 start: ) here dup hex.

\ pins for SPI
PB3 constant spi1-SCK
PB4 constant spi1-MISO
PB5 constant spi1-MOSI

\ starting SPI1 part
SPI1 $08 + constant SPI1-SR
SPI1 $0C + constant SPI1-DR

\ Port alternate function
\ : set-alternate ( af# pin# baseAddr -- )
\    >R dup 8 < if 
\        4 * lshift R> $20 + 
\    else 
\        8 - 4 * lshift R> $24 + 
\    then
\    bis!
\ ;

\ initialize spi
: spi1-init ( -- )  
    $02 RCC_AHB1ENR bis!         \ set GPIOBEN
    omode-af-pp spi1-SCK io-mode!
    omode-af-pp spi1-MISO io-mode!
    omode-af-pp spi1-MOSI io-mode!
\ --- TODO ---
\ needs to be generalized!!!
    5 3 GPIOB set-alternate      \ PB13 -> Alternate function: %0101: AF5 (SPI2)
    5 4 GPIOB set-alternate      \ PB14 -> Alternate function: %0101: AF5 (SPI2) 
    5 5 GPIOB set-alternate      \ PB15 -> Alternate function: %0101: AF5 (SPI2) 
\ --- ODOT ---
    1 12 lshift RCC_APB2ENR bis!  \ set SPI1EN
    $04 SPI1 bis!   \ MSTR in SPI_CR1
    \ expecting APB2 speed 84MHz setting to ~10MHz for nRF24 and w25q16 use 
    $10 SPI1 bis!   \ spi1 speed 1/8 BR0:2  in SPI_CR1
    8 bit SPI1 bis! \ SSI  in SPI_CR1
    9 bit SPI1 bis! \ SSM  in SPI_CR1
    6 bit SPI1 bis! \ SPE - enable  in SPI_CR1
;

\ read and write in same cycle
: >spi1> ( byte -- byte )
    SPI1-DR h@ drop  \ clearing OVR flag
    begin 2 SPI1-SR bit@ not while pause repeat \ wait for TXE
    SPI1-DR h!                                  \ send byte
    begin 1 SPI1-SR bit@ not while pause repeat \ wait for RXNE
    SPI1-DR h@                                  \ fetch byte
;

: spi1> ( -- byte )
    $ff >spi1> \ send dummy byte
;

: >spi1 ( byte -- )
    >spi1>
    drop       \ drop answer 
;

\ end SPI1 part

(   spi1   end: ) here dup hex.
(   spi1  size: ) swap - hex.