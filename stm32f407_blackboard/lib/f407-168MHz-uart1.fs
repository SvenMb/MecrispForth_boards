\ STM32F407 black board 
\ 168MHz fcpu settings
\ standard clock settings
\ uart1 115k2
\ by Igor de om1zz, 2015 for discovery board
\ very small changes by Sven Muehlberg 2021 for black board
\ no warranties of any kind

\ $40023800 constant RCC
 
\ $40023C00 constant Flash_ACR \ Flash Access Control Register
\ $40004408 constant USART2_BRR
\ $40011008 constant USART1_BRR

\ f (VCO clock) = f (PLL clock input) * (PLLN/PLLM)
\ f (PLL general clock output) = F (VCO clock) / PLLP
\ f (USB, RNG und andere) = f (VCO clock) / PLLQ

\ RCC_Base $00 + constant RCC_CR
  1 24 lshift constant PLLON
  1 25 lshift constant PLLRDY

\ RCC_Base $04 + constant RCC_PLLCRGR
   1 22 lshift constant PLLSRC

\ RCC_Base $08 + constant RCC_CFGR


\ was missing in original
[ifndef] clock-hz
8000000 variable clock-hz \ systemclock is 8MHz after reset
[then]

115200 variable uart1-baud


: 168MHz ( -- )

  \ Set Flash waitstates !
  $103 Flash_ACR !   \ 3 Waitstates for 120 MHz with more than 2.7 V Vcc, Prefetch buffer enabled.

  PLLSRC          \ HSE clock as 8 MHz source

  8  0 lshift or  \ PLLM Division factor for main PLL and audio PLL input clock 
                  \ 8 MHz / 8 =  1 MHz. Divider before VCO. Frequency entering VCO to be between 1 and 2 MHz.

336  6 lshift or  \ PLLN Main PLL multiplication factor for VCO - between 192 and 432 MHz
                  \ 1 MHz * 336 = 336 MHz

  7 24 lshift or  \ PLLQ = 7, 336 MHz / 7 = 48 MHz

  0 16 lshift or  \ PLLP Division factor for main system clock
                  \ 0: /2  1: /4  2: /6  3: /8
                  \ 336 MHz / 2 = 168 MHz 
    RCC_PLLCFGR !

    PLLON RCC_CR bis!
    \ Wait for PLL to lock:
    begin PLLRDY RCC_CR bit@ until

    2                 \ Set PLL as clock source
    %101 10 lshift or \ APB  Low speed prescaler (APB1) - Max 42 MHz ! Here 168/4 MHz = 42 MHz.
    %100 13 lshift or \ APB High speed prescaler (APB2) - Max 84 MHz ! Here 168/2 MHz = 84 MHz.
    RCC_CFGR !
    168000000 dup clock-hz !
    2 / uart1-baud @ / \ calculate divider ($2da for 115200) 
    USART1_BRR ! \ Set Baud rate divider

    \ also set for UART2
    $16d USART2_BRR ! \ Set Baud rate divider for 115200 Baud at 42 MHz. 22.786

;

: 8MHz
    CR ." currently not implemented, sorry."
;
