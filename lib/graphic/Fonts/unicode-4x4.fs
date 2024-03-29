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


binary
create font4x4
$0020 h, 0000000000000000 h,   \  
$0021 h, 0000010000001110 h,   \ !
$0022 h, 1010000000001010 h,   \ "
$0023 h, 0000101000001010 h,   \ #
                                  
$0024 h, 0110111000000110 h,   \ $
$0025 h, 0000001000000100 h,   \ %
$0026 h, 1100111000000110 h,   \ &
$0027 h, 0100000000000100 h,   \ '
$0028 h, 0100010000000010 h,   \ (
$0029 h, 0100010000001000 h,   \ )
$002A h, 0100101000000100 h,   \ *
$002B h, 1110010000000100 h,   \ +
$002C h, 0100100000000000 h,   \ ,
$002D h, 1110000000000000 h,   \ -
$002E h, 0000010000000000 h,   \ .
$002F h, 0100100000000010 h,   \ /

$0030 h, 1010111000001110 h,   \ 0
$0031 h, 0110001000000010 h,   \ 1
$0032 h, 0100101000001100 h,   \ 2
$0033 h, 0110110000001100 h,   \ 3
$0034 h, 1110010000001000 h,   \ 4
$0035 h, 1100010000001110 h,   \ 5
$0036 h, 1110111000001000 h,   \ 6
$0037 h, 0010010000001110 h,   \ 7
$0038 h, 1110011000001100 h,   \ 8
$0039 h, 1110001000001110 h,   \ 9
                                  
$003A h, 0000010000000100 h,   \ :
$003B h, 0100100000000100 h,   \ ;
$003C h, 0100001000000010 h,   \ <
$003D h, 0000111000001110 h,   \ =
$003E h, 0100100000001000 h,   \ >
$003F h, 0110010000001110 h,   \ ?
$0040 h, 0010101000001100 h,   \ @

$0041 h, 1110101000000100 h,   \ A
$0042 h, 0110111000001110 h,   \ B
$0043 h, 1000011000000110 h,   \ C
$0044 h, 1010110000001100 h,   \ D
$0045 h, 1100111000001110 h,   \ E
$0046 h, 1100100000001110 h,   \ F
$0047 h, 1010011000000110 h,   \ G
$0048 h, 1110101000001010 h,   \ H
$0049 h, 0100111000000100 h,   \ I
$004A h, 0010110000000010 h,   \ J
$004B h, 1100101000001010 h,   \ K
$004C h, 1000111000001000 h,   \ L
$004D h, 1110101000001110 h,   \ M
$004E h, 1010101000001110 h,   \ N
$004F h, 1010010000000100 h,   \ O
$0050 h, 1110100000001100 h,   \ P
$0051 h, 1010011000000100 h,   \ Q
$0052 h, 1100101000001110 h,   \ R
$0053 h, 0100110000000110 h,   \ S
$0054 h, 0100010000001110 h,   \ T
$0055 h, 1010111000001010 h,   \ U
$0056 h, 1010010000001010 h,   \ V
$0057 h, 1110111000001010 h,   \ W
$0058 h, 0100101000001010 h,   \ X
$0059 h, 0100010000001010 h,   \ Y
$005A h, 0100111000001110 h,   \ Z

$005B h, 1000110000001100 h,   \ [
$005C h, 0100001000001000 h,   \ \
$005D h, 0010011000000110 h,   \ ]
$005E h, 1010000000000100 h,   \ ^
$005F h, 0000111000000000 h,   \ _
$0060 h, 0100000000001000 h,   \ `

$0061 h, 1110101000000100 h,   \ a -> A
$0062 h, 0110111000001110 h,   \ b -> B
$0063 h, 1000011000000110 h,   \ c -> C
$0064 h, 1010110000001100 h,   \ d -> D
$0065 h, 1100111000001110 h,   \ e -> E
$0066 h, 1100100000001110 h,   \ f -> F
$0067 h, 1010011000000110 h,   \ g -> G
$0068 h, 1110101000001010 h,   \ h -> H
$0069 h, 0100111000000100 h,   \ i -> I
$006A h, 0010110000000010 h,   \ j -> J
$006B h, 1100101000001010 h,   \ k -> K
$006C h, 1000111000001000 h,   \ l -> L
$006D h, 1110101000001110 h,   \ m -> M
$006E h, 1010101000001110 h,   \ n -> N
$006F h, 1010010000000100 h,   \ o -> O
$0070 h, 1110100000001100 h,   \ p -> P
$0071 h, 1010011000000100 h,   \ q -> Q
$0072 h, 1100101000001110 h,   \ r -> R
$0073 h, 0100110000000110 h,   \ s -> S
$0074 h, 0100010000001110 h,   \ t -> T
$0075 h, 1010111000001010 h,   \ u -> U
$0076 h, 1010010000001010 h,   \ v -> V
$0077 h, 1110111000001010 h,   \ w -> W
$0078 h, 0100101000001010 h,   \ x -> X
$0079 h, 0100010000001010 h,   \ y -> Y
$007A h, 0100111000001110 h,   \ z -> Z

$007B h, 1100100000000110 h,   \ {
$007C h, 0100010000000100 h,   \ |
$007D h, 0110001000001100 h,   \ }
$007E h, 1110100000000010 h,   \ ~

$00C4 h, 1110101000000110 h,   \ Ä
$00D6 h, 1010010000000110 h,   \ Ö
$00DC h, 1000111000001010 h,   \ Ü
$00DF h, 0110110000000110 h,   \ ß
$0000 h, 1110111000001110 h,
decimal

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
    
    4 16 \ 4 pix width, 12 pixels at all (free line below assumed)
    \ 4 16 \ change to this if you need to write empty pixels
  
    1-foldable
;

\ how to use with charwriter.fs
\ ['] fixed4x4 font-hook !
\ : helloworld s" Hello World!" 0 8 drawstring s" (äöüß$€¤£ °¶)" 0 8 drawstring ;

( fixed4x4 end: ) here dup hex.
( fixed4x4 size: ) swap - hex.


