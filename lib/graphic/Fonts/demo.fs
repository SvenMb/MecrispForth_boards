include unicode-4x4.fs 
include unicode-4x6.fs
include unicode-8x8.fs 
include unicode-8x16.fs 
include ascii-6x8.fs 
include ascii-c64.fs 

include ../charwriter.fs

i2c-init
lcd-init
clear display

: hw s" Hi World! üßñ€£¶" ;

['] fixed4x4 font-hook !
hw 0 0 drawstring display

['] fixed4x6 font-hook !
hw 0 6 drawstring display

['] ascii6x8 font-hook !
hw 0 14 drawstring display

['] fixed8x8 font-hook !
hw 0 24 drawstring display

['] c64_8x8 font-hook !
hw 0 34 drawstring display

['] fixed8x16 font-hook !
hw 0 44 drawstring display




