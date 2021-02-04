# libs for stm32f407 black board



## f407-168Mhz-uart1.fs

source: Mecrisp Stellaris Distribution from Mathias Koch

https://sourceforge.net/projects/mecrisp/files/

mecrisp:/stm32f407/f407-pll-168MHz.txt

I changed the USART port to USART1. I removed some register definitions, which are already defined otherwise.

## ili9341_fsmc.fs

Source: Me

this provides fsmc access to the tft-display via some graphic primitives.



## font8x6.fs

source: Jean Claude Wipplers flib distribution, Me

jcw:/embello/explore/1608-forth/flib/mecrisp/graphics.fs

 https://github.com/jcw/jcw.github.io/blob/main/zips/embello-2020-03-31.zip

font and words are derived from jcw, adaptation/optimisation to this tft done by me



