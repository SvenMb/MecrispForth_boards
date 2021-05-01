\ STM32F407 black board 
\ 168MHz fcpu settings
\ standard clock settings
\ uart1 115k2
\ by Igor de om1zz, 2015 for discovery board
\ very small changes by Sven Muehlberg 2021 for black board
\ no warranties of any kind

$40023800 constant RCC_BASE
 
$40023C00 constant Flash_ACR \ Flash Access Control Register
$40004408 constant USART2_BRR
$40011008 constant USART1_BRR

\ f (VCO clock) = f (PLL clock input) * (PLLN/PLLM)
\ f (PLL general clock output) = F (VCO clock) / PLLP
\ f (USB, RNG und andere) = f (VCO clock) / PLLQ

RCC_Base $00 + constant RCC_CR

RCC_Base $04 + constant RCC_PLLCFGR

RCC_Base $08 + constant RCC_CFGR


\ current clocks and baud rates
8000000 variable clock-hz \ systemclock is 8MHz after reset
0 variable APB1-hz
0 variable APB2-hz

115200 variable uart1-baud
115200 variable uart2-baud


: 168MHz ( -- )

    \ Set Flash waitstates !
    $705 Flash_ACR !   \ 5 Waitstates, all buffer enabled.

    1 16 lshift RCC_CR bis! \ HSEON
    begin 1 17 lshift RCC_CR bit@ until \ wait for HSERDY

    1                 \ Set HSE as clock source
    %101 10 lshift or \ APB  Low speed prescaler (APB1) - Max 42 MHz ! Here 168/4 MHz = 42 MHz.
    %100 13 lshift or \ APB High speed prescaler (APB2) - Max 84 MHz ! Here 168/2 MHz = 84 MHz.
    RCC_CFGR !

    1 22 lshift          \ HSE clock as PLLSRC
    8  0 lshift or  \ PLLM Division factor for main PLL and audio PLL input clock 
                    \ 8 MHz / 8 =  1 MHz. Divider before VCO. Frequency entering VCO to be between 1 and 2 MHz.

    336 6 lshift or  \ PLLN Main PLL multiplication factor for VCO - between 192 and 432 MHz
                      \ 1 MHz * 336 = 336 MHz

    7 24 lshift or  \ PLLQ = 7, 336 MHz / 7 = 48 MHz for USB

    0 16 lshift or  \ PLLP Division factor for main system clock
                    \ 0: /2  1: /4  2: /6  3: /8
                    \ 336 MHz / 2 = 168 MHz 
    RCC_PLLCFGR !

    1 24 lshift RCC_CR bis! \ PLLON 
    \ Wait for PLL to lock
    begin 1 25 lshift RCC_CR bit@ until \ wait PLLRDY

    2                 \ Set PLL as clock source
    %101 10 lshift or \ APB  Low speed prescaler (APB1) - Max 42 MHz ! Here 168/4 MHz = 42 MHz.
    %100 13 lshift or \ APB High speed prescaler (APB2) - Max 84 MHz ! Here 168/2 MHz = 84 MHz.
    RCC_CFGR !

    42000000 APB1-hz !
    84000000 APB2-hz !
    168000000 clock-hz !

    \ UART1 on fast APB2
\    APB2-hz @ uart1-baud @ / \ calculate divider
\    USART1_BRR ! \ Set Baud rate divider
    \ UART2 on slow APB1
\    APB1-hz @ uart2-baud @ /
\    USART2_BRR
$16d USART2_BRR !
;
