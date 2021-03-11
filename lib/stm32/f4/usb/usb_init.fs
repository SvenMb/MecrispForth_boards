0 variable usb-end

: usb_init
    \ set usb pins 
    \ PA11 OMODE-AF-OD-hs-high io-mode!
    PA12 OMODE-AF-OD io-mode!


    bit 29 bit 30 or rcc_ahb1enr bic! \ disable Highspeed
    7 bit rcc_ahb2enr bis!  \ enable OTG FS

    7 bit rcc_ahb2rstr bis! 500 ms 7 bit rcc_ahb2rstr bic! \ reset OTG FS module
    100 ms

    7 bit rcc_ahb2enr bis!  \ enable OTG FS

    21 bit 16 bit or OTG_FS_GLOBAL_FS_GCCFG bis! \ set novbussens, pwrdwn deactivate
    30 bit OTG_FS_GLOBAL_FS_GUSBCFG bis!         \ set fdmod
    30 ms
    %11 OTG_FS_DEVICE_FS_DCFG bis!               \ set dspd to full speed (usb 1.1)
;

: usb_poll

    OTG_FS_GLOBAL_FS_GINTSTS @ \ get usb irqs
    dup CR hex.
    dup OTG_FS_GLOBAL_FS_GINTSTS ! \ reset all usb irqs
    dup 13 bit and if \ enumdne
        CR ." enum done"

        \ reserve usb buffer mem
        512 4 / OTG_FS_GLOBAL_FS_GRXFSIZ h! \ 512b for RX
        128 4 / 16 lshift 512 or OTG_FS_GLOBAL_FS_DIEPTXF0 \ 128 byte for ep0 
        512 4 / 16 lshift 512 128 + or OTG_FS_GLOBAL_FS_DIEPTXF1 \ 512 byte for ep1

        \ fifo 1 
        31 bit 26 bit or 22 bit or %10 18 lshift or 15 bit or 64 or 
        OTG_FS_DEVICE_DIEPCTL1 ! \ set EPENA,CNAK, TXFNUM=1, EPTYP=2 (bulk), USBAEP, maxpkgsiz=64

        \ fifo 2
        23 bit %11 18 lshift or 15 bit or 64 or
        OTG_FS_DEVICE_DIEPCTL2 ! \ set TXFNUM=2, EPTYP=3 (irq), USBAEP, maxpkgsiz=64  

        \ endpoint 0 transfer size
        %11 29 lshift 64 or
        OTG_FS_DEVICE_DOEPTSIZ0 ! \ set stupcnt=3, xfrsize=64

        \ endpoint0 ctrl
        31 bit 15 bit or 26 bit or
        OTG_FS_DEVICE_DOEPCTL0 ! \ set EPENA, CNAK, USBAEP
    then
    4 bit and if \ packet rx pending
        CR ." pkg pending! "
        OTG_FS_GLOBAL_FS_GRXSTSP
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



