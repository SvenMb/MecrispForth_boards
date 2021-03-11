

$E000E010 constant STK_CTRL
$E000E014 constant STK_LOAD
$E000E018 constant STK_VAL
$E000E01c constant STK_CALIB

0 variable ticks
: ++ticks ( -- )
1 ticks +!
;

: systick-hz ( u -- ) \ enable systick at given frequency 
    clock-hz @ 8 / \ take care of 1/8 uP clock divider
    swap / 1-
    STK_LOAD ! \ counter for 1/8 uP clock
    ['] ++ticks irq-systick !
    -1 nvic-enable 
    3 STK_CTRL ! \ enable with 1/8 uP clock
;

: systick-hz? ( -- u ) \ derive current systick frequency from clock
    clock-hz @
    STK_LOAD @ 1+  /
;

: micros ( -- n )  \ return elapsed microseconds, this wraps after some 2000s
\ assumes systick is running at 1000 Hz, overhead is about 1.8 us @ 72 MHz
\ get current ticks and systick, spinloops if ticks changed while we looked
    begin ticks @ STK_VAL @ over ticks @ <> while 2drop repeat
    STK_LOAD @ 1+ swap -  \ convert down-counter to remaining
    clock-hz @  8000000 / ( ticks systicks mhz )
    / swap 1000 * +
;

: millis
    ticks @
;

: us ( n -- )  \ microsecond delay using a busy loop, this won't switch tasks
    2 -  \ adjust for approximate overhead of this code itself
    micros +  begin dup micros - 0< until  drop
;

: ms ( n -- )  \ millisecond delay, multi-tasker aware (may switch tasks!)
    millis +  begin millis over - 0< while pause repeat  drop
;
   
