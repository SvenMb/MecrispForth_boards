# ILI9341 via FSMC 16bit

## Commands

```c++
#define ILI9341_RESET			 		0x0001
#define ILI9341_SLEEP_OUT		 	 	0x0011
#define ILI9341_GAMMA			    	0x0026
#define ILI9341_DISPLAY_OFF				0x0028
#define ILI9341_DISPLAY_ON				0x0029
#define ILI9341_COLUMN_ADDR				0x002A
#define ILI9341_PAGE_ADDR			  	0x002B
#define ILI9341_GRAM				    0x002C
#define ILI9341_TEARING_OFF				0x0034
#define ILI9341_TEARING_ON				0x0035
#define ILI9341_DISPLAY_INVERSION		0x00b4
#define ILI9341_MAC			        	0x0036
#define ILI9341_PIXEL_FORMAT    		0x003A
#define ILI9341_WDB			    		0x0051
#define ILI9341_WCD				    	0x0053
#define ILI9341_RGB_INTERFACE   		0x00B0
#define ILI9341_FRC					    0x00B1
#define ILI9341_BPC					    0x00B5
#define ILI9341_DFC				 	    0x00B6
#define ILI9341_Entry_Mode_Set			0x00B7
#define ILI9341_POWER1					0x00C0
#define ILI9341_POWER2					0x00C1
#define ILI9341_VCOM1					0x00C5
#define ILI9341_VCOM2					0x00C7
#define ILI9341_POWERA					0x00CB
#define ILI9341_POWERB					0x00CF
#define ILI9341_PGAMMA					0x00E0
#define ILI9341_NGAMMA					0x00E1
#define ILI9341_DTCA					0x00E8
#define ILI9341_DTCB					0x00EA
#define ILI9341_POWER_SEQ				0x00ED
#define ILI9341_3GAMMA_EN				0x00F2
#define ILI9341_INTERFACE				0x00F6
#define ILI9341_PRC				   	    0x00F7
#define ILI9341_VERTICAL_SCROLL 		0x0033
```

## Colors

```c++
#define BLACK           0x0000      /*   0,   0,   0 */
#define NAVY            0x000F      /*   0,   0, 128 */
#define DGREEN          0x03E0      /*   0, 128,   0 */
#define DCYAN           0x03EF      /*   0, 128, 128 */
#define MAROON          0x7800      /* 128,   0,   0 */
#define PURPLE          0x780F      /* 128,   0, 128 */
#define OLIVE           0x7BE0      /* 128, 128,   0 */
#define LGRAY           0xC618      /* 192, 192, 192 */
#define DGRAY           0x7BEF      /* 128, 128, 128 */
#define BLUE            0x001F      /*   0,   0, 255 */
#define BLUE2			0x051D
#define GREEN           0x07E0      /*   0, 255,   0 */
#define GREEN2		    0xB723
#define GREEN3		    0x8000
#define CYAN            0x07FF      /*   0, 255, 255 */
#define RED             0xF800      /* 255,   0,   0 */
#define MAGENTA         0xF81F      /* 255,   0, 255 */
#define YELLOW          0xFFE0      /* 255, 255,   0 */
#define WHITE           0xFFFF      /* 255, 255, 255 */
#define ORANGE          0xFD20      /* 255, 165,   0 */
#define GREENYELLOW     0xAFE5      /* 173, 255,  47 */
#define BROWN 			0xBC40 //
#define BRRED 			0xFC07 //
```

## Size

```c++
#define ILI9341_WIDTH       240
#define ILI9341_HEIGHT      320
#define ILI9341_PIXEL_COUNT	ILI9341_WIDTH * ILI9341_HEIGHT
```

