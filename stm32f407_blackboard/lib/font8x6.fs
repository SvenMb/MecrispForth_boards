\ ---
\ subject: words for handling character writing
\ author: Sven Muehlberg 2021
\ notice: this is mostly based on Jean Claud Wipplers flibs, which he placed in public domain, thanks for writing open source software!
\ copyright: this is public domain, feel free to do whatever you want
\ ---

\ 5x7 bitmap font, display on a 6x8 grid
\ this creates a 53 x 30 console

create font-5x7 hex
  00 c, 00 c, 00 c, 00 c, 00 c, \
  00 c, 00 c, 4F c, 00 c, 00 c, \ !
  00 c, 03 c, 00 c, 03 c, 00 c, \ "
  14 c, 3E c, 14 c, 3E c, 14 c, \ #
  24 c, 2A c, 7F c, 2A c, 12 c, \ $
  63 c, 13 c, 08 c, 64 c, 63 c, \ %
  36 c, 49 c, 55 c, 22 c, 50 c, \ &
  00 c, 00 c, 07 c, 00 c, 00 c, \ '
  00 c, 1C c, 22 c, 41 c, 00 c, \ (
  00 c, 41 c, 22 c, 1C c, 00 c, \ )
  0A c, 04 c, 1F c, 04 c, 0A c, \ *
  04 c, 04 c, 1F c, 04 c, 04 c, \ +
  50 c, 30 c, 00 c, 00 c, 00 c, \ ,
  08 c, 08 c, 08 c, 08 c, 08 c, \ -
  60 c, 60 c, 00 c, 00 c, 00 c, \ .
  00 c, 60 c, 1C c, 03 c, 00 c, \ /
  3E c, 41 c, 49 c, 41 c, 3E c, \ 0
  00 c, 02 c, 7F c, 00 c, 00 c, \ 1
  46 c, 61 c, 51 c, 49 c, 46 c, \ 2
  21 c, 49 c, 4D c, 4B c, 31 c, \ 3
  18 c, 14 c, 12 c, 7F c, 10 c, \ 4
  4F c, 49 c, 49 c, 49 c, 31 c, \ 5
  3E c, 51 c, 49 c, 49 c, 32 c, \ 6
  01 c, 01 c, 71 c, 0D c, 03 c, \ 7
  36 c, 49 c, 49 c, 49 c, 36 c, \ 8
  26 c, 49 c, 49 c, 49 c, 3E c, \ 9
  00 c, 33 c, 33 c, 00 c, 00 c, \ :
  00 c, 53 c, 33 c, 00 c, 00 c, \ ;
  00 c, 08 c, 14 c, 22 c, 41 c, \ <
  14 c, 14 c, 14 c, 14 c, 14 c, \ =
  41 c, 22 c, 14 c, 08 c, 00 c, \ >
  06 c, 01 c, 51 c, 09 c, 06 c, \ ?
  3E c, 41 c, 49 c, 15 c, 1E c, \ @
  78 c, 16 c, 11 c, 16 c, 78 c, \ A
  7F c, 49 c, 49 c, 49 c, 36 c, \ B
  3E c, 41 c, 41 c, 41 c, 22 c, \ C
  7F c, 41 c, 41 c, 41 c, 3E c, \ D
  7F c, 49 c, 49 c, 49 c, 49 c, \ E
  7F c, 09 c, 09 c, 09 c, 09 c, \ F
  3E c, 41 c, 41 c, 49 c, 7B c, \ G
  7F c, 08 c, 08 c, 08 c, 7F c, \ H
  00 c, 41 c, 7F c, 41 c, 00 c, \ I
  38 c, 40 c, 40 c, 41 c, 3F c, \ J
  7F c, 08 c, 08 c, 14 c, 63 c, \ K
  7F c, 40 c, 40 c, 40 c, 40 c, \ L
  7F c, 06 c, 18 c, 06 c, 7F c, \ M
  7F c, 06 c, 18 c, 60 c, 7F c, \ N
  3E c, 41 c, 41 c, 41 c, 3E c, \ O
  7F c, 09 c, 09 c, 09 c, 06 c, \ P
  3E c, 41 c, 51 c, 21 c, 5E c, \ Q
  7F c, 09 c, 19 c, 29 c, 46 c, \ R
  26 c, 49 c, 49 c, 49 c, 32 c, \ S
  01 c, 01 c, 7F c, 01 c, 01 c, \ T
  3F c, 40 c, 40 c, 40 c, 7F c, \ U
  0F c, 30 c, 40 c, 30 c, 0F c, \ V
  1F c, 60 c, 1C c, 60 c, 1F c, \ W
  63 c, 14 c, 08 c, 14 c, 63 c, \ X
  03 c, 04 c, 78 c, 04 c, 03 c, \ Y
  61 c, 51 c, 49 c, 45 c, 43 c, \ Z
  00 c, 7F c, 41 c, 00 c, 00 c, \ [
  00 c, 03 c, 1C c, 60 c, 00 c, \ \
  00 c, 41 c, 7F c, 00 c, 00 c, \ ]
  0C c, 02 c, 01 c, 02 c, 0C c, \ ^
  40 c, 40 c, 40 c, 40 c, 40 c, \ _
  00 c, 01 c, 02 c, 04 c, 00 c, \ `
  20 c, 54 c, 54 c, 54 c, 78 c, \ a
  7F c, 48 c, 44 c, 44 c, 38 c, \ b
  38 c, 44 c, 44 c, 44 c, 44 c, \ c
  38 c, 44 c, 44 c, 48 c, 7F c, \ d
  38 c, 54 c, 54 c, 54 c, 18 c, \ e
  08 c, 7E c, 09 c, 09 c, 00 c, \ f
  0C c, 52 c, 52 c, 54 c, 3E c, \ g
  7F c, 08 c, 04 c, 04 c, 78 c, \ h
  00 c, 00 c, 7D c, 00 c, 00 c, \ i
  00 c, 40 c, 3D c, 00 c, 00 c, \ j
  7F c, 10 c, 28 c, 44 c, 00 c, \ k
  00 c, 00 c, 3F c, 40 c, 00 c, \ l
  7C c, 04 c, 18 c, 04 c, 78 c, \ m
  7C c, 08 c, 04 c, 04 c, 78 c, \ n
  38 c, 44 c, 44 c, 44 c, 38 c, \ o
  7F c, 12 c, 11 c, 11 c, 0E c, \ p
  0E c, 11 c, 11 c, 12 c, 7F c, \ q
  00 c, 7C c, 08 c, 04 c, 04 c, \ r
  48 c, 54 c, 54 c, 54 c, 24 c, \ s
  04 c, 3E c, 44 c, 44 c, 00 c, \ t
  3C c, 40 c, 40 c, 20 c, 7C c, \ u
  1C c, 20 c, 40 c, 20 c, 1C c, \ v
  1C c, 60 c, 18 c, 60 c, 1C c, \ w
  44 c, 28 c, 10 c, 28 c, 44 c, \ x
  46 c, 28 c, 10 c, 08 c, 06 c, \ y
  44 c, 64 c, 54 c, 4C c, 44 c, \ z
  00 c, 08 c, 77 c, 41 c, 00 c, \ {
  00 c, 00 c, 7F c, 00 c, 00 c, \ |
  00 c, 41 c, 77 c, 08 c, 00 c, \ }
  10 c, 08 c, 18 c, 10 c, 08 c, \ ~
calign decimal

: ascii>bitpattern ( c -- c-addr ) \ Translates ASCII to address of bitpatterns.
    32 umax 127 umin  32 - 5 * font-5x7 +  1-foldable
;

: drawchar ( c x y -- )
    over 5 +
    over 7 +
    window 2drop
    ascii>bitpattern
    $2c TFT_reg h! \ fill
    8 0 do
        5 0 do
            j bit over i + cbit@
            if
                tft-fg
            else
                tft-bg
            then
            @ TFT_ram h!
        loop
        tft-bg @ TFT_ram h!
    loop drop
;

: drawstring ( addr u x y -- )
    rot \ addr x y u 
    0 ?do \ addr x y
        2 pick i + c@
        2 pick
        2 pick
        drawchar
        swap 6 + swap
    loop drop drop drop
;

\ very basic output 1 char to tft
: tft-emit ( c -- )
    TFT-cursor @
    53 /mod
    8 * \ y
    swap
    6 * 1+ \ x
    swap
    drawchar
    1 TFT-cursor +!
;
