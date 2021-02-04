\ simple one-shot ADC
\ for stm32f411, maybe other stm32f4
\ needs stm32f411.fs - register definitions
\ needs io.fs

\ factory calib data
$1FFF7A2A  constant VREFIN_CAL \ reference Voltage reading at 3.3V 30 C
$1FFF7A2C  constant TS_CAL_30  \ temperature reading at 30 C 3.3V
$1FFF7A2E  constant TS_CAL_110 \ temperature reading at 110 C 3.3V

: adc-calib ( -- )  \ not needed on F4, retained for compatibility
;

: adc-once ( -- u )  \ read ADC value once
  0 bit ADC1_CR2 bis!  \ set ADON to start ADC
  30 bit ADC1_CR2 bis! \ set SWSTART to begin conversion
  begin 1 bit ADC1_SR bit@ until  \ wait until EOC set
  ADC1_DR @ ;

: adc-init ( -- )  \ initialise ADC, if used before that it will hang!
  8 bit RCC_APB2ENR bis!  \ set ADC1EN
  23 bit ADC_Common_CCR bis! \ set TSVREFE for vRefInt use
   0 bit ADC1_CR2 bis!  \ set ADON to enable ADC
  \ 7.5 cycles sampling time is enough for 18 kΩ to ground, measures as zero
  \ even 239.5 cycles is not enough for 470 kΩ, it still leaves 70 mV residual
  %111 21 lshift ADC1_SMPR1 bis! \ set SMP17 to 480 cycles for vRefInt
  %111 24 lshift ADC1_SMPR1 bis! \ set SMP18 to 480 cycles for vBat
  %110110110 ADC1_SMPR2 bis! \ set SMP0/1/2 to 144 cycles
  adc-once drop
;

: adc# ( pin -- n )  \ convert pin number to adc index
\ nasty way to map the pins (a "c," table offset lookup might be simpler)
\   PA0..7 => 0..7, PB0..1 => 8..9, PC0..5 => 10..15
  dup io# swap  io-port ?dup if shl + 6 + then ;

: adc ( pin -- u )  \ read ADC value 0...$FFF for 0-3,3V
\ IMODE-ADC over io-mode!
\ nasty way to map the pins (a "c," table offset lookup might be simpler)
\   PA0..7 => 0..7, PB0..1 => 8..9, PC0..5 => 10..15
  adc# ADC1_SQR3 !  adc-once ;

: adc-vref ( -- mv )       \ read current Vref value
  22 bit adc_common_ccr bic! \ unset VBATE
  23 bit adc_common_ccr bis!  \ set TSVREFE
  17 ADC1_SQR3 ! adc-once
;

\ unfortunately on most boeards (like weact stm32f411) this is connected via diode to Vcc, so reading real Vbat not possible
: adc-vbat ( -- mv )  \ return estimated Vbat
  22 bit adc_common_ccr bis! \ set VBATE
  23 bit adc_common_ccr bic! \ unset TSVREFE
  18 ADC1_SQR3 ! adc-once
  VREFIN_CAL h@ $FFF and adc-vref */ \ adjust temperatur
  3300 $400 */ \ adjust to real volt
;

: fadc ( u -- c-addr length ) \ make string like 0,123
  0 <# # # # $2c hold # #>
;

: fvadc ( u -- c-adddr length ) \ makes string like 0,123V
  0 <# $56 hold # # # $2c hold # #>
;

: adc. ( pin -- ) \ prints voltage of a pin
\ set pin to imode-adc before that
\ Ex: IMODE-ADC PA0 io-mode! PA0 adc.
  adc
  VREFIN_CAL h@ $FFF and adc-vref */ \ adjust temperatur
  3300 $1000 */ \ adjust to 3.3 Volt
  fvadc
  type
;

: adc-vbat. ( -- ) \ print current Vbat
  adc-vbat
  fvadc
  type
;

: adc-temp ( -- cC )  \ return estimated Temp in 1/100 Celsius
  22 bit adc_common_ccr bic! \ unset VBATE for measuring Temp 
  23 bit adc_common_ccr bis! \ set TSVREFE for measuring Temp 
  18 ADC1_SQR3 !  adc-once
  VREFIN_CAL h@ $FFF and adc-vref */ 
  TS_CAL_30 h@ $fff and - \ diff to 30degree calib data 
  8000 TS_CAL_110 h@ $fff and TS_CAL_30 h@ $fff and - */ \ use chip calib data
  3000 + \ add 30 degree back
;

: ftemp ( cC -- c-addr length )
  0 <# $43 hold $27 hold # # $2C hold # # #>
;

: futemp ( cC -- c-addr length )
  0 <# $43 hold $B0 hold $C2 hold # # $2C hold # # #> \ need unicode terminal!
;

: adc-temp. ( -- ) \ don't forget adc-init before
  adc-temp
  futemp
  type
;

