# MecrispForth_OLED

words for OLED-displays

these words are derivated from Jean-Claude Wipplers [ssd1306.fs](https://git.jeelabs.org/jcw/embello/src/branch/master/explore/1608-forth/flib/i2c/ssd1306.fs), but that one didn't exactly work with my ssd1306.fs so I had to change the init function. As I was on it I also wrote words for a sh1106 and a ssd1327 OLED-displays. Fortunately this means they all work with jcws [graphics.fs](https://git.jeelabs.org/jcw/embello/src/branch/master/explore/1608-forth/flib/mecrisp/graphics.fs). Which is very usefull, since it provides basic graphic and font writing capabilities.

## ssd1306

small changes for my ssd1306, the original version from jcw didn't work.

## sh1106

support for sh1106 displays which need a bit different init and is internaly 132 wide organized

## ssd1327

support for ssd1327 displays which allows 4 bit grayscale per pixel and bigger resolutions.
