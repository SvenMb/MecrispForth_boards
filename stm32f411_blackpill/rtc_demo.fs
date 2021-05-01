$ffffff variable tcolor
$0 variable temp
$0 variable humi
$1 variable leddemo
$0 variable readdht11

: rtcisr
    $0400 rtc_isr bit@ not if \ check if new second
	exit
    then
    1 22 lshift exti_pr bit@ not if \ check if new second
	exit
    then
	
    [ifdef] drawstring
        clear
        s" Time:" 0 4  drawstring rtc-get-time ftime 36 4  drawstring
        s" Date:" 0 20 drawstring rtc-get-date fdate 36 20 drawstring
        \ my dht11@ word doesn't work when started from here
        \ probably systick isn't working from here
      [ifdef] DHT11@
          rtc-get-time 10 mod 0= if \ every 10 sec
              1 readdht11 c!
          then
          drop drop \ delete hours and minutes from stack
          s" Temp:"  0 36 drawstring
          temp @ 500 min
          s>d swap over dabs <# # $2E hold #S rot sign #>
          36 36 drawstring
          s" Humi:" 64 36 drawstring
          humi @ 1000 min
          0 <# # $2E hold #S #>
          100 36 drawstring
      [then]
      [ifdef] adc-temp
        s" cpu:" 0 52 drawstring  adc-temp     ftemp 36 52 drawstring 
      [then]
      display 
      [ifdef] sec-cnt
          leddemo @ 1 = if
              rtc-get-time tcolor @ swap sec-cnt
              drop drop
          then
      [then]
      [ifdef] fire
          leddemo @ 2 = if
              tcolor @
              fire
              drop
          then
      [then]
    [else]
      CR time. $20 emit date.
      [ifdef] adc-temp
        CR adc-temp. $20 emit adc-vbat adc-vbat adc.
      [then]
    [then]
    $0400 rtc_isr bic! \ reset wakeup flag
    1 22 lshift exti_pr bis! \ reset bit in exti
;

: demo
    [ifdef] lcd-init
      lcd-init show-logo
    [then]
    [ifdef] adc-init
      adc-init
      CR ." ADC initialized"
    [then]
    [ifdef] led-init
        led-init
    [then]
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
;
