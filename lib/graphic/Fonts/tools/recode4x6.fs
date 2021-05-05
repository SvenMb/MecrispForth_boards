\ these tools where used to reencode some fonts

: recode4x4 ( font-addr )
    CR ." binary"
    CR ." create font4x4"
    binary
    begin
        CR
        dup [char] $ emit h@ h.4 ."  h, "
        dup 2+ c@ u.8
        dup #3 + c@ u.8 ."  h,"
    dup h@ while
        #4 +
    repeat
    decimal
    drop
    CR ." decimal"
    CR
;

\ changes byteorder for 4x6 font provided with mecrisp
: recode4x6 ( font-addr )

    begin
        CR 
        dup h@ . ." h,  $" \ char #
        dup 2 + c@ h.2
        dup 3 + c@ h.2
        dup 4 + c@ h.2
        dup 5 + c@ h.2
        ."  ,"
    dup h@ while
        6 +
    repeat
    drop
;


\ changes bit direction for 5x7 font
0 variable bitcounter

: bitout
    bitcounter @
    8 mod 0= if
        [char] % emit
    then
    
    if
        [char] 1 emit
    else
        [char] 0  emit
    then
    bitcounter @
    8 mod 7 = if
        ."  c, "
    then
    
    1 bitcounter +!
;

    

: recode5x7
    96 0 do
        \ CR ." \ " i .
        CR
        0 bitcounter !
        8 0 do
            5 0 do
                j bit
                over i + cbit@
                bitout
            loop
            false bitout
        loop
        5 +
    loop
    drop
;
