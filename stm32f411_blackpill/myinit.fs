\ ---
\ subject: switch to 96MHz and start systicks for STM32F411 boards
\ author: Sven Muehlberg
\ notice: this puts together what others (mostly Jean Claude Wippler) have created, thanks to to all of them for creating free open source software
\ copyright: this is public domain, feel free to do whatever you want
\ ---

( board start: ) here dup hex.


include ../lib/mecrisp/conditional.fs
include ../lib/mecrisp/disassembler-m3.fs
include ../lib/mecrisp/hexdump.fs
include ../lib/stm32/f4/stm32f4.fs
include ../lib/stm32/nvic.fs
include ../lib/mecrisp/calltrace.fs
include ../lib/stm32/io.fs

3  constant io-ports  \ A..C
: io.all ( -- )  \ display all the readable GPIO registers
  io-ports 0 do i 0 io io. loop ;

include ../lib/stm32/pkgs/pins48.fs

\ define onboard leds
PC13 constant LED0 \ low active

\ define onboard switches
PA0 constant KEY_WKUP \ to GND

include ../lib/stm32/f4/ids.fs
include lib/96MHz.fs
include ../lib/stm32/systick.fs
include ../lib/stm32/f4/cornerstone.fs

\ align to halfword 
: calign 
    here 1 and 
    if
        0 c,
    then
; 

: hello ( -- ) flash-kb . ." KB STM32F411 #" hwid hex.
  flash-kb $400 * compiletoflash here -  flashvar-here compiletoram here - 
  ." ram: " . ." flash: " . ." bytes free " ;

: init
	['] ct-irq irq-fault !  \ show call trace in unhandled exceptions
	\ jtag-deinit
	96MHz
	1000 systick-hz
	hello ." ok." CR
;

compiletoram? not [if]
cornerstone eraseflash
[then]

\ add words for onboard clock
include ../lib/stm32/f4/rtc/rtc.fs
include ../lib/stm32/f4/rtc/rtc_ui.fs
\ graphics need cleanup, but hardware is working
\ include ../lib/stm32/i2c-bb.fs \ bit banged driver
\ include ../lib/ext/OLed/sh1106.fs  \ OLED hardware driver
\ include ../lib/ext/OLed/sdd1306.fs \ or this
\ include ../lib/ext/OLed/ssd1327.fs \ or this
\ include ../lib/graphic/graphics.fs \ line, ellipse, circle...
\ include ../lib/graphic/Fonts/unicode-4x6.fs \ very small Fonts
\ include ../lib/graphic/Fonts/unicode-8x8.fs \ very small Fonts


( board end: ) here dup hex.
( board size: ) swap - hex.
