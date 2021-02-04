# modifications mecrisp stellaris
These files modify the Mecrisp Stellaris Forth 2.5.5 for stm32f407-ra

## compile your own MecrispForth

Place the **interrupt.s**, **vector.s** and **terminal.s** inside the directory "mecrisp-stellaris-source/stm32f407-ra".

 - make clean
 - make

you will get the **mecrisp-stellaris-stm32f407.bin** binary.

## description of the changes

### terminal.s:
 - changed the terminal output to usart1 instead usart2

### vector.s and interrupt.s
 - added missing interupts to these files