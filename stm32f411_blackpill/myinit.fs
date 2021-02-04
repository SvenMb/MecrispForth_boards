\ ---
\ subject: switch to 96MHz and start systicks for STM32F411 boards
\ author: Sven Muehlberg
\ ---

( board start: ) here dup hex.


include conditional.fs
include disassembler-m3.fs
include hexdump.fs
include stm32f411.fs
include nvic.fs
include calltrace.fs
include io.fs

3  constant io-ports  \ A..C
: io.all ( -- )  \ display all the readable GPIO registers
  io-ports 0 do i 0 io io. loop ;

include pins48.fs

include ids.fs
include 96MHz.fs
include systick.fs
include cornerstone.fs

: calign ; \ not needed with stm32f411?

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

include rtc.fs
include rtc_ui.fs
include i2c-bb.fs
include sh1106.fs
include graphics.fs \ small font

( board end: ) here dup hex.
( board size: ) swap - hex.
