# libs for stm32



## io.fs

defines io primitives for stm32

source: Jean Claude Wipplers flib distribution

https://github.com/jcw/jcw.github.io/blob/main/zips/embello-2020-03-31.zip

jcw:/embello/explore/1608-forth/flib/stm32f4/io.fs

font and words are derived from jcw, removed already otherwise defined io-base

## nvic.fs

interrupt controller tools

source: Mecrisp Stellaris Distribution from Mathias Koch

https://sourceforge.net/projects/mecrisp/files/

mecrisp:/common/nvic.txt

## systick.fs

provides systick interrupt, some wait primitives like **ms** or **us**

source: Mecrisp Stellaris Distribution from Mathias Koch, added some words from jcw

https://sourceforge.net/projects/mecrisp/files/

mecrisp:/stm32f411/systick.fs