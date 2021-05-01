\ ws2812b.fs
\ for stm32f4 with 24mhz pclk1
\ Output is on MOSI2 (PB15)
\ 
\ this is based on https://github.com/tht/forth-stuff/blob/master/ws2812b/ws2812b.fs 
\ which was written for stm32f103


\ --------------------------------------------------
\  Register adresses
\ --------------------------------------------------

DMA1 $10 + $18 4 * + constant DMA1-S4CR   \ control register for stream 4
DMA1 $0c +           constant DMA1-HIFCR  \ interupt clear register
DMA1 $14 + $18 4 * + constant DMA1-S4NDTR \ stream 4 number of data register
DMA1 $1c + $18 4 * + constant DMA1-S4M0AR \ stream 4 memory 0 address register
DMA1 $18 + $18 4 * + constant DMA1-S4PAR  \ stream 4 peripheral address register 
SPI2 $04 + constant SPI2-CR2
SPI2 $08 + constant SPI2-SR
SPI2 $0C + constant SPI2-DR


\ --------------------------------------------------
\  Configuration
\ --------------------------------------------------

\ PB13 constant SCLK2  
\ PB14 constant MISO2  
PB15 constant MOSI2  

\ number of LEDs (using 9 Bytes of memory each!)
[ifndef] MAX-LEDS 32 constant MAX-LEDS [then]


\ --------------------------------------------------
\  Internal Helpers
\ --------------------------------------------------

\ Changes a flag to %110 (if true) or %100 (otherwise)
: bit2triplett ( f - u ) if %110 else %100 then ;

\ Iterates over all bits from input-byte and replaces them by bit2triplett output
\ 1 byte becomes 3 bytes
: byte2triplett ( u  - u ) 0 0 7 do 3 lshift over i bit and bit2triplett or -1 +loop nip ;

\ Write one color-byte in WS2812B format to 3 bytes in memory
: writetriplett ( color base_addr index - )
  3 * + >r                       \ calculate addr base and store on return stack
  byte2triplett 
           dup $ff and r@ 2 + c! \ output last byte
  8 rshift dup $ff and r@ 1+  c! \ output second byte
  8 rshift     $ff and r>     c! \ output first byte and clear return stack
;

\ recalculates colors from stripdata
: triplett2byte ( triplettaddr - colorbyte )
    \ only checks second bit in triplett
    0                \ will be the colorbyte after or-ing the bits in loop  
    8 0 do
        over                  \ adress of bytetripletts
        i 3 * 1+              \ calc bit
        8 /mod
        2 swap - rot +        \ calc byte
        swap bit swap cbit@   \ check bit
        if
            i bit or
        then
    loop
    swap drop
;


\ Calculate number of bytes needed for a given number of LEDs
: led2bytes ( u - u ) 2 + 9 * ; \ one empty at beginning and end, 3 bytes per color times 3 colors for every led

\ in-memory buffer -> 9 bytes for every LED
MAX-LEDS led2bytes buffer: strip


\ --------------------------------------------------
\  External API
\ --------------------------------------------------

\ Sets color of an LED (r, g, b are 0-255 / index is 0-based)
: setpixel ( r g b index -- )
  1+ 9 * strip + >r
  r@ 2 writetriplett
  r@ 0 writetriplett
  r> 1 writetriplett
;

\ gets color of an led, color is r, g, b 
: getpixel ( index - r g b )
    1+ 9 * strip + >r
    r@ 3 + triplett2byte
    r@     triplett2byte
    r> 6 + triplett2byte
;


\ sets color of an led, color is packed rgb 24bit
: setpix ( rgb index - )
    1+ 9 * strip + >r
    dup $0000ff and
    r@ 2 writetriplett
    dup $00ff00 and 8 rshift
    r@ 0 writetriplett
    $ff0000 and 16 rshift
    r> 1 writetriplett
;

\ gets color of an led, color is packed 24 bit
: getpix ( index - rgb )
    1+ 9 * strip + >r
    r@ 3 + triplett2byte 16 lshift
    r@     triplett2byte 8 lshift or
    r> 6 + triplett2byte or
;


: led-all ( rgb -- )
  MAX-LEDS 0 do
    dup i setpix
  loop
  drop
;

: led-clear ( -- )
    $0 led-all
;



: led-show ( )
    0 bit DMA1-S4CR bic!             \ F4 Make sure channel is disabled
    %111101 DMA1-HIFCR bis!          \ F4 Clear all the interrupt flags for ch5
    MAX-LEDS led2bytes DMA1-S4NDTR !       \ 9 Bytes to transfer
    0 bit DMA1-S4CR bis!   \ Make sure channel is enabled
;

