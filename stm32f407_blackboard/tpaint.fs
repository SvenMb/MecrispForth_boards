

create panel
$ff $ff $ff rgb2tft h, \
$c0 $c0 $c0 rgb2tft h, \
$80 $80 $80 rgb2tft h, \
$40 $40 $40 rgb2tft h, \
$00 $00 $00 rgb2tft h, \ 
$ff $00 $00 rgb2tft h, \
$ff $ff $00 rgb2tft h, \
$00 $ff $00 rgb2tft h, \ 
$00 $ff $ff rgb2tft h, \
$00 $00 $ff rgb2tft h, \
$ff $00 $ff rgb2tft h, \

: menu
    swap drop \ throw x away
    dup 220 <
    if
        20 / 2* panel + h@ tft-fg h!
    else
        tft-fg h@ 0 0 299 239 rect 
    then
;


: tpaint
    $ffff tft-fg h!
    $0000 tft-bg h!
    clear
    tft-on

    $80 $80 $80 rgb2tft 300 0 300 239 rect

    12 0 do             \ 11 colors
        panel i 2* + h@ \ get color
        301 i 20 * dup 19 + 319 swap rect
    loop
    s" clr" 302 227 drawstring

    ." Press Key for quit"
    
    begin
        wait_touch trxy
        if        
            over
            300 <
            if putpixel
            else menu
            then
        else 2drop
        then
        key?
    until
;