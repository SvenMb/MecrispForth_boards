\ ---
\ subject: switch to 168MHz and start systicks for STM32F407 black boards
\ author: Sven Muehlberg
\ notice: this puts together what others (mostly Jean Claude Wippler) have created, thanks to to all of them for creating free open source software
\ copyright: this is public domain, feel free to do whatever you want
\ ---

( board start: ) here dup hex.


include conditional.fs
include disassembler-m3.fs
include hexdump.fs
include stm32f411.fs
include nvic.fs
include calltrace.fs
include io.fs

5  constant io-ports  \ A..E
: io.all ( -- )  \ display all the readable GPIO registers
  io-ports 0 do i 0 io io. loop ;

include pins100.fs

\ define onboard leds
PA6 constant LED0
PA7 constant LED1

\ define onboard switches
PE4 constant KEY0
PE3 constant KEY1
PA0 constant KEY_WKUP

include ids.fs
include lib/f407-168MHz-uart1.fs
include systick.fs
include cornerstone.fs

: calign ; \ not needed with stm32f4(?)

: hello ( -- ) flash-kb . ." KB STM32F407 #" hwid hex.
  flash-kb $400 * compiletoflash here -  flashvar-here compiletoram here - 
  ." ram: " . ." flash: " . ." bytes free " ;

: init
	['] ct-irq irq-fault !  \ show call trace in unhandled exceptions
	\ jtag-deinit
	168MHz
	1000 systick-hz
	hello ." ok." CR
;

compiletoram? not [if]
cornerstone eraseflash
[then]

include rtc.fs
include rtc_ui.fs
include lib/ili9341_fsmc.fs
include lib/graphics.fs
include lib/font8x6.fs

( board end: ) here dup hex.
( board size: ) swap - hex.