: led-init ( - )
    21 bit RCC_AHB1ENR bis!          \ F4 DMA1EN clock enable

    strip MAX-LEDS led2bytes $00 fill
    led-clear                        \ clear buffer

    0 DMA1-S4CR !                     \ F4 Make sure channel is disabled
    
    %111101 DMA1-HIFCR bis!          \ F4 Clear all the interrupt flags for ch5

    \ MAX-LEDS led2bytes DMA1-S4NDTR h! \ F4 number of bytes to transfer
    strip DMA1-S4M0AR !    \ F4 read from strip memory
    SPI2-DR DMA1-S4PAR !   \ F4 write to SPI2

    %000  25 lshift        \ F4 channel 0  
    %10   16 lshift or     \ F4 high priority
    %0000 11 lshift or     \ F4 8 bit transfers
    %10    9 lshift or     \ F4 auto-increment on memory address only
    %0     8 lshift or     \ F4 one-shot
    %1     6 lshift or     \ F4 read from memory
    %0000  1 lshift or     \ F4 no interrupts
    %1              or     \ F4 enable channel
    DMA1-S4CR ! \ F4

    \ now init spi2 and configure for DMA TX
    OMODE-AF-PP MOSI2 io-mode!
    %0101 28 lshift GPIOB GPIO.AFRH + bis! \ PB15 Alternate function 5
    

    14 bit RCC_APB1ENR bis!  \ set SPI2EN
    $04 SPI2 bis!   \ MSTR in SPI_CR1
    \ assuming 24MHz PCLK1
    $10 SPI2 bis!   \ spi speed 1/8 BR0:2  in SPI_CR1 (330ns per short impuls)
    8 bit SPI2 bis! \ SSI  in SPI_CR1
    9 bit SPI2 bis! \ SSM  in SPI_CR1
    6 bit SPI2 bis! \ SPE - enable  in SPI_CR1

    SPI2-SR @ drop         \ appears to be needed to avoid hang in some cases
    1 bit SPI2-CR2 bis!    \ enable DMA Tx

    led-show
;


\ --------------------------------------------------
\  Testing
\ --------------------------------------------------

\ Output memory buffer in internal format
\ : led. ( - )
\  MAX-LEDS 1+ 1 do
\    CR i h.2 ." :"
\    9 0 do ."  " strip j 9 * + i + c@ h.2 loop
\  loop
\ ;

\ Output leds in packed rgb format
: led. ( -- )
    MAX-LEDS 0 do
        CR i h.2 ." : "
        i getpix hex.
    loop
;


\ returns one of the colors for use in an animation
\ : colorwheel ( i - u u u )
\  5 mod case
\    0 of $ff $ff $ff endof
\    1 of $ff $00 $00 endof
\    2 of $00 $ff $00 endof
\    3 of $00 $00 $ff endof
\    4 of $00 $00 $00 endof
\  endcase ;

: colorwheel2 ( i - u u u )
    8 mod case
        0 of $ff $ff $ff endof
        1 of $ff $00 $00 endof
        2 of $ff $ff $00 endof
        3 of $00 $ff $00 endof
        4 of $00 $ff $ff endof
        5 of $00 $00 $ff endof
        6 of $ff $00 $ff endof
        7 of $00 $00 $00 endof
    endcase
;


\ output some demo data ( input is offset for animation)
: demodata ( i )
  MAX-LEDS 0 do
    dup i + colorwheel2 i setpixel
  loop led-show
;

\ : ringanimate ( - )
\  100 0 do
\    i demodata drop 200 ms
\  loop
\ ;
: ringanimate ( -- )
    0
    begin
        demodata 100 ms
        1+ 64 mod \ 64 needs to be adjusted if other colorwheel is used
        key?
    until
    drop
    led-clear led-show
;


\ pseudo random generator

$7a92764b variable seed

: prand ( -- u )
    seed @
    dup 13 lshift xor
    dup 17 rshift xor
    dup 5  lshift xor
    dup seed !
    57947 *
;

: prandrange ( u0 -- u1 ) \ random u1 will be smaller then u1
    prand um* nip
;

\ fire and burn with adjustable colors
: fire ( rgb -- rgb )
    MAX-LEDS 0 do
        60 prandrange >r
        dup $FF0000 and 16 rshift        \ get red part
           dup r@ -                      \ diff rnd
           swap i 32 */ - 0 max 255 min  \ scale according to pos 
        over $00FF00 and 8 rshift        \ get green part
           dup r@ -
           swap i 32 */ - 0 max 255 min
        2 pick $0000FF and               \ get blue part
           dup r> -
           swap i 32 */ - 0 max 255 min
        i setpixel                       \ write to strip-mem
    loop
    led-show
;

    
: burn ( rgb -- )
    begin
        [ifdef] readdht11
            readdht11 @ if
                0 readdht11 !
                dht11@ humi ! temp !
            then
        [then]
        fire
        150 50 prandrange - ms
        \ 200 ms
        key?
    until
    led-clear led-show
    drop \ delete rgb from stack
;

\ red fire example
\ $FF6010 burn
\ green fire example
\ $60FF10 burn
\ small blue example
\ $101020 burn \ blue seems to be very strong


\ up-down runner demo
: sec-cnt ( rgb index -- ) 
    dup 29 > if
        59 swap  -
        swap drop
        0 swap
    then
    setpix led-show
;

: led-run ( rgb -- )
    begin
        60 0 do
            dup i sec-cnt 
            200 ms
        loop
        key? \ should be done earlier, since it runs at least one complete loop
    until
    led-clear led-show
    drop
;




