\ ---
\ subject: demo for fsmc graphics display ili9341
\ author: Sven Muehlberg
\ ---

(   TFT start: ) here dup hex.

[ifdef] rtc-init
    : rtcisr
        $0400 rtc_isr bit@ not if \ check if new second
            exit
        then
        1 22 lshift exti_pr bit@ not if \ check if new second
            exit
        then
        rtc-get-time ftime 254 223 drawstring
        rtc-get-date fdate 254 232 drawstring
        [ifdef] LED0
            LED0 iox!
        [then]
        [ifdef] LED1
            LED1 iox!
        [then]
        $0400 rtc_isr bic! \ reset wakeup flag
        1 22 lshift exti_pr bis! \ reset bit in exti
    ;
[then]


: demo ( -- )
    tft-init tft-on

    48 48 48 rgb2tft tft-bg !
    clear
    $0000 tft-bg !
    12 1 do $ffff 0 i 20 * 319 i 20 * rect loop
    16 1 do $ffff i 20 * 0 i 20 * 239 rect loop
    $0000 101 101 219 139 rect
    160 120 120 circle
    160 120 32 10 ellipse display
    160 120 34 12 ellipse display
    s" Mecrisp" 140 116 drawstring display
    112 110 122 134 line display
    114 110 124 134 line display
    $ff $ff $ff rgb2tft  61 21 99 39 rect 
    $C0 $C0 $C0 rgb2tft  61 40 99 59 rect
    $80 $80 $80 rgb2tft  61 60 99 79 rect
    $00 $ff $00 rgb2tft  100 21 139 39 rect 
    $60 $FF $60 rgb2tft  100 40 139 59 rect
    $40 $C0 $40 rgb2tft  100 60 139 79 rect
    $ff $00 $00 rgb2tft  140 21 179 39 rect 
    $FF $60 $60 rgb2tft  140 40 179 59 rect
    $c0 $40 $40 rgb2tft  140 60 179 79 rect
    $00 $00 $ff rgb2tft  180 21 219 39 rect 
    $60 $60 $FF rgb2tft  180 40 219 59 rect
    $40 $40 $c0 rgb2tft  180 60 219 79 rect
    $80 $80 $80 rgb2tft  220 60 259 79 rect
    $40 $40 $40 rgb2tft  220 40 259 59 rect
    $00 $00 $00 rgb2tft  220 21 259 39 rect
    $00 $00 $00 rgb2tft  61 81 79 119 rect 
    $1c $1c $1c rgb2tft  80 81 99 119 rect 
    $38 $38 $38 rgb2tft  100 81 119 99 rect 
    $54 $54 $54 rgb2tft  120 81 139 99 rect 
    $70 $70 $70 rgb2tft  140 81 159 99 rect 
    $8c $8c $8c rgb2tft  160 81 179 99 rect 
    $a8 $a8 $a8 rgb2tft  180 81 199 99 rect 
    $c4 $c4 $c4 rgb2tft  200 81 219 99 rect 
    $E0 $e0 $e0 rgb2tft  220 81 220 99 rect 
    $E0 $e0 $e0 rgb2tft  221 81 239 119 rect 
    $ff $ff $ff rgb2tft  240 81 259 119 rect
    $00 $80 $80 rgb2tft  61 121 99 149 rect
    $80 $80 $00 rgb2tft  221 121 259 149 rect
    260 61 do
        i 10 - 0 0 rgb2tft  i 141 i 150 rect
    loop
    260 61 do
        260 i - dup 0 rgb2tft  i 151 i 160 rect
    loop
    260 61 do
        0 i 10 - 0 rgb2tft  i 161 i 170 rect
    loop
    260 61 do
        0 260 i - dup rgb2tft  i 171 i 180 rect
    loop
    260 61 do
        0 0 i 10 - rgb2tft  i 181 i 190 rect
    loop
    260 61 do
        260 i - dup 0 swap rgb2tft  i 191 i 199 rect
    loop
    $ff $ff $ff rgb2tft   61 201  99 219 rect 
    $ff $ff $00 rgb2tft  100 201 139 219 rect 
    $ff $00 $ff rgb2tft  140 201 179 219 rect 
    $00 $ff $ff rgb2tft  180 201 219 219 rect 
    $00 $00 $00 rgb2tft  220 201 259 219 rect

    [ifdef] rtc-init
        [ifdef] LED0
            omode-od LED0 io-mode!
            LED0 ios!
        [then]
        [ifdef] LED1
            omode-od LED1 io-mode!
            LED1 ioc!
        [then]
        $0000 241 221 319 239 rect
        rtc-get-time ftime 254 223 drawstring
        rtc-get-date fdate 254 232 drawstring

        rtc-init
        CR ." RTC initialised"
        ['] rtcisr irq-rtc ! \ write my isr to irq vector tab
        3 nvic-enable
        1 22 lshift exti_rtsr bis!
        1 22 lshift exti_imr bis! 
        
        CR ." disable wakeup"
        $0400 rtc_cr bic! \ disable wakeup
        begin
            $0004 rtc_isr bit@ 
        until
        CR ." set count to 1s"
        0 rtc_wutr h!            \ only count to 1 
        $0004 rtc_cr bis!        \ enable second wakeup rtc-cr
        CR ." enable wakeup RTC-irq"
        $0400 rtc_cr bis!        \ enable wakeup timer
        $4000 rtc_cr bis!        \ enable wakeup timer irq
        CR ." reset wakup flag"
        $0400 rtc_isr bic!       \ reset wakeup flag
        CR ." done"
    [then]
;

( TFT end:   ) here dup hex.
( TFT size:  ) swap - hex.
