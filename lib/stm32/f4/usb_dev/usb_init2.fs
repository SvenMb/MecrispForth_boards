0 variable usb-end

$40023834 constant RCC_AHB2ENR
$40023830 constant RCC_AHB1ENR
rcc_base $14 + constant RCC_AHB2RSTR
$0B constant PA11
$0C constant PA12

$50000000 constant OTG_BASE
OTG_BASE $008 + constant OTG_GAHBCFG
OTG_BASE $00C + constant OTG_GUSBCFG
OTG_BASE $010 + constant OTG_GRSTCTL
OTG_BASE $014 + constant OTG_GINTSTS  
OTG_BASE $020 + constant OTG_GRXSTSP  
OTG_BASE $024 + constant OTG_GRXFSIZ  
OTG_BASE $028 + constant OTG_DIEPTXF0 
OTG_BASE $038 + constant OTG_GCCFG    
OTG_BASE $104 + constant OTG_DIEPTXF1 
OTG_BASE $800 + constant OTG_DCFG
OTG_BASE $804 + constant OTG_DCTL
OTG_BASE $808 + constant OTG_DSTS     
OTG_BASE $900 + constant OTG_DIEPCTL0 
OTG_BASE $910 + constant OTG_DIEPTSIZ0
OTG_BASE $920 + constant OTG_DIEPCTL1 
OTG_BASE $940 + constant OTG_DIEPCTL2 
OTG_BASE $918 + constant OTG_DTXFSTS0 
OTG_BASE $B00 + constant OTG_DOEPCTL0 
OTG_BASE $B10 + constant OTG_DOEPTSIZ0
OTG_BASE $E00 + constant OTG_PCGCCTL

: usb_core_init

    \ Reset USB OTG_FS
    1 7 lshift RCC_AHB2RSTR bis!
    100 ms
    1 7 lshift RCC_AHB2RSTR bic!
    100 ms
    
    \ enable USBOTG
    $80 RCC_AHB2ENR bis!  \ enable OTG FS (7 bit)

    \ enable GPIOA clock (already done in mecrisp terminal.s)
    \ $01 RCC_AHB1ENR bis! \ GPIOAEN 

    \ set led as output for debuging
    omode-od 6 io-mode! 6 ios!
    omode-od 7 io-mode! 7 ios!
    
    
    \ set usb output pin, why?  
    PA12 OMODE-OD OMODE-SLOW + io-mode!
    3 ms
    
    \ 10 12 lshift \ pin 11, AF mode  10 (USB)
    \ 10 16 lshift or
    \ GPIOA GPIO.AFRH + bis! \ set AF for GPIOA
    10 PA11 alt-mode!
    10 PA12 alt-mode!
    
    PA11 OMODE-AF-PP OMODE-SLOW + io-mode!
    \ PA11 OMODE-AF-PP OMODE-FAST + io-mode!
        
    PA12 OMODE-AF-PP OMODE-SLOW + io-mode!
    \ PA12 OMODE-AF-PP OMODE-FAST + io-mode!


    1 6 lshift OTG_GUSBCFG bis! \ FS phy-select

    1 0 lshift OTG_GRSTCTL bis! \ set CSRST (softreset)

    begin 1 0 lshift OTG_GRSTCTL bit@ not until \ wait for reset end

    1 21 lshift 1 16 lshift or OTG_GCCFG bis! \ set novbussens, pwrdwn deactivate
        
    1 30 lshift OTG_GUSBCFG bis!         \ set fdmod

    30 ms \ wait for device mode to settle ...
;

: usb_dev_init


    \ see stm32 HAl, why?
    4 0 do
        0 OTG_DIEPTXF1 I + ! 
    loop

    \ soft disconnect
    1 1 lshift OTG_DCTL bis!

    \ novbussens
    1 21 lshift OTG_GCCFG bis!
    \ no vbus sens for dev or host
    1 18 lshift 1 19 lshift or OTG_GCCFG bic!

    \ restart phy clock
    0 OTG_PCGCCTL !

    \
    OTG_DCFG bis! 
    
    
    %11 OTG_DCFG bis!               \ set dspd to full speed (usb 1.1)
;

: usb_poll

    OTG_GINTSTS @ \ get usb irqs
    dup OTG_GINTSTS ! \ reset all usb irqs
    dup 1 12 lshift and if \ USBRST
        CR ." reset done"

        1 27 lshift        
        OTG_DOEPCTL0 ! \ set SNAK

        \ reserve usb buffer mem
        512 4 / OTG_GRXFSIZ h! \ 512b for RX
        128 4 / 16 lshift 512 or OTG_DIEPTXF0 ! \ 128 byte for ep0 
        512 4 / 16 lshift 512 128 + or OTG_DIEPTXF1 ! \ 512 byte for ep1

        \ endpoint 0 transfer size
        %11 29 lshift 64 or
        OTG_DOEPTSIZ0 ! \ set stupcnt=3, xfrsize=64

    then
    dup 1 13 lshift and if \ enumdne
        CR ." enum done"
        OTG_BASE $808 + @ hex.


        \ fifo 1 
        1 31 lshift 1 26 lshift or 1 22 lshift or %10 18 lshift or 1 15 lshift or 64 or 
        OTG_DIEPCTL1 ! \ set EPENA,CNAK, TXFNUM=1, EPTYP=2 (bulk), USBAEP, maxpkgsiz=64

        \ fifo 2
        1 23 lshift %11 18 lshift or 1 15 lshift or 64 or
        OTG_DIEPCTL2 ! \ set TXFNUM=2, EPTYP=3 (irq), USBAEP, maxpkgsiz=64  


        \ endpoint0 ctrl
        1 31 lshift
        \ 1 15 lshift or
        1 26 lshift or
        OTG_DOEPCTL0 ! \ set EPENA, CNAK, USBAEP
    then
    dup CR hex.
    1 4 lshift and if \ packet rx pending
        CR ." pkg pending!"
        OTG_GRXSTSP @
        dup 17 rshift ." Typ:" hex.
        dup $F and ." chan:" hex.
        dup 4 rshift $7ff and ." count:" hex.
        dup 17 rshift case
            %0010 of \ out
            endof
            %0011 of \ out end
            endof    
            %0110 of \ setup
            endof
            %0100 of \ setup end
            endof
        endcase
    true usb-end !
    then
;

: test
    usb_init
    CR ." init done"
    begin
        usb_poll
        usb-end @ 
        key? or
    until
;



