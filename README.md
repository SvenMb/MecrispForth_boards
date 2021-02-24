# MecrispForth board collection 

### basis installations for some microcontroller boards with display via i2c or fsmc bus

 - stm32f407_blackboard is ready for use
 - stm32f411_blackpill is ready for use (documentation is a mess)

## Driver/Words for some peripherals
### Neopixel driver for stm32f4

Source:
/lib/ext/neopix/ws2812b_stm32f4.fs

 - working DMA based driver
 - currently tested with stm32f411 board

### Example TSOP1838 driver for stm32f4

Source:
/lib/ext/TSOP1838/TSOP1838_f4.fs

- works via interrupt
