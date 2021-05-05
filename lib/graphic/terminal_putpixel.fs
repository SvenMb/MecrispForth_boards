\ -------------------------------------------------------------
\  Interface to real graphics hardware necessary.
\  This is just for ASCII art in terminal !
\ -------------------------------------------------------------

  : u.base10 ( u -- ) base @ decimal swap 0 <# #s #> type base ! ;
  : ESC[ ( -- ) 27 emit 91 emit ;
  : at-xy ( column row -- ) 1+ swap 1+ swap ESC[ u.base10 ." ;" u.base10 ." H" ;

: clear ESC[ ." 2J" 0 0 at-xy ;

: putpixel ( x y -- )  at-xy [char] * emit ;

\ -------------------------------------------------------------
\  Bresenham line
\ -------------------------------------------------------------
