#!/usr/bin/perl

my $line = 0;

print "create icon
binary

\$02 h,        \\ icon
#64 #64 * h,  \\ 64x64 pixels
#64 h,        \\ 64pix width
" ;

while(<>) {
	s/^\s+//; 
	next if not /^[01],/;
	$line++;
	chomp if ($line % 4 > 0) ; 
	# 	/^\d+,\d+,\d+,\d+,\d+,\d+,\d+,\d+,\d+/ ;
	s/^(\d+),(\d+),(\d+),(\d+),(\d+),(\d+),(\d+),/$1$2$3$4$5$6$7/ ;
	s/^(\d+),(\d+),(\d+),(\d+),(\d+),(\d+),(\d+),(\d+),/$1,$2$3$4$5$6$7$8/ ;
	s/,/ c, /g ;
	s/([01])$/$1 c, / ;
	print;
}

print "
decimal

: icon ( uc -- c-addr width pixels )
    drop
    icon 6 +  \\ get address
    dup 2- h@ \\ get width
    over 4 - h@ \\ get pix count
;
"
