\ ---
\ subject: unicode fixed font 4x4 size
\ origin: mecrisp2.5.8 distribution (common/graphics/graphics-unicode-3x3.txt)
\ author: Sven Muehlberg
\ copyright: see below and see mecrisp from Mathias Koch
\ ---

( fixed4x4 start: ) here dup hex.

\ -------------------------------------------------------------
\  Artwork for 3x3 Bitmap font, taken from "Fakoo"
\ -------------------------------------------------------------
\ SMb: which is really 4x4 because you need some space between characters...
\ changed to that...

decimal
create font4x4

\ Unicode point, Bitmap Data

32 h,  %0000.0000.0000 drop h,   \
33 h,  %1110.0000.0100 drop h,   \ !
34 h,  %1010.1010.0000 drop h,   \ "
35 h,  %1010.0000.1010 drop h,   \ #
                      
36 h,  %0110.0110.1110 drop h,   \ $
37 h,  %0100.0000.0010 drop h,   \ %
38 h,  %0110.1100.1110 drop h,   \ &
39 h,  %0100.0100.0000 drop h,   \ '
40 h,  %0010.0100.0100 drop h,   \ (
41 h,  %1000.0100.0100 drop h,   \ )
42 h,  %0100.0100.1010 drop h,   \ *
43 h,  %0100.1110.0100 drop h,   \ +
44 h,  %0000.0100.1000 drop h,   \ ,
45 h,  %0000.1110.0000 drop h,   \ -
46 h,  %0000.0000.0100 drop h,   \ .
47 h,  %0010.0100.1000 drop h,   \ /
                      
48 h,  %1110.1010.1110 drop h,   \ 0
49 h,  %0010.0110.0010 drop h,   \ 1
50 h,  %1100.0100.1010 drop h,   \ 2
51 h,  %1100.0110.1100 drop h,   \ 3
52 h,  %1000.1110.0100 drop h,   \ 4
53 h,  %1110.1100.0100 drop h,   \ 5
54 h,  %1000.1110.1110 drop h,   \ 6
55 h,  %1110.0010.0100 drop h,   \ 7
56 h,  %1100.1110.0110 drop h,   \ 8
57 h,  %1110.1110.0010 drop h,   \ 9
                      
58 h,  %0100.0000.0100 drop h,   \ :
59 h,  %0100.0100.1000 drop h,   \ ;
60 h,  %0010.0100.0010 drop h,   \ <
61 h,  %1110.0000.1110 drop h,   \ =
62 h,  %1000.0100.1000 drop h,   \ >
63 h,  %1110.0110.0100 drop h,   \ ?
64 h,  %1100.0010.1010 drop h,   \ @
                      
65 h,  %0100.1110.1010 drop h,   \ A
66 h,  %1110.0110.1110 drop h,   \ B
67 h,  %0110.1000.0110 drop h,   \ C
68 h,  %1100.1010.1100 drop h,   \ D
69 h,  %1110.1100.1110 drop h,   \ E
70 h,  %1110.1100.1000 drop h,   \ F
71 h,  %0110.1010.0110 drop h,   \ G
72 h,  %1010.1110.1010 drop h,   \ H
73 h,  %0100.0100.1110 drop h,   \ I
74 h,  %0010.0010.1100 drop h,   \ J
75 h,  %1010.1100.1010 drop h,   \ K
76 h,  %1000.1000.1110 drop h,   \ L
77 h,  %1110.1110.1010 drop h,   \ M
78 h,  %1110.1010.1010 drop h,   \ N
79 h,  %0100.1010.0100 drop h,   \ O
80 h,  %1100.1110.1000 drop h,   \ P
81 h,  %0100.1010.0110 drop h,   \ Q
82 h,  %1110.1100.1010 drop h,   \ R
83 h,  %0110.0100.1100 drop h,   \ S
84 h,  %1110.0100.0100 drop h,   \ T
85 h,  %1010.1010.1110 drop h,   \ U
86 h,  %1010.1010.0100 drop h,   \ V
87 h,  %1010.1110.1110 drop h,   \ W
88 h,  %1010.0100.1010 drop h,   \ X
89 h,  %1010.0100.0100 drop h,   \ Y
90 h,  %1110.0100.1110 drop h,   \ Z

