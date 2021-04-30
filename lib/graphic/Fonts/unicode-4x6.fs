\ ---
\ subject: unicode fixed font 4x6 size
\ origin: mecrisp2.5.8 distribution (common/graphics/graphics-unicode-4x6.txt)
\ author: Sven Muehlberg
\ copyright: see below and mecrisp from Mathias Koch
\ ---

( fixed4x6 start: ) here dup hex.

\ -------------------------------------------------------------
\  Artwork for 4x6 Bitmap font, taken from "Tom Thumb"
\  https://robey.lag.net/2010/01/23/tiny-monospace-font.html
\ -------------------------------------------------------------

decimal
create font4x6

\ Unicode point, Bitmap Data

 32 h,  $00000000 ,   \   
 33 h,  $00404044 ,   \ ! 
 34 h,  $000000AA ,   \ " 
 35 h,  $00A0AEAE ,   \ # 
 36 h,  $00406C6C ,   \ $ 
 37 h,  $00204882 ,   \ % 
 38 h,  $0060EACC ,   \ & 
 39 h,  $00000044 ,   \ ' 
 40 h,  $00204424 ,   \ ( 
 41 h,  $00804484 ,   \ ) 
 42 h,  $0000A0A4 ,   \ * 
 43 h,  $0000E404 ,   \ + 
 44 h,  $00800400 ,   \ , 
 45 h,  $0000E000 ,   \ - 
 46 h,  $00400000 ,   \ . 
 47 h,  $00804822 ,   \ / 
 48 h,  $00C0AA6A ,   \ 0 
 49 h,  $0040444C ,   \ 1 
 50 h,  $00E048C2 ,   \ 2 
 51 h,  $00C042C2 ,   \ 3 
 52 h,  $0020E2AA ,   \ 4 
 53 h,  $00C0C2E8 ,   \ 5 
 54 h,  $00E0EA68 ,   \ 6 
 55 h,  $008048E2 ,   \ 7 
 56 h,  $00E0EAEA ,   \ 8 
 57 h,  $00C0E2EA ,   \ 9 
 58 h,  $00000404 ,   \ : 
 59 h,  $00800404 ,   \ ; 
 60 h,  $00208424 ,   \ < 
 61 h,  $00000E0E ,   \ = 
 62 h,  $00802484 ,   \ > 
 63 h,  $004040E2 ,   \ ? 
 64 h,  $0060E84A ,   \ @ 
 65 h,  $00A0EA4A ,   \ A 
 66 h,  $00C0CACA ,   \ B 
 67 h,  $00608868 ,   \ C 
 68 h,  $00C0AACA ,   \ D 
 69 h,  $00E0E8E8 ,   \ E 
 70 h,  $0080E8E8 ,   \ F 
 71 h,  $0060EA68 ,   \ G 
 72 h,  $00A0EAAA ,   \ H 
 73 h,  $00E044E4 ,   \ I 
 74 h,  $00402A22 ,   \ J 
 75 h,  $00A0CAAA ,   \ K 
 76 h,  $00E08888 ,   \ L 
 77 h,  $00A0EAAE ,   \ M 
 78 h,  $00A0EEAE ,   \ N 
 79 h,  $0040AA4A ,   \ O 
 80 h,  $0080C8CA ,   \ P 
 81 h,  $0060AE4A ,   \ Q 
 82 h,  $00A0ECCA ,   \ R 
 83 h,  $00C04268 ,   \ S 
 84 h,  $004044E4 ,   \ T 
 85 h,  $0060AAAA ,   \ U 
 86 h,  $0040A4AA ,   \ V 
 87 h,  $00A0EEAA ,   \ W 
 88 h,  $00A04AAA ,   \ X 
 89 h,  $004044AA ,   \ Y 
 90 h,  $00E048E2 ,   \ Z 
 91 h,  $00E088E8 ,   \ [ 
 92 h,  $00004208 ,   \ \ 
 93 h,  $00E022E2 ,   \ ] 
 94 h,  $0000004A ,   \ ^ 
 95 h,  $00E00000 ,   \ _ 
 96 h,  $00000084 ,   \ ` 
 97 h,  $00E06A0C ,   \ a 
 98 h,  $00C0AA8C ,   \ b 
 99 h,  $00608806 ,   \ c 
100 h,  $0060AA26 ,   \ d
101 h,  $0060AC06 ,   \ e
102 h,  $0040E424 ,   \ f
103 h,  $0024AE06 ,   \ g
104 h,  $00A0AA8C ,   \ h
105 h,  $00404440 ,   \ i
106 h,  $00A42220 ,   \ j
107 h,  $00A0CC8A ,   \ k
108 h,  $00E044C4 ,   \ l
109 h,  $00A0EE0E ,   \ m
110 h,  $00A0AA0C ,   \ n
111 h,  $0040AA04 ,   \ o
112 h,  $00C8AA0C ,   \ p
113 h,  $0062AA06 ,   \ q
114 h,  $00808806 ,   \ r
115 h,  $00C0C606 ,   \ s
116 h,  $0060444E ,   \ t
117 h,  $0060AA0A ,   \ u
118 h,  $0040AE0A ,   \ v
119 h,  $00E0EE0A ,   \ w
120 h,  $00A0440A ,   \ x
121 h,  $0024A60A ,   \ y
122 h,  $00E06C0E ,   \ z
123 h,  $00608464 ,   \ {
124 h,  $00400444 ,   \ |
125 h,  $00C024C4 ,   \ }
126 h,  $0000006C ,   \ ~
161 h,  $00404440 ,   \ ¡
162 h,  $00408E4E ,   \ ¢
163 h,  $00E0E464 ,   \ £
164 h,  $00A0E4A4 ,   \ €
165 h,  $00404EAA ,   \ ¥
166 h,  $00400444 ,   \ Š
167 h,  $00C0A464 ,   \ §
168 h,  $000000A0 ,   \ š
169 h,  $00006068 ,   \ ©
170 h,  $00E0E06A ,   \ ª
171 h,  $00004048 ,   \ «
172 h,  $0000200E ,   \ ¬
173 h,  $0000C000 ,   \ ­
174 h,  $0000A0CC ,   \ ®
175 h,  $000000E0 ,   \ ¯
176 h,  $0000404A ,   \ °
177 h,  $00E0404E ,   \ ±
178 h,  $000060C4 ,   \ ²
179 h,  $0000E0E6 ,   \ ³
180 h,  $00000024 ,   \ Ž
181 h,  $0080ACAA ,   \ µ
182 h,  $0060666A ,   \ ¶
183 h,  $0000EE0E ,   \ ·
184 h,  $00C04200 ,   \ ž
185 h,  $00004044 ,   \ ¹
186 h,  $00E0404A ,   \ º
187 h,  $00004042 ,   \ »
188 h,  $00200688 ,   \ Œ
189 h,  $00600C88 ,   \ œ
190 h,  $002006CC ,   \ Ÿ
191 h,  $00E04840 ,   \ ¿
192 h,  $00A04E42 ,   \ À
193 h,  $00A04E48 ,   \ Á
194 h,  $00A04EE0 ,   \ Â
195 h,  $00A04E6C ,   \ Ã
196 h,  $00A0AEA4 ,   \ Ä
197 h,  $00A0AECC ,   \ Å
198 h,  $00E0EC6C ,   \ Æ
199 h,  $00248668 ,   \ Ç
200 h,  $00E0EC42 ,   \ È
201 h,  $00E0EC48 ,   \ É
202 h,  $00E0ECE0 ,   \ Ê
203 h,  $00E0ECA0 ,   \ Ë
204 h,  $00E0E442 ,   \ Ì
205 h,  $00E0E448 ,   \ Í
206 h,  $00E0E4E0 ,   \ Î
207 h,  $00E0E4A0 ,   \ Ï
208 h,  $00C0EACA ,   \ Ð
209 h,  $00A0AEC6 ,   \ Ñ
210 h,  $00E0EA42 ,   \ Ò
211 h,  $00E0EA48 ,   \ Ó
212 h,  $00E0EAE0 ,   \ Ô
213 h,  $00E0EAC6 ,   \ Õ
214 h,  $00E0EAA0 ,   \ Ö
215 h,  $00004A0A ,   \ ×
216 h,  $00C0EA6A ,   \ Ø
217 h,  $00E0AA84 ,   \ Ù
218 h,  $00E0AA24 ,   \ Ú
219 h,  $00E0AAE0 ,   \ Û
220 h,  $00E0AAA0 ,   \ Ü
221 h,  $0040AE24 ,   \ Ý
222 h,  $0080AE8E ,   \ Þ
223 h,  $00C8CA6A ,   \ ß
224 h,  $00E06A42 ,   \ à
225 h,  $00E06A48 ,   \ á
226 h,  $00E06AE0 ,   \ â
227 h,  $00E06A6C ,   \ ã
228 h,  $00E06AA0 ,   \ ä
229 h,  $00E06A66 ,   \ å
230 h,  $00C0EE06 ,   \ æ
231 h,  $00248606 ,   \ ç
232 h,  $00606E42 ,   \ è
233 h,  $00606E48 ,   \ é
234 h,  $00606EE0 ,   \ ê
235 h,  $00606EA0 ,   \ ë
236 h,  $00404442 ,   \ ì
237 h,  $00404448 ,   \ í
238 h,  $004044E0 ,   \ î
239 h,  $004044A0 ,   \ ï
240 h,  $00606A6C ,   \ ð
241 h,  $00A0CAC6 ,   \ ñ
242 h,  $00404A42 ,   \ ò
243 h,  $00404A48 ,   \ ó
244 h,  $00404AE0 ,   \ ô
245 h,  $00404AC6 ,   \ õ
246 h,  $00404AA0 ,   \ ö
247 h,  $0040E040 ,   \ ÷
248 h,  $00C0EA06 ,   \ ø
249 h,  $0060AA84 ,   \ ù
250 h,  $0060AA24 ,   \ ú
251 h,  $0060AAE0 ,   \ û
252 h,  $0060AAA0 ,   \ ü
253 h,  $0024A624 ,   \ ý
254 h,  $00C8CA08 ,   \ þ
255 h,  $0024A6A0 ,   \ ÿ
285 h,  $00000000 ,
338 h,  $0060EC6C ,
339 h,  $00E0EC06 ,
352 h,  $00C0C6A6 ,
353 h,  $00C0C6A6 ,
376 h,  $0040A4A0 ,
381 h,  $00E06CAE ,
382 h,  $00E06CAE ,
3748 h,  $00000000 ,
5024 h,  $00000000 ,
8226 h,  $00004000 ,
8230 h,  $00A00000 ,
8364 h,  $0060EC6E ,
65533 h,  $00E0AAEA ,
0 h,  $00E0AAEA ,  \ End of font marker, replacement glyph.

align decimal

\ public mapper for font-hook, see charwriter.fs

: fixed4x6 ( x -- c-addr width pixels) \ Translates Unicode character to address of 4x6 bitmap.

    font4x6 ( x addr )
    begin
        2dup h@
        dup -rot  <>  and \ true if character found, also if cur char# is $0
    while \ or no more characters left in the glyph collection...
            6 + \ Not this one, skip it and its bitmap
    repeat

    nip 2 + \ Character not found within available collection of glyphs. Display replacement.
    
    4 24 \ add size of character, 4 pix wide, 24 pix per char 
    
    1-foldable
;

\ how to use with charwriter.fs
\ ['] fixed4x6 font-hook !
\ : helloworld s" Hello World!" 0 8 drawstring s" (äöüß$€¤£ °¶)" 0 14 drawstring ;

( fixed4x6 end: ) here dup hex.
( fixed4x6 size: ) swap - hex.
