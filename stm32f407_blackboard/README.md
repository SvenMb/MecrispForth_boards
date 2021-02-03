# Mecrisp forth for stm32f407vet6 black board

I have modified the forth-kernel to use the correct serial port uart1 on this board, I also enabled all the interupts dor forth use.
Thanks to Mathias Koch (the creator of mecrisp stellaris) this was very easy.

## Demo for fsmc TFT-display and onboard RTC

Please set time and date according to documentation in rtc.fs.

Example:
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

