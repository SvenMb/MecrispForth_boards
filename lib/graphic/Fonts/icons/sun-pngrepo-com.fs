create icon
binary

$02 h,       \ icon
#40 #40 * h,  \ 40x40 pixels
#40 h,        \ 40pix width
00000000 c, 00000000 c, 00000000 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00011100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00011000 c, 00000000 c, 00000000 c, 
00000011 c, 10000000 c, 00000000 c, 00000001 c, 11000000 c, 
00000011 c, 11000000 c, 00000000 c, 00000011 c, 11000000 c, 
00000011 c, 11100000 c, 01111111 c, 00000111 c, 11000000 c, 
00000001 c, 11100011 c, 11111111 c, 11000111 c, 10000000 c, 
00000000 c, 11000111 c, 11111111 c, 11100011 c, 00000000 c, 
00000000 c, 00001111 c, 11111111 c, 11111000 c, 00000000 c, 
00000000 c, 00011111 c, 10000000 c, 11111000 c, 00000000 c, 
00000000 c, 00111110 c, 00000000 c, 01111100 c, 00000000 c, 
00000000 c, 01111100 c, 00000000 c, 00111110 c, 00000000 c, 
00000000 c, 01111000 c, 00000000 c, 00011110 c, 00000000 c, 
00000000 c, 11110000 c, 00000000 c, 00001111 c, 00000000 c, 
00000000 c, 11110000 c, 00000000 c, 00001111 c, 00000000 c, 
00111000 c, 11110000 c, 00000000 c, 00001111 c, 00011110 c, 
01111100 c, 11110000 c, 00000000 c, 00001111 c, 00111110 c, 
01111100 c, 11110000 c, 00000000 c, 00001111 c, 00111110 c, 
01111000 c, 11110000 c, 00000000 c, 00001111 c, 00011110 c, 
00000000 c, 11110000 c, 00000000 c, 00001111 c, 00000000 c, 
00000000 c, 11110000 c, 00000000 c, 00001111 c, 00000000 c, 
00000000 c, 01111000 c, 00000000 c, 00011110 c, 00000000 c, 
00000000 c, 01111100 c, 00000000 c, 00011110 c, 00000000 c, 
00000000 c, 00111100 c, 00000000 c, 00111100 c, 00000000 c, 
00000000 c, 00111111 c, 00000000 c, 11111100 c, 00000000 c, 
00000000 c, 00011111 c, 11100111 c, 11111000 c, 00000000 c, 
00000000 c, 11001111 c, 11111111 c, 11110011 c, 00000000 c, 
00000001 c, 11100011 c, 11111111 c, 11000111 c, 10000000 c, 
00000011 c, 11100000 c, 11111111 c, 00000111 c, 11000000 c, 
00000011 c, 11000000 c, 00000000 c, 00000011 c, 11000000 c, 
00000011 c, 10000000 c, 00000000 c, 00000001 c, 11000000 c, 
00000000 c, 00000000 c, 00011000 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00111100 c, 00000000 c, 00000000 c, 
00000000 c, 00000000 c, 00000000 c, 00000000 c, 00000000 c, 

decimal

: icon ( uc -- c-addr width pixels )
    drop
    icon 6 +  \ get address
    dup 2- h@ \ get width
    over 4 - h@ \ get pix count
;
