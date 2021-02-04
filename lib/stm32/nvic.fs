
\ ----------------------------
\  Interrupt controller tools
\ ----------------------------

\ these registers are probably not defined in stm32f411
[ifndef] NVIC_ISER3
  NVIC $10C + constant NVIC_ISER3 
  NVIC $18C + constant NVIC_ICER3 
[then]


: nvic-enable ( irq# -- )
  \ 16 - \ Cortex Core Vectors
  dup 32 u< if      1 swap lshift NVIC_ISER0 ! exit then
  dup 64 u< if 32 - 1 swap lshift NVIC_ISER1 ! exit then
  dup 96 u< if 64 - 1 swap lshift NVIC_ISER2 ! exit then
               96 - 1 swap lshift NVIC_ISER3 !
;

: nvic-disable ( irq# -- )
  \ 16 - \ Cortex Core Vectors
  dup 32 u< if      1 swap lshift NVIC_ICER0 ! exit then
  dup 64 u< if 32 - 1 swap lshift NVIC_ICER1 ! exit then
  dup 96 u< if 64 - 1 swap lshift NVIC_ICER2 ! exit then
               96 - 1 swap lshift NVIC_ICER3 !
;
 
: nvic-priority ( priority irq# -- )
  NVIC_IPR0 + c!
;
