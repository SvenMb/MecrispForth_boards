@
@    Mecrisp-Stellaris - A native code Forth implementation for ARM-Cortex M microcontrollers
@    Copyright (C) 2013  Matthias Koch
@
@    This program is free software: you can redistribute it and/or modify
@    it under the terms of the GNU General Public License as published by
@    the Free Software Foundation, either version 3 of the License, or
@    (at your option) any later version.
@
@    This program is distributed in the hope that it will be useful,
@    but WITHOUT ANY WARRANTY; without even the implied warranty of
@    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
@    GNU General Public License for more details.
@
@    You should have received a copy of the GNU General Public License
@    along with this program.  If not, see <http://www.gnu.org/licenses/>.
@

@ -----------------------------------------------------------------------------
@ Interruptvektortabelle
@ -----------------------------------------------------------------------------

.include "../common/vectors-common.s"

@ Special interrupt handlers for this particular chip:

.word nullhandler+1 @ Position  0: Window Watchdog
.word nullhandler+1 @ Position  1: PVD through EXTI line detection
.word nullhandler+1 @ Position  2: Tamper and TimeStamp through EXTI line
.word irq_vektor_rtc+1 @ Position  3: RTC Wakeup
.word nullhandler+1 @ Position  4: Flash
.word nullhandler+1 @ Position  5: RCC
.word irq_vektor_exti0+1 @ Position  6: EXTI Line 0
.word irq_vektor_exti1+1 @ Position  7: EXTI Line 1
.word irq_vektor_exti2+1 @ Position  8: EXTI Line 2
.word irq_vektor_exti3+1 @ Position  9: EXTI Line 3
.word irq_vektor_exti4+1 @ Position 10: EXTI Line 4
.word nullhandler+1 @ Position 11: DMA1 Stream 0
.word nullhandler+1 @ Position 12: DMA1 Stream 1
.word nullhandler+1 @ Position 13: DMA1 Stream 2
.word nullhandler+1 @ Position 14: DMA1 Stream 3
.word nullhandler+1 @ Position 15: DMA1 Stream 4
.word nullhandler+1 @ Position 16: DMA1 Stream 5
.word nullhandler+1 @ Position 17: DMA1 Stream 6
.word irq_vektor_adc+1 @ Position 18: ADC global interrupts
.word nullhandler+1 @ Position 19:
.word nullhandler+1 @ Position 20:
.word nullhandler+1 @ Position 21:
.word nullhandler+1 @ Position 22:
.word irq_vektor_exti5+1 @ Position 23: EXTI9_5
.word irq_vektor_tim1brk+1 @ Position 24: TIM1_BRK
.word irq_vektor_tim1up+1 @ Position 25: TIM1_UP
.word irq_vektor_tim1trg+1 @ Position 26: TIM1_TRG_COM
.word irq_vektor_tim1cc+1 @ Position 27: TIM1_CC
.word irq_vektor_tim2+1 @ Position 28: Timer 2 global interrupt
.word irq_vektor_tim3+1 @ Position 29: Timer 3 global interrupt
.word irq_vektor_tim4+1 @ Position 30: Timer 4 global interrupt

.word irq_vektor_i2c1ev+1 @ Position 31: I2C1_EV
.word irq_vektor_i2c1er+1 @ Position 32: I2C1_ER
.word irq_vektor_i2c2ev+1 @ Position 33: I2C2_EV
.word irq_vektor_i2c2er+1 @ Position 34: I2C2_ER
.word irq_vektor_spi1+1 @ Position 35: SPI1
.word irq_vektor_spi2+1 @ Position 36: SPI2
.word irq_vektor_usart1+1 @ Position 37: USART1
.word irq_vektor_usart2+1 @ Position 38: USART2
.word irq_vektor_usart3+1 @ Position 39: USART3
.word irq_vektor_exti10+1 @ Position 40: EXTI15_10

.word irq_vektor_rtcalarm+1 @ Position 41: RTCAlarm
.word irq_vektor_usbwkup+1 @ Position 42: OTG_FS_WKUP
.word nullhandler+1 @ Position 43:
.word nullhandler+1 @ Position 44:
.word nullhandler+1 @ Position 45:
.word nullhandler+1 @ Position 46:
.word nullhandler+1 @ Position 47:
.word nullhandler+1 @ Position 48:
.word nullhandler+1 @ Position 49:

.word irq_vektor_tim5+1 @ Position 50: TIM5
.word irq_vektor_spi3+1 @ Position 51: SPI3
.word nullhandler+1 @ Position 52:
.word nullhandler+1 @ Position 53:
.word nullhandler+1 @ Position 54:
.word nullhandler+1 @ Position 55:
.word nullhandler+1 @ Position 56:
.word nullhandler+1 @ Position 57:
.word nullhandler+1 @ Position 58:
.word nullhandler+1 @ Position 59:

.word nullhandler+1 @ Position 60:
.word nullhandler+1 @ Position 61:
.word nullhandler+1 @ Position 62:
.word nullhandler+1 @ Position 63:
.word nullhandler+1 @ Position 64:
.word nullhandler+1 @ Position 65:
.word nullhandler+1 @ Position 66:
.word irq_vektor_usbfs+1 @ Position 67: OTG_FS
.word nullhandler+1 @ Position 68:
.word nullhandler+1 @ Position 69:

.word nullhandler+1 @ Position 70:
.word nullhandler+1 @ Position 71:
.word nullhandler+1 @ Position 72:
.word nullhandler+1 @ Position 73:
.word nullhandler+1 @ Position 74:
.word nullhandler+1 @ Position 75:
.word nullhandler+1 @ Position 76:
.word nullhandler+1 @ Position 77:
.word nullhandler+1 @ Position 78:
.word nullhandler+1 @ Position 79:

.word nullhandler+1 @ Position 80:
.word nullhandler+1 @ Position 81:

@ -----------------------------------------------------------------------------
