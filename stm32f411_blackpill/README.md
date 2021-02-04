---
subject: Installation of basic mecrisp forth system on stm32f411
author: Sven Mühlberg
---

**currently not ready for use, need to upload working initial release**

# STM32F411 mecrisp Forth Basic Installation

* serial port is RX - PA3, TX - PA2
* USB is currently not usable in forth

## assemble and install your own kernal

*Own kernal is needed, since a lot of interupt vectors are not declared as of mecrisp 2.5.4a*

* put interrupt.s and vector.s into source directory
* make new kernal
* install on stm32f411 with st-link 

> cp *.s $mecrisp/mecrisp-stellaris-source/stm32f411-ra/
> cd $mecrisp/mecrisp-stellaris-source/stm32f411-ra/
> make clean
> make all
> ls -l mecrisp-stellaris-stm32f411.bin # check if there and ~20k size
> st-flash erase i                      # erase full flash 
> st-flash write mecrisp-stellaris-stm32f411.bin 0x08000000 # writes flash
> folie -r /dev/ttyUSB0 # adjust port, check if REPL is avaiable


## install forth words for basic usage

### install conditional compiling

conditional.fs: source $mecrisp/common/conditional.txt - no changes

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s conditional.fs
> compiletoram

### install disassembler (even if not complete for m4) and dump util

disassembler-m3.fs: source $mecrisp/common/disassembler-m3.txt - no changes
hexdump.fs: source $jcw/flib/mecrisp/hexdump.fs - conditional for u.4 added, need adjust hexdump

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s disassembler-m3.fs
> !s hexdump.fs  \ TODO: export flash from stm32f411
> compiletoram


### definitions for stm32f4 registers and interrupts

stm32f411.fs: source $mecrisp/stm32f411/stm32f411.fs - no changes, just removed compiletoflash
nvic.fs: source $mecrisp/common/nvic.txt - changed to already defined register names
calltrace: source $mecrisp/common/calltrace.txt - no changes

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s stm32f411.fs
> !s nvic.fs
> !s calltrace.fs
> compiletoram

### definitions for simple IO from jcw

io.fs: source $jcw/flib/stm32f4/io.fs - already defined io-base included
pins48.fs: source $jcw/flib/pkg/pins48.fs - no change

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s io.fs
> !s pins48.fs
> compiletoram

### systick, 96MHz switch, cornerstone, ChipIDs

ids.fs: source $jcw/flib/stm32f4/hal.fs - extracted only the id part
96mhz.fs: source $mecrisp/stm32f411-ra/96mhz.fs - simplified
systick.fs: source $mecrisp/stm32f411-ra/systick.fs - adapted to jcw style, added some words like us and ms
cornerstone.fs: source $mecrisp/stm32f411-ra/cornerstone.fs - no change

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s ids.fs
> !s 96mhz.fs
> !s systick.fs
> !s cornerstone.fs
> compiletoram

### myinit

* loads all of the above
* just a new init to activate 96MHz and systicks after reboot
* provides new hello flash/ram usage info
* creates cornerstone eraseflash

myinit.fs: 

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s myinit.fs
> compiletoram

### rtc.fs - integrated real time clock

rtc.fs: source $mecrisp/stm32f411/rtc.fs - no changes

rtc_ui.fs: own work - formting time and date

> folie -r /dev/ttyUSB0 # or whatever port you use
> compiletoflash
> !s rtc.fs
> !s rtc_ui.fs
> compiletoram

### adc.fs - Analog input

adc.fs: source $jcw

### i2c-bb.fs - i2c driver

i2c-bb.fs: source $jcw/flib/any/i2c-bb.fs

folie -r /dev/ttyUSB0 # or whatever port you use
compiletoflash
!s rtc.fs
compiletoram

### sh1106.fs

oled driver

### graphics.fs - graphics for

* graphic and text for oled or whatever 

graphics.fs: source $jcw/flib/mecrisp/graphics.fs  
other graphics.fs with different fonts in $mecrisp/common/graphics*.fs work too