91 h,  %1100.1000.1100 drop h,   \ [
92 h,  %1000.0100.0010 drop h,   \ \
93 h,  %0110.0010.0110 drop h,   \ ]
94 h,  %0100.1010.0000 drop h,   \ ^
95 h,  %0000.0000.1110 drop h,   \ _
96 h,  %1000.0100.0000 drop h,   \ `
                      
97 h,  %0100.1110.1010 drop h,   \ a -> A
98 h,  %1110.0110.1110 drop h,   \ b -> B
99 h,  %0110.1000.0110 drop h,   \ c -> C
100 h, %1100.1010.1100 drop h,   \ d -> D
101 h, %1110.1100.1110 drop h,   \ e -> E
102 h, %1110.1100.1000 drop h,   \ f -> F
103 h, %0110.1010.0110 drop h,   \ g -> G
104 h, %1010.1110.1010 drop h,   \ h -> H
105 h, %0100.0100.1110 drop h,   \ i -> I
106 h, %0010.0010.1100 drop h,   \ j -> J
107 h, %1010.1100.1010 drop h,   \ k -> K
108 h, %1000.1000.1110 drop h,   \ l -> L
109 h, %1110.1110.1010 drop h,   \ m -> M
110 h, %1110.1010.1010 drop h,   \ n -> N
111 h, %0100.1010.0100 drop h,   \ o -> O
112 h, %1100.1110.1000 drop h,   \ p -> P
113 h, %0100.1010.0110 drop h,   \ q -> Q
114 h, %1110.1100.1010 drop h,   \ r -> R
115 h, %0110.0100.1100 drop h,   \ s -> S
116 h, %1110.0100.0100 drop h,   \ t -> T
117 h, %1010.1010.1110 drop h,   \ u -> U
118 h, %1010.1010.0100 drop h,   \ v -> V
119 h, %1010.1110.1110 drop h,   \ w -> W
120 h, %1010.0100.1010 drop h,   \ x -> X
121 h, %1010.0100.0100 drop h,   \ y -> Y
122 h, %1110.0100.1110 drop h,   \ z -> Z
                      
123 h, %0110.1100.1000 drop h,   \ {
124 h, %0100.0100.0100 drop h,   \ |
125 h, %1100.0110.0010 drop h,   \ }
126 h, %0010.1110.1000 drop h,   \ ~
                      
                      
196 h, %0110.1110.1010 drop h,   \ Ä
214 h, %0110.1010.0100 drop h,   \ Ö
220 h, %1010.1000.1110 drop h,   \ Ü
223 h, %0110.0110.1100 drop h,   \ ß
                      
  0 h, %1110.1110.1110 drop h,   \ End of font marker, replacement glyph.

align decimal

\ public mapper for font-hook, see charwriter.fs

: fixed4x4 ( uc -- c-addr width pixels ) \ Translates Unicode character to address of 8x8 bitmap.

    font4x4 ( x addr )
    begin
        2dup h@
        dup -rot  <>  and \ true if character found, also if cur char# is $0
    while \ or no more characters left in the glyph collection...
            4 + \ Not this one, skip it and its bitmap
    repeat

    nip 2 + \ Character not found within available collection of glyphs. Display replacement.
    
    4 12 \ 4 pix width, 12 pixels at all (free line below assumed)
    \ 4 16 \ change to this if you need to write empty pixels
  
    1-foldable
;

\ how to use with charwriter.fs
\ ['] fixed4x4 font-hook !
\ : helloworld s" Hello World!" 0 8 drawstring s" (äöüß$€¤£ °¶)" 0 8 drawstring ;

( fixed4x4 end: ) here dup hex.
( fixed4x4 size: ) swap - hex.


