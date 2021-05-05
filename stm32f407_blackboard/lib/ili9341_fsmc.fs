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

\ public:
\ set colors
$ffff variable DISPLAY-fg \ white foreground
$0000 variable DISPLAY-bg \ black background

\ public:
\ cursor for disp-emit
0 variable DISPLAY-pos

\ public
\ tft size
\ probaly not a good idea to make constants??? (pivot!)
320 constant disp_x
256 constant disp_y

\ public:
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

\ public:
\ switch backlight on
: DISPLAY-on ( -- )
    TFT_BL ios!
;

\ public:
\ switch backlight on
: DISPLAY-off ( -- )
    omode-od TFT_BL io-mode!
    TFT_BL ioc! 
;


\ initialises FSMC port on black board for stm32f407VET6
: FSMC-init ( -- )

    \ TFT_BL init and off
    DISPLAY-off
    
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

\ public:
\ write solid rectangle 
: rect ( col x1 y1 x2 y2 -- )
    window
    *
    $2c TFT_reg h! \ fill
    0 do 
        dup TFT_ram h!
    loop drop
;

\ public:
\ clear screen with backgound color
: clear ( -- )
    DISPLAY-bg @ 0 0 disp_x 1- disp_y 1- rect \ for landscape
    0 DISPLAY-pos ! \ reset cursor 
;

\ public:
\ initialises TFT
: DISPLAY-init ( -- )

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

\ public:
\ set just one pixel, wrapper to rectangle
: putpixel ( x y -- )
    DISPLAY-fg @ -rot
    2dup
    rect
;

\ public:
\ reset just one pixel, wrapper to rectangle
: clrpixel
    DISPLAY-bg @ -rot
    2dup
    rect
;

\ public:
\ only for compatibility
: display ( -- )
    DISPLAY-on \ just in case it was off
    \ do nothing
;

\ -------------------------------------
\ bitmap writer for font or icon usage
\ -------------------------------------

\ -------------------------------------------------------------
\ font-hook
\ provided function should be unicode aware
\ (and should deliver 'unprintable>-char if it can't find the char)
\ font-mapper synopsis:
\ font-mapper ( uc -- c-addr width pixels )
\ uc -> 16bit unicode char#
\ c-addr -> begin of bytes for bitmap  
\ width of that char
\ pixel -> total amount of pixels of that char
\ -------------------------------------------------------------

\ initialization for some basic fonts if provided, else dummyfont
[ifdef] fixed8x8 \ first choice
    ['] fixed8x8
[else]
    [ifdef] fixed4x6 \ second choice
        ['] fixed4x6
    [else]
        [ifdef] fixed8x16 \ last choice, else dummy
            ['] fixed8x16
        [else] \ no known font installed, create dummyfont
            create dummyfont
            $E0AAAA0E ,

            : dummymap ( uc - c-addr width pixels )
                drop
                dummyfont 4 32
            ;
        
            ['] dummymap
        [then]
    [then]
[then]

variable font-hook \ initialise with a known font

\ -------------------------------------------------------------
\  Write an Unicode character bitmap font
\ -------------------------------------------------------------

: drawbitmap ( x y c-addr width pixels -- )
    >r >r        \ save pixels, width to return stack
    -rot         \ put c-addr back
    2r@ /        \ get pixels, width and calc delta y
    \ stack: c-addr x y dy  rstack: pixels, width 
    over + 1-    \ calc y2
    \ stack: c-addr x y y2    
    2 pick r> + 1- \ calc x2
    swap         \ sort x2, y2
    \ stack: c-addr x y x2 y2 rstack: pixels
    window 2drop \ create window for pixmap on tft
    $2c TFT_reg h!   \ fill cmd for tft
    r>           \ get pixels
    0 do
        i 8 /mod \
        2 pick +    \ calc c-addr for current bit
        swap
        \ stack: c-addr c-addr-bit bit#
        negate 7 + \ wrong :) bitorder in provided fonts
        bit swap cbit@  \ gen mask and get bit
        if
            DISPLAY-fg
        else
            DISPLAY-bg
        then
        @ TFT_ram h!
    loop
    drop \ drop c-addr
;


\ public:
\ utf16 aware drawchar (not utf8, use drawchar_u8 or drawstring for that)
: drawchar ( x y uc -- new_x new_y )
    over  >r \ save y
    2 pick >r \ save x
    font-hook @ execute
    \ stack: x y c-addr width pixels
    over r> + >r \ calc new_x
    drawbitmap
    r> r> \ get new_x, y from r-stack
;

( TFT end:   ) here dup hex.
( TFT size:  ) swap - hex.
