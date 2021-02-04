\ ---
\ subject: add words for fsmc tft ili9341
\ author: Sven Muehlberg
\ copyright: this is public domain, feel free to do whatever you want
\ ---

( board_tft start: ) here dup hex.

\ add words for ili9341 fsmc tftt display
include lib/ili9341_fsmc.fs

\ add words for some further graphic primitive
include ../lib/graphic/graphics.fs

\ fonts incl. TFT specific words
include lib/font8x6.fs

: init
    init
    TFT-init
;

( board_tft end: ) here dup hex.
( board_tft size: ) swap - hex.
