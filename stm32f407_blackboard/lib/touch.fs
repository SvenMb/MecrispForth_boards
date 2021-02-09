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
    $38 SPI2 bis!   \ spi speed 1/256 BR0:2  in SPI_CR1
    8 bit SPI2 bis! \ SSI  in SPI_CR1
    9 bit SPI2 bis! \ SSM  in SPI_CR1
    6 bit SPI2 bis! \ SPE - enable  in SPI_CR1
;

: +spi2
    PB12 ioc! \ set PB12 low
;

: -spi2
    PB12 ios! \ set PB12 high
;


\ to touch
: >spi2 ( byte -- )
    begin 2 SPI2-SR bit@ not while pause repeat         \ wait for TXE
    SPI2-DR h!                                          \ send byte
;

\ from touch
: spi2> (  --  byte )
    SPI2-DR h@ drop  \ clearing OVR flag
    $FF  >spi2       \ send dummy (no read without write)
    begin 1 SPI2-SR bit@ not while pause repeat \ wait for RXNE
    SPI2-DR h@       \ fetch data
;

: isTouch ( -- b )
    PC5 io@ not
;

: TPread ( cmd -- x )
    +spi2
    >spi2 
    spi2> drop 
    spi2>
    spi2>
    -spi2
    3 rshift $1f and swap
    5 lshift $FE0 and or
;

: TPxy ( -- x y / 0 )
    isTouch
    if
        $90 TPread \ X
        3 ms 
        $D0 TPread \ Y
        3 ms
        CR hex. hex.
    then
;

: calibTouch ( -- )
    
    
;


: testTouch begin TPxy key? until ;