\ ---
\ subject: add words for fsmc tft ili9341
\ author: Sven Muehlberg
\ copyright: this is public domain, feel free to do whatever you want
\ ---

( board_tft start: ) here dup hex.

\ add fonts
include ../lib/graphic/Fonts/unicode-4x4.fs 
include ../lib/graphic/Fonts/unicode-4x6.fs
include ../lib/graphic/Fonts/unicode-8x8.fs 
include ../lib/graphic/Fonts/unicode-8x16.fs 
include ../lib/graphic/Fonts/ascii-6x8.fs 
include ../lib/graphic/Fonts/ascii-c64.fs 
\ include iso8859-15-vga.fs
\ include kc85.fs
\ include kc85-4x8.fs

\ add words for ili9341 fsmc tftt display
include lib/ili9341_fsmc.fs

\ add words for some further graphic primitive
include ../lib/graphic/graphics.fs

\ drawstring
include ../lib/graphic/charwriter.fs


\ touch
include lib/touch.fs

: init
    init
    DISPLAY-init
    touch-init
;

( board_tft end: ) here dup hex.
( board_tft size: ) swap - hex.
