\ Example IR receiver
\ with TSOP1838 on PA1 with nec protocol

\ be aware I just packed the bits in IRCODE how I like them

\ needs io.fs from jcw
\ made for STM32F411C8 'black pill',
\ should work on all stm32f4 and others 

\ data from RM0383 for STM32F1411
\ but should work on any STM32F4

\ all constants already defined in stm32f4.fs
\ $40023800 constant RCC
\ RCC $44 + constant RCC_APB2ENR

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

\ documented in RM0383 for STM32F10xxx
\ table 37 Vector table
\ from position 32 it starts with NVIC_ISER1
\ $E000E100 constant NVIC_ISER0 
\ NVIC_ISER0 $4 + constant NVIC_ISER1 
\ 6 constant EXTI0_irq 
7 constant EXTI1_irq \ for PA1
\ 8 constant EXTI2_irq

0 variable IRTime
0 variable IRBIT
0 variable IRCODE

: IR.
    CR IRCODE @ hex. \ just print out the code
;

\ irq service routine for IR input
\ very simple not much error prone, but works fro me :)
: ir_isr
    1 bit EXTI_PR bit@ not if exit then  \ exit wenn nicht exti1
    micros IRTime @ over IRTIME ! - \ get diff to last irq
    case
        dup 13500 - abs 2000 < ?of
            0 IRBIT !
            0 IRCODE !
        endof
        dup 1200 - abs 200 < ?of
            1 IRBIT +! \ its L, next one
        endof
        dup 2300 - abs 200 < ?of
            IRBIT @ 16 + 32 mod bit
            IRCODE bis! \ set it high
            1 IRBIT +!
        endof
        \ ." E" dup .
    endcase
    IRBIT @ 32 = if \ yeah all bits arrived
        IR.
    then
    1 bit EXTI_PR bis! \ clear exti0
;

\ setup code for only PA2
: irsetup
    imode-float PA1 io-mode! \ for IR TSOP1838 sensor
    ['] ir_isr irq-exti1 ! \ set isr for exti2
    
    \ enable syscfg
    14 bit RCC_APB2ENR bis! \ set SYSCFGEN
    $00F0 SYSCFG_EXTICR1 bic!   \ PA1 for exti1

    1 bit EXTI_FTSR bis! \ falling edge for exti1
    1 bit EXTI_IMR bis!  \ enable exti1
    
    \ EXTI1_irq bit nvic_iser0 bis! \ enable exti1 in nvic
    EXTI1_irq nvic-enable
    CR ." IR Ready!"
;
    
