\ f (VCO clock) = f (PLL clock input) * (PLLN/PLLM)
\ f (PLL general clock output) = F (VCO clock) / PLLP
\ f (USB, RNG und andere) = f (VCO clock) / PLLQ
\   compiletoflash
  1 24 lshift constant PLLON
  1 25 lshift constant PLLRDY
  1 22 lshift constant PLLSRC
  1 16 lshift constant HSEON
  1 17 lshift constant HSERDY
USART2 $8 + constant USART2_BRR

\ was missing in original
[ifndef] clock-hz
16000000 variable clock-hz \ systemclock is 16MHz after reset
[then]

115200 variable uart2-baud




: 96MHz ( -- )
      HSEON RCC_CR bis!
      begin HSERDY RCC_CR bit@ until
      \ Set Flash waitstates !
      $104 Flash_ACR !   \ 3 Waitstates for 100 MHz with more than 2.7 V Vcc, Prefetch buffer enabled.
      PLLSRC          \ HSE clock as 25 MHz source
     25  0 lshift or  \ PLLM Division factor for main PLL and audio PLL input clock 
    192  6 lshift or  \ PLLN Main PLL multiplication factor for VCO - between 192 and 432 MHz
      4 24 lshift or  \ PLLQ = 4, 96 MHz / 4 = 24 MHz
      0 16 lshift or  \ PLLP Division factor for main system clock
                      \ 0: /2  1: /4  2: /6  3: /8
                      \ 96 MHz / 2 = 48 MHz 
      RCC_PLLCFGR !
      PLLON RCC_CR bis!
      \ Wait for PLL to lock:
      begin PLLRDY RCC_CR bit@ until

      2                 \ Set PLL as clock source
                        \ 0xx: AHB clock not divided
                        \ 100: AHB clock divided by 2
                        \ 101: AHB clock divided by 4
                        \ 110: AHB clock divided by 8
                        \ 111: AHB clock divided by 16
      %101 10 lshift or \ APB  Low speed prescaler (APB1) 96/4=24 - Max 42 MHz !
      %100 13 lshift or \ APB High speed prescaler (APB2) 96/2=48 - Max 90 MHz !
      RCC_CFGR !
      96000000 dup clock-hz !
      4 / uart2-baud @ / \ calculate divider ($d0 for 115200) 
      USART2_BRR ! \ set baud rate divider
;

: 16MHz
    CR ." currently not implemented, sorry."
;

