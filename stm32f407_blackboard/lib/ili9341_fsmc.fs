\ ---
\ subject: words for tft-display on STM32f407VET6 black board
\ author: Sven Muehlberg
\ notice: 
\ copyright: this is public domain, feel free to do whatever you want
\ ---

(   TFT start: ) here dup hex.
$A0000000 constant FSMC_BCR \ BANK 1 is offset 0
$A0000004 constant FSMC_BTR

$60000000 constant TFT_reg
$60080000 constant TFT_ram \ A18 <-> RS stm32f407zet needs different addr!

PB1 constant TFT_BL

\ set colors
$ffff variable tft-fg \ black background
$0000 variable tft-bg \ white foreground

\ cursor for tft-emit
0 variable TFT-cursor 

\ calculate tft 16bit color from rgb
: rgb2tft ( r g b -- 16bit )
    $f8 and \ blue
    3 rshift
    swap
    $fc and \ green
    3 lshift or
    swap
    $f8 and
    8 lshift or
;

\ switch backlight on
: TFT-on ( -- )
    TFT_BL ios! 
;

\ switch backlight on
: TFT-off ( -- )
    omode-od TFT_BL io-mode!
    TFT_BL ioc! 
;

\ initialises FSMC port on black board for stm32f407VET6
: FSMC-init ( -- )

    \ TFT_BL init and off
    TFT-off
    
    \ enable peripheral clock (bit 0 FSMCEN)
    $01 RCC_AHB3ENR bis!
    
    \ enable port D and E (bit 3 GPIODEN and 4 GPIOEEN)
    3 bit 4 bit or RCC_AHB1ENR bis!
    
    \ set port D pins to AF PP and very high speed
    omode-af-pp-hs
    dup PD0 io-mode!  \ D2
    dup PD1 io-mode!  \ D3
    dup PD4 io-mode!  \ NOE
    dup PD5 io-mode!  \ NWE
    dup PD7 io-mode!  \ NE1
    dup PD8 io-mode!  \ D13
    dup PD9 io-mode!  \ D14
    dup PD10 io-mode! \ D15
    dup PD13 io-mode! \ A18
    dup PD14 io-mode! \ D0
    dup PD15 io-mode! \ D1
    
    \ set port E pins to alternative mode
    dup PE7 io-mode!  \ D4
    dup PE8 io-mode!  \ D5
    dup PE9 io-mode!  \ D6
    dup PE10 io-mode! \ D7
    dup PE11 io-mode! \ D8
    dup PE12 io-mode! \ D9
    dup PE13 io-mode! \ D10
    dup PE14 io-mode! \ D11
    PE15 io-mode! \ D12

    \ set pins to fsmc
    %1111 8 lshift
    %1111 12 lshift or
    %1111 24 lshift or
    not GPIOD GPIO.AFRL + bic!
    %1100 
    %1100 4 lshift or
    %1100 16 lshift or
    %1100 20 lshift or
    %1100 28 lshift or
    GPIOD GPIO.AFRL + bis!

    %1111 12 lshift
    %1111 16 lshift or
    not GPIOD GPIO.AFRH + bic!
    %1100
    %1100 4 lshift or
    %1100 8 lshift or
    %1100 20 lshift or
    %1100 24 lshift or
    %1100 28 lshift or
    GPIOD GPIO.AFRH + bis!
    
    %1111 28 lshift
    GPIOE GPIO.AFRL + bic!
    %1100 28 lshift
    GPIOE GPIO.AFRL + bis!
    
    $CCCCCCCC
    GPIOE GPIO.AFRH + !
    
    \ FSMC_Bank1->BTR0 = FSMC_BTR1_ADDSET_1 | FSMC_BTR1_DATAST_1;
    \ %0110             \ slow
    \ %0110 8 lshift or
    %0001               \ fast
    %0001 8 lshift or
    FSMC_BTR bis!
    
    \ FSMC_Bank1->BCR0 = FSMC_BCR1_MWID_0 | FSMC_BCR1_WREN | FSMC_BCR1_MBKEN;
    %01 4 lshift
    %1 12 lshift or
    %1 or
    FSMC_BCR bis!
;

\ set access to rectangular window on display 
: window ( x1 y1 x2 y2 -- deltax+1 deltay+1 )
    $2b TFT_reg h! \ y1 y2 addr
    2 pick
    dup 8 rshift
    TFT_ram h!
    $ff and
    TFT_ram h!
    dup 8 rshift
    TFT_ram h!
    dup $ff and
    TFT_ram h!
    
    $2a TFT_reg h! \ x1 x2 addr
    3 pick
    dup 8 rshift
    TFT_ram h!
    $ff and
    TFT_ram h!
    over
    dup 8 rshift
    TFT_ram h!
    $ff and
    TFT_ram h!
    
    rot - 1+
    -rot swap - 1+
    swap
;

\ write solid rectangle 
: rect ( col x1 y1 x2 y2 -- )
    window
    *
    $2c TFT_reg h! \ fill
    0 do 
        dup TFT_ram h!
    loop drop
;

\ clear screen with backgound color
: clear ( -- )
    tft-bg @ 0 0 320 240 rect \ for landscape
    \ tft-bg @ 0 0 240 320 rect \ for portrait
    0 TFT-cursor ! \ reset cursor 
;

\ initialises TFT, called after FSMC_init
: TFT-init ( -- )

    FSMC-init

    \ reset
    $01 TFT_reg h!
    100 ms
    \ display off
    $28 TFT_reg h!
    \ power 1
    $C0 TFT_reg h!
    $26 TFT_ram h!
    \ power2
    $C1 TFT_reg h!
    $11 TFT_ram h!
    \ VCOM1
    $C5 TFT_reg h!
    $35 TFT_ram h!
    $3e TFT_ram h!
    \ VCOM2
    $C7 TFT_reg h!
    $be TFT_ram h!

    \ MEM ACC
    $36 TFT_reg h!
    \ $48 TFT_ram h! \ 0 degree rotation!
    \ $28 TFT_ram h! \ 90 degree
    \ $88 TFT_ram h! \ 180 degree
    $E8 TFT_ram h! \ 270 degree
    
    \ pixel format
    $3A TFT_reg h!
    $55 TFT_ram h!

    \ frame rate
    $B1 TFT_reg h!
    $00 TFT_ram h!
    \ $1F TFT_ram h! \ 61Hz
    $1B TFT_ram h! \ 70Hz
    \ $15 TFT_ram h! \ 90Hz
    \ $13 TFT_ram h! \ 100Hz
    \ $10 TFT_ram h! \ 119Hz

    \ tearing off
    $34 TFT_reg h!
    
    \ Entry mode
    $B7 TFT_reg h!
    $07 TFT_ram h! \ normal disp

    \ function control
    $B6 TFT_reg h!
    $0a TFT_ram h!
    $82 TFT_ram h!
    $27 TFT_ram h!
    $00 TFT_ram h!

    \ sleep out
    $11 TFT_reg h!
    100 ms

    \ display on
    $29 TFT_reg h!
    100 ms

    clear
;

\ set just one pixel, wrapper to rectangle
: putpixel ( x y -- )
    tft-fg @ -rot
    2dup
    rect
;

\ only for compatibility
: display ( -- )
    TFT-on \ just in case it is off
    \ do nothing
;

( TFT end:   ) here dup hex.
( TFT size:  ) swap - hex.
