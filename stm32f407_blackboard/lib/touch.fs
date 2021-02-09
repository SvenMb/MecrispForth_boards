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

: Touch? ( -- b )
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
\    Touch?
\    if
        $90 TPread \ X
        $FFF swap -
        4 lshift
        3 ms 
        $D0 TPread \ Y
        $FFF swap -
        4 lshift
        3 ms
\   then
;

: s_cross s" touch cross" ;
: s_release s" release" ;

: wait_touch
    begin
        Touch?
        key? or        
    until
    tpxy
;

160 variable ax
6500 variable bx
200 variable ay
50 variable by

: trxy ( rx ry -- tx ty  )
    by @ - ay @ /
    swap
    bx @ - ax @ /
    swap
;


: calibTouch ( -- )
    clear
    tft-on

    0 0 40 40 line
    40 0 0 40 line
    20 20 15 circle
    s_cross 120 116 drawstring
    wait_touch
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms
    
    319 0 over 40 - 40 line
    279 0 over 40 + 40 line
    299 20 15 circle
    s_cross 120 116 drawstring
    wait_touch
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms

    drop \ I don't use that y-value
    rot dup -rot
    - 319 / dup ax !
    ." ax:" dup .
    20 * - bx !
    ." bx:" dup .

    0 239 40 over 40 - line
    0 199 40 over 40 + line
    20 219 15 circle
    s_cross 120 116 drawstring
    wait_touch
    clear
    s_release 120 116 drawstring
    begin Touch? not key? or until
    500 ms

    swap drop over
    - 239 / dup ay !
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


: testTouch begin wait_touch trxy 2dup putpixel CR hex. hex. key? until ;