\ Example using simple interupt driven buttons or touch sensors
\ not debounced...
\ needs io.fs from jcw
\ made for STM32F4

\ irq5-exti5 is not defined in standard mecrisp kernel
\ use mine in stm32f411-ra subfolder, or assemble it yourself with that irq

\ data from RM0383 for STM32F1411
\ but should work on any STM32F4

\ all constants already defined in stm32f4.fs 
\ $40013800 constant SYSCFG

\ 4 bit defining the port PA -> %0000 PB-> %0001 ...
\ starting with exti0
\ syscfg $08 + constant SYSCFG_EXTICR1
\ starting with exti4
\ SYSCFG $0C + constant SYSCFG_EXTICR2
\ starting with exti8
\ SYSCFG $10 + constant SYSCFG_EXTICR3
\ starting with exti12
\ SYSCFG $14 + constant SYSCFG_EXTICR4

\ $40013C00 constant EXTI
\ bits for enabling exti
\ EXTI $0 + constant EXTI_IMR
\ bits for raising edge irq
\ EXTI $8 + constant EXTI_RTSR
\ bits for falling edge irq
\ EXTI $C + constant EXTI_FTSR
\ irq status, also for irq reset
\ EXTI $14 + constant EXTI_PR

\ documented in RM0383 for STM32F411
\ table 37 Vector table
\ from position 32 it starts with NVIC_ISER1
\ $E000E100 constant NVIC_ISER0 
\ NVIC_ISER0 $4 + constant NVIC_ISER1 
6 constant EXTI0_irq 
23 constant EXTI5_irq



\ irq service routine
: gpio_isr
    EXTI_PR @ case \ irq status case
        dup 0 bit and ?of \ check for exti0
            CR ." PA0"
            0 bit EXTI_PR bis! \ clear exti0
        endof
        dup 5 bit and ?of \ check for exti5
            CR ." PB5"
            5 bit EXTI_PR bis! \ clear exti5
        endof
        dup 8 bit and ?of \ check for exti8
            CR ." PB8"
            8 bit EXTI_PR bis! \ clear exti8
        endof
    endcase
;

\ setup code for only PA0
: setup_PA0
\    imode-float PA0 io-mode! \ for touch sensor
    imode-low PA0 io-mode! \ most other things
    

    ['] gpio_isr irq-exti0 ! \ set isr for exti0
    
    $000F SYSCFG_EXTICR1 bic!   \ PA0 for exti0
    0 bit EXTI_RTSR bis! \ rising edge for exti0
\    0 bit EXTI_FTSR bis! \ falling edge for exti0 if needed
    0 bit EXTI_IMR bis!  \ enable exti0
    
    \ EXTI0_irq bit nvic_iser0 bis! \ enable exti0 in nvic
    EXTI0_irq nvic-enable
    CR ." Ready!"
;
    
\ setup code for multiline PB5 and PB8
: setup_PB5_PB8
\    imode-float PB5 io-mode!
\    imode-float PB8 io-mode!
    imode-low PA5 io-mode! \ just to have PA pins clean
    imode-low PA8 io-mode!
    imode-low PB5 io-mode! \ these are the one which should work, but
    imode-low PB8 io-mode!
    
    ['] gpio_isr irq-exti5 ! \ set isr for exti5-9 (!)

    \ why is this ignored???
    $00E0 SYSCFG_EXTICR2 bic! $0010 SYSCFG_EXTICR2 bis!   \ PB5 for exti5
    $000E SYSCFG_EXTICR3 bic! $0001 SYSCFG_EXTICR3 bis!   \ PB8 for exti8
    \ after that syscfg_exticrX is still empty, why???
    
    5 bit 8 bit or EXTI_RTSR bis! \ rising edge for exti5 and exti8
    5 bit 8 bit or EXTI_IMR bis!  \ enable exti5 and exti8

    \ EXTI5_irq bit nvic_iser0 bis! \ enable exti5-9(!) in nvic
    \ EXTI5_irq nvic-enable
    CR ." Ready!"
;
