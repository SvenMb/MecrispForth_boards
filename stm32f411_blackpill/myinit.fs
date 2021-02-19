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

include ../lib/stm32/f4/ids.fs
include lib/96MHz.fs
include ../lib/stm32/systick.fs
include ../lib/stm32/f4/cornerstone.fs

: calign 
    align
; \ just aliased for some scripts

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
\ include i2c-bb.fs
\ include sh1106.fs
\ include graphics.fs \ small font

( board end: ) here dup hex.
( board size: ) swap - hex.
