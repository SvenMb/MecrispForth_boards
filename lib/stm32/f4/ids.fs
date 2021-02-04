\ definitions for stm32f4
\ from jcw flib/stm32f4/hal.fs

: chipid ( -- u1 u2 u3 3 )  \ unique chip ID as N values on the stack
  $1FFF7A10 @ $1FFF7A14 @ $1FFF7A18 @ 3 ;
: hwid ( -- u )  \ a "fairly unique" hardware ID as single 32-bit int
  chipid 1 do xor loop ;
: flash-kb ( -- u )  \ return size of flash memory in KB
  $1FFF7A22 h@ ;

