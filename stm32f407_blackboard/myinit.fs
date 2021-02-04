\ ---
\ subject: switch to 168MHz and start systicks for STM32F407 black boards
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

5  constant io-ports  \ A..E
: io.all ( -- )  \ display all the readable GPIO registers
  io-ports 0 do i 0 io io. loop ;

include ../lib/stm32/pkgs/pins100.fs

\ define onboard leds
PA6 constant LED0
PA7 constant LED1

\ define onboard switches
PE4 constant KEY0
PE3 constant KEY1
PA0 constant KEY_WKUP

include ../lib/stm32/f4/ids.fs
include lib/f407-168MHz-uart1.fs
include ../lib/stm32/systick.fs
include ../lib/stm32/f4/cornerstone.fs

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

\ add words for onboard clock
include ../lib/stm32/f4/rtc/rtc.fs
include ../lib/stm32/f4/rtc/rtc_ui.fs

( board end: ) here dup hex.
( board size: ) swap - hex.
