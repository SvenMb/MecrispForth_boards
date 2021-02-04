\ examples
\ formates time to string hh:mm:ss
: ftime ( hh mm ss - - c-addr length )
    swap rot
    100 * + 100 * +
    0 <# # # $3a hold # # $3a hold # # #>
;

\ prints time
: time.
    rtc-get-time
    ftime
    type
;

\ formates date to dd.mm.yyyy
: fdate ( yy wd mm dd - - c-addr length )
    100 * +
    swap drop
    10000 * +
    2000 + \ waiting for year 2100 problems
    0 <# # # # # $2e hold # # $2e hold # # #>
;

\ prints date in dd.mm.yyyy
: date. ( - - )
    rtc-get-date
    fdate
    type
;
