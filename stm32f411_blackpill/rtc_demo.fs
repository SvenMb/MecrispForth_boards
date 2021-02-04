 : rtcisr
    $0400 rtc_isr bit@ not if \ check if new second
	exit
    then
    1 22 lshift exti_pr bit@ not if \ check if new second
	exit
    then
	
    [ifdef] drawstring
      clear rtc-get-time ftime 0 4 drawstring rtc-get-date fdate 0 24 drawstring
      [ifdef] adc-temp
        adc-temp ftemp 0 44 drawstring 
      [then]
      display
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
