## pins for sdcard interface

Using stm32-sdio-interface

SDCARD       | STM32F407 | 
-------------|-----------|-----
SDIO_D0      | PC8       |
SDIO_D1      | PC9       |
SDIO_D2      | PC10      |
SDIO_D3 (CD) | PC11      |
SDIO_SCK     | PC12      |
SDIO_CMD     | PD2       |

TODO:
* sdio_clk_enable
* enable dma2 txrx clock (or poll)
* enable gpio clocks C+D
* gpioc8-12 mode_af_pp, pullup, speed_frq_very_high, af12_sdio
* gpiod2  --"--  

## pins for onboard flash

flash (W25Q16)  | STM32F407  
----------------|----------------------
CS  (F_CS)      | PB0
SO  (SPI1_MISO) | PB4 (also for nrf24)
CLK (SPI1_SCK)  | PB3 (also for nrf24)
SI  (SPI1_MOSI) | PB5 (also for nrf24)

* spi1-init


