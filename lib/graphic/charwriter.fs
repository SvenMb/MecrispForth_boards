\ ---
\ subject: words for writing chars on display, just uses putpixel
\ origin: mecrisp2.5.8 distribution (common/graphics/graphics-unicode-8x16.txt)
\ author: Sven Muehlberg (
\ copyright: see mecrisp from Mathias Koch
\ changes: splitted graphics, char writing and fonts, made it more universal, some cleanup
\ ---
\

( charwriter start: ) here dup hex.

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

variable font-hook \ initialise with provided smallest font

\ -------------------------------------------------------------
\  Write an Unicode character bitmap font
\ -------------------------------------------------------------

: drawbitmap ( x y c-addr width pixels -- new_x new_y )
    0 do
        over \ get c-addr on top
        \ stack: c-addr width c-addr
        i 8 /mod
        rot + 
        swap negate 7 + \ wrong :) bitorder in provided fonts
        bit swap cbit@ \ gen mask and get bit
        \ i 2 pick mod 0= if CR then \ debug: new line 
        \ dup if [char] * else $20 then emit  \ debug: * if pix
        if \ pixel is set in char
            3 pick 3 pick
            i 3 pick /mod \ gets row and column in char
            rot +
            -rot + swap
            putpixel
        then
    loop
    swap drop \ drop c-addr
    rot + swap \ one char further
;

\ with correct bit order, just for test
: drawbitmap_r ( x y c-addr width pixels -- new_x new_y )
    0 do
        over \ get c-addr on top
        \ stack: c-addr width c-addr
        i 8 /mod
        rot + 
        swap \ negate 7 + \ correct :) bitorder 
        bit swap cbit@ \ gen mask and get bit
        \ i 2 pick mod 0= if CR then \ debug: new line 
        \ dup if [char] * else $20 then emit  \ debug: * if pix
        if \ pixel is set in char
            3 pick 3 pick
            i 3 pick /mod \ gets row and column in char
            rot +
            -rot + swap
            putpixel
        then
    loop
    swap drop \ drop c-addr
    rot + swap \ one char further
;

\ public:
\ utf16 aware drawchar (not utf8, use drawchar_u8 or drawstring for that)
: drawchar ( x y uc -- new_x new_y ) font-hook @ execute drawbitmap ;

\ -------------------------------------------------------------
\  Unicode UTF-8 encoding decoder
\ -------------------------------------------------------------

0 variable utf8collection
0 variable utf8continuation

: utf8-character-length ( c -- c u )
  dup %11000000 and %11000000 = if dup 24 lshift not clz else 1 then ;

\ public:
\ Handles a stream of UTF-8 bytes and translates this into Unicode letters.
: drawchar_u8 ( x y c -- new_x new_y )

  utf8continuation @
  if   \ Continue to receive an extended character into buffer

    %00111111 and utf8collection @ 6 lshift or utf8collection !  \ Six more bits
    -1 utf8continuation +!                                       \ One less continuation byte to expect
    utf8continuation @ 0= if utf8collection @ drawchar then   \ Draw character if complete encoding was buffered.

  else \ Begin of a new character

    utf8-character-length 1- ?dup

    if \ Start of a new character or a sequence
      dup utf8continuation !
      25 + tuck lshift swap rshift \ Remove the length encoding by shifting it out of the register temporarily
      utf8collection !
    else \ One byte characters are classic 7 bit ASCII and can be drawn immediately
      drawchar
    then

  then
;


\ -------------------------------------------------------------
\  Write a string and split into individual characters
\  utf8 aware
\ -------------------------------------------------------------

\ public:
: drawstring ( addr u x y -- )
    rot
    
    0 ?do 
        2 pick i + c@
        drawchar_u8 
    loop
    drop drop drop
;


\ -------------------------------------------------------------
\  A small demo
\ -------------------------------------------------------------

: demo ( -- )
  clear
  50 14 32 10 ellipse
  50 14 34 12 ellipse
  s" Mecrisp" 22 10 drawstring
  2 4 12 24 line
  4 4 14 24 line
  s" äüößÄÜÖ€§" 22 40 drawstring
  display
;

\ usage:
\ i2c-init
\ lcd-init
\ demo

( charwriter end: ) here dup hex.
( charwriter size: ) swap - hex.
