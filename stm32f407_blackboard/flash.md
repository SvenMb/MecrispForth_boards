## generate your own mecrisp-forth kernel

* self compiling:

    ```bash
    cd mecrisp-stellaris-2.5.8/mecrisp-stellaris-source/stm32f407-ra
    # --> extend avaiable irq for mecrisp:
    cp  $mysrc/stm32f407-ra/interrupts.s   $mysrc/stm32f407-ra/vectors.s .
    # --> optionaly change terminal to USART1
    cp $mysrc/stm32f407-ra/terminal.s .
    # --> generate .bin and .elf files
    make 
    # --> generate .hex file for gdb
    arm-none-eabi-objcopy --change-addresses 0x08000000 -I binary -O ihex mecrisp-stellaris-stm32f407.bin mecrisp-stellaris-stm32f407.hex
    ```

## Flash with Black Magic Probe

 Attach the BMP to your target and computer USB. There should be 2 new serial ports, first is debug, second is serial.


```bash
gdb-multiarch
(gdb) target extended-remote /dev/ttyACM0 # or whatever port your debug port is
##(gdb) monitor tpwr enable <-- only if Power should be delivered from BMP(!)
(gdb) monitor swdp_scan
Target voltage: OK
Available Targets:
No. Att Driver
 1      STM32F40x M4
(gdb) attach 1
Attaching to Remote target 
(gdb) flash-erase
Erasing flash memory region at address 0x8000000, size = 0x10000
Erasing flash memory region at address 0x8010000, size = 0x10000
Erasing flash memory region at address 0x8020000, size = 0xe0000
(gdb) load mecrisp-stellaris-stm32f407.hex
Loading section .sec1, size 0x521c lma 0x8000000
Start address 0x8000000, load size 21020
Transfer rate: 25 KB/sec, 944 bytes/write.
(gdb) detach
(gdb) quit
```
now reset target and it should respond over serial port with:

`Mecrisp-Stellaris RA 2.5.8 for STM32F407 by Matthias Koch`

## connect to mecrisp-forth REPL

usage example with folie and Black Magic Probe serial port: 

```bash
folie -r
Select the serial port: 
  1: /dev/ttyACM0 
  2: /dev/ttyACM1
2
Enter '!help' for additional help, or ctrl-d to quit.
ok. 
```

