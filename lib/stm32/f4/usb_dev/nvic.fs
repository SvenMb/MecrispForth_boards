
\ ----------------------------
\  Interrupt controller tools
\ ----------------------------

\ these registers are probably not defined in stm32f411
$E000E000 constant NVIC
NVIC $100 + constant NVIC_ISER0 
NVIC $104 + constant NVIC_ISER1 
NVIC $108 + constant NVIC_ISER2 
NVIC $10C + constant NVIC_ISER3 

NVIC $180 + constant NVIC_ICER0 
NVIC $184 + constant NVIC_ICER1 
NVIC $188 + constant NVIC_ICER2 
NVIC $18C + constant NVIC_ICER3 

NVIC $400 + constant NVIC_IPR0 

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
