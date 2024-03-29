\ include unicode-4x4.fs 
\ include unicode-4x6.fs
\ include unicode-8x8.fs 
\ include unicode-8x16.fs 
\ include ascii-6x8.fs 
\ include ascii-c64.fs 

\ include ../charwriter.fs

\ DISPLAY-init
\ clear display

\ : hw s" Hi World! üßñ€£¶" ;

\ ['] fixed4x4 font-hook !
\ hw 0 0 drawstring display

\ ['] fixed4x6 font-hook !
\ hw 0 6 drawstring display

\ ['] ascii6x8 font-hook !
\ hw 0 14 drawstring display

\ ['] fixed8x8 font-hook !
\ hw 0 24 drawstring display

\ ['] c64_8x8 font-hook !
\ hw 0 34 drawstring display

\ ['] fixed8x16 font-hook !
\ hw 0 44 drawstring display


\ font demo for 320 x 240 tft display

: test4x4
    \ DISPLAY-init
    clear
    DISPLAY-on
    ['] fixed4x4 font-hook !
    0
    32 begin
        over    \ get next pos
        80 /mod \ char per line
        6 *     \ calc y
        swap 4 * swap \ calc x
        \ stack: pos char x y
        2 pick  \ get char
        0 hex <# # # # # [char] : hold< #> decimal
        \ Stack: pos char x y char c-addr u
        2over drawstring
        swap 24 + swap \ forward 6 chars
        2 pick drawchar 2drop
        \ stack: pos char
        dup
    while
            fixed4x4 2drop 2 + h@ \ next char
            swap 8 + swap          \ next pos
    repeat
    2drop
    [ifdef] rgb2tft
        $FF $20 $20 rgb2tft DISPLAY-fg !
    [then]
    s" <<< That's it. >>>" 0 232 drawstring
    [ifdef] rgb2tft
        $FF $FF $FF rgb2tft DISPLAY-fg !
    [then]
;

: test4x6
    \ DISPLAY-init
    clear
    DISPLAY-on
    ['] fixed4x6 font-hook !
    0
    32 begin
        over    \ get next pos
        80 /mod \ char per line
        8 *     \ calc y
        swap 4 * swap \ calc x
        \ stack: pos char x y
        2 pick  \ get char
        0 hex <# # # # # [char] : hold< #> decimal
        \ Stack: pos char x y char c-addr u
        2over drawstring
        swap 24 + swap \ forward 6 chars
        2 pick drawchar 2drop
        \ stack: pos char
        dup
    while
            fixed4x6 2drop 4 + h@ \ next char
            swap 8 + swap         \ next pos
    repeat
    2drop \ drop pos, char
    [ifdef] rgb2tft
        $FF $20 $20 rgb2tft DISPLAY-fg !
    [then]
    s" <<< That's it. >>>" 0 232 drawstring
    [ifdef] rgb2tft
        $FF $FF $FF rgb2tft DISPLAY-fg !
    [then]
;

: test8x8
    \ DISPLAY-init
    \ touch-init
    clear
    DISPLAY-on
    ['] fixed8x8 font-hook !
    0
    32 begin
        over    \ get next pos
        1159 > if         \ check if last pos
            [ifdef] rgb2tft
                $FF $20 $20 rgb2tft DISPLAY-fg !
            [then]
             s" <<<touch or key for next>>>" 0 232 drawstring
            [ifdef] rgb2tft
                $FF $FF $FF rgb2tft DISPLAY-fg !
            [then]
            nip 0 swap \ reset pos
            wait_touch 2drop
            clear
        then
        over
        40 /mod \ char per line
        8 *     \ calc y
        swap 8 * swap \ calc x
        \ stack: pos char x y
        2 pick  \ get char
        0 hex <# # # # # [char] : hold< #> decimal
        \ Stack: pos char x y char c-addr u
        2over drawstring
        swap 48 + swap \ forward 6 chars
        2 pick drawchar 2drop
        \ stack: pos char
        dup
    while
            fixed8x8 2drop 8 + h@ \ next char
            swap 8 + swap         \ next pos
    repeat
    2drop
    [ifdef] rgb2tft
        $FF $20 $20 rgb2tft DISPLAY-fg !
    [then]
    s" <<< That's it. >>>" 0 232 drawstring
    [ifdef] rgb2tft
        $FF $FF $FF rgb2tft DISPLAY-fg !
    [then]
;

: test8x16
    \ DISPLAY-init
    \ touch-init
    clear
    DISPLAY-on
    ['] fixed8x16 font-hook !
    0
    32 begin
        over    \ get next pos
        559 > if         \ check if last pos
            [ifdef] rgb2tft
                $FF $20 $20 rgb2tft DISPLAY-fg !
            [then]
            s" <<<touch or key for next>>>" 0 224 drawstring
            [ifdef] rgb2tft
                $FF $FF $FF rgb2tft DISPLAY-fg !
            [then]

            nip 0 swap \ reset pos
            wait_touch 2drop
            clear
        then
        over
        40 /mod \ char per line
        16 *     \ calc y
        swap 8 * swap \ calc x
        \ stack: pos char x y
        2 pick  \ get char
        0 hex <# # # # # [char] : hold< #> decimal
        \ Stack: pos char x y char c-addr u
        2over drawstring
        swap 48 + swap \ forward 6 chars
        2 pick drawchar 2drop
        \ stack: pos char
        dup
    while
            fixed8x16 2drop 16 + h@ \ next char
            swap 8 + swap         \ next pos
    repeat
    2drop
    [ifdef] rgb2tft
        $FF $20 $20 rgb2tft DISPLAY-fg !
    [then]
    s" <<< That's it. >>>" 0 224 drawstring
    [ifdef] rgb2tft
        $FF $FF $FF rgb2tft DISPLAY-fg !
    [then]
;
