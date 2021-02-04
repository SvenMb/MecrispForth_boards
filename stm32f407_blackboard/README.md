# Mecrisp forth for stm32f407vet6 black board (with optional ili9341 fsmc tft display)

## console connection

serial connection:
 - USART1 PA9 - TX, PA10 - RX (extra serial header)
 
usb-connection is work in progress 

## Driver for supplied FSMC-display

There is an nice TFT-display with touch avaiable for this board. I wrote forth words (driver) for that.

hardware driver:
 - lib/ili9341_fsmc.fs 
 
line/ellipse/circle:
 - ../lib/graphic/graphics.fs
 
character writer:
 - lib/font8x6.fs 

## Demo for fsmc TFT-display and onboard RTC

Please set time and date according to documentation in rtc.fs.

Code:
```forth
21 24 20 rtc-set-time
21 3 02 03 rtc-set-date
```

Then import demo.fs via folie and start demo.
```forth
!s demo.fs
demo
```

You should get the following output:
![demo.fs output](img/IMG_20210203_212411.jpg)

