#include <stdio.h>
#include <stdbool.h>

#include "stm32f4xx.h"
#include "stm32f4xx_spi.h"
#include "stm32f4xx_gpio.h"
#include "stm32f4xx_rcc.h"
#include "GUI.h"
#include "TouchPanel.h"


#define SPI_PORT	SPI2

#define TOUCH_CS_PORT	 GPIOB
#define TOUCH_CS_PIN	 GPIO_Pin_12

#define TOUCH_IRQ_PORT	 GPIOC
#define TOUCH_IRQ_PIN	   GPIO_Pin_5

#define T_CS()   GPIO_ResetBits ( TOUCH_CS_PORT, TOUCH_CS_PIN );
#define T_DCS()  GPIO_SetBits ( TOUCH_CS_PORT, TOUCH_CS_PIN );

static uint16_t SPI_Write ( uint16_t cmd );
static void SpiDelay ( uint16_t DelayCnt );

static uint16_t TPReadX ( void );
static uint16_t TPReadY ( void );
static bool TouchVerifyCoef ( void );
bool isTouch ( void );
static int16_t ax, bx, ay, by;
static int16_t axc[2], ayc[2], bxc[2], byc[2];
static const int16_t xCenter[] = { 35, LCD_PIXEL_WIDTH-35, 35, LCD_PIXEL_WIDTH-35 };
static const int16_t yCenter[] = { 35, 35, LCD_PIXEL_HEIGHT-35, LCD_PIXEL_HEIGHT-35 };
static int16_t xPos[5], yPos[5];

static uint16_t SPI_Write ( uint16_t cmd )
{
	uint16_t val, MSB, LSB;

	SpiDelay ( 10 );
	SPI_I2S_SendData ( SPI_PORT, cmd );

	// Wait to receive a byte
	while ( SPI_I2S_GetFlagStatus ( SPI_PORT, SPI_I2S_FLAG_RXNE ) == RESET ) { ; }
	MSB = SPI_I2S_ReceiveData ( SPI_PORT );
	SPI_I2S_SendData ( SPI_PORT, 0 );
	while ( SPI_I2S_GetFlagStatus ( SPI_PORT, SPI_I2S_FLAG_RXNE ) == RESET ) { ; }
	// Return the byte read from the SPI bus
	LSB = SPI_I2S_ReceiveData ( SPI_PORT );
	val = ( ((MSB<<4) & 0x0FF0) | ((LSB>>12) & 0x000F) )<<1;

	SpiDelay ( 10 );
	return val;
}

void TouchInit ( void )
{
  GPIO_InitTypeDef GPIO_InitStruct;
  SPI_InitTypeDef  SPI_InitStructure;

  RCC_AHB1PeriphClockCmd (RCC_AHB1Periph_GPIOB| RCC_AHB1Periph_GPIOC, ENABLE );

  GPIO_InitStruct.GPIO_Pin = GPIO_Pin_13|GPIO_Pin_14|GPIO_Pin_15;
  GPIO_InitStruct.GPIO_Mode = GPIO_Mode_AF;
  GPIO_InitStruct.GPIO_Speed = GPIO_Speed_25MHz;
  GPIO_InitStruct.GPIO_OType = GPIO_OType_PP;
  GPIO_InitStruct.GPIO_PuPd = GPIO_PuPd_DOWN;
  GPIO_Init ( GPIOB, &GPIO_InitStruct );

  GPIO_PinAFConfig ( GPIOB, GPIO_PinSource13, GPIO_AF_SPI2 );	// SLK
  GPIO_PinAFConfig ( GPIOB, GPIO_PinSource14, GPIO_AF_SPI2 );	// MISO
  GPIO_PinAFConfig ( GPIOB, GPIO_PinSource15, GPIO_AF_SPI2 );	// MOSI

  // SPI configuration
  RCC_APB1PeriphClockCmd ( RCC_APB1Periph_SPI2, ENABLE );

  SPI_Cmd ( SPI_PORT, DISABLE );
  SPI_InitStructure.SPI_Direction = SPI_Direction_2Lines_FullDuplex;
  SPI_InitStructure.SPI_Mode = SPI_Mode_Master;
  SPI_InitStructure.SPI_DataSize = SPI_DataSize_16b;
  SPI_InitStructure.SPI_CPOL = SPI_CPOL_Low;
  SPI_InitStructure.SPI_CPHA = SPI_CPHA_1Edge;
  SPI_InitStructure.SPI_NSS = SPI_NSS_Soft;
  SPI_InitStructure.SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_256;
  SPI_InitStructure.SPI_FirstBit = SPI_FirstBit_MSB;
  SPI_InitStructure.SPI_CRCPolynomial = 7;
  SPI_Init ( SPI_PORT, &SPI_InitStructure );
  SPI_CalculateCRC ( SPI_PORT, DISABLE );
  SPI_SSOutputCmd ( SPI_PORT, DISABLE );
  SPI_NSSInternalSoftwareConfig ( SPI_PORT, SPI_NSSInternalSoft_Set );
  SPI_Cmd ( SPI_PORT, ENABLE );

  // CS - OUT
  GPIO_InitStruct.GPIO_Pin = TOUCH_CS_PIN;
  GPIO_InitStruct.GPIO_Mode = GPIO_Mode_OUT;
  GPIO_InitStruct.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStruct.GPIO_OType = GPIO_OType_PP;
  GPIO_InitStruct.GPIO_PuPd = GPIO_PuPd_UP;
  GPIO_Init ( TOUCH_CS_PORT, &GPIO_InitStruct );

  // T_PEN - TS_IRQ -  PC5
  GPIO_InitStruct.GPIO_Mode = GPIO_Mode_IN;
  GPIO_InitStruct.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStruct.GPIO_OType = GPIO_OType_PP;
  GPIO_InitStruct.GPIO_PuPd = GPIO_PuPd_UP;
  GPIO_InitStruct.GPIO_Pin = TOUCH_IRQ_PIN;
  GPIO_Init ( TOUCH_IRQ_PORT, &GPIO_InitStruct );
}

static void SpiDelay ( uint16_t DelayCnt )
{
	uint16_t i;
	for ( i = 0; i < DelayCnt; i ++ );
}

static uint16_t TPReadX ( void)
{
	uint16_t x;

	T_CS ( );
	x = SPI_Write ( 0x9000 );		// 10010100 - Read Y, 12bit mode
									//  001       - A2..A0 - X+
									//     0      - 12 bit
									//      1     = SER/nDFR = 1
	T_DCS ( );

	return x;
}

static uint16_t TPReadY ( void )
{
	uint16_t y;

	T_CS ( );
	y = SPI_Write ( 0xD000 );			// 11010100 - Read Y, 12bit mode
										//  101       - A2..A0 - X+
										//     0      - 12 bit
										//      1     = SER/nDFR = 1
	T_DCS ( );

	return y;
}

bool TouchReadXY ( uint16_t *px, uint16_t *py, bool isReadCorrected )
{
	bool flag;
	uint16_t x, y, xx, yy;

	flag = isTouch();
	if ( flag )
	{
		x = 4095 - TPReadX();
		y = TPReadY();
		if ( isReadCorrected && !TouchVerifyCoef() )
		{
			xx = ( x / ax ) + bx;
			yy = ( y / ay ) + by;
			if (xx > LCD_PIXEL_WIDTH)
				*px = LCD_PIXEL_WIDTH;
			else
				*px = xx;

			if (yy > LCD_PIXEL_HEIGHT)
				*py = LCD_PIXEL_HEIGHT;
			else
				*py = yy;
		}
		else
		{
			*px = x;
			*py = y;
		}
	}

	return flag;
}

void TouchReadStore ( void )
{
	bool flag;
	uint16_t x,y;
	GUI_PID_STATE TS_State;

	flag = TouchReadXY ( &x, &y, true );
	if ( flag )
	{
		TS_State.x = x;
		TS_State.y = y;
		TS_State.Pressed = TOUCH_PRESSED;
		GUI_TOUCH_StoreStateEx(&TS_State);
	} else {
		GUI_TOUCH_GetState(&TS_State);
		if (TS_State.Pressed == TOUCH_PRESSED )
		{
			TS_State.Pressed = TOUCH_UNPRESSED;
			GUI_TOUCH_StoreStateEx(&TS_State);
		}
	}
}

bool isTouch ( void )
{
    if ( GPIO_ReadInputDataBit ( TOUCH_IRQ_PORT, TOUCH_IRQ_PIN ) == RESET )
    	return true;

	return false;
}

void TouchSetCoef ( int16_t _ax, int16_t _bx, int16_t _ay, int16_t _by )
{
	ax = _ax;
	bx = _bx;
	ay = _ay;
	by = _by;
}

static bool TouchVerifyCoef(void)
{
	if ( ax == 0 || ax == 0xFFFF || bx == 0xFFFF || ay == 0 || ay == 0xFFFF || by == 0xFFFF )
		return true;

	return false;
}

void DrawTarget(int x, int y)
{
	int i;

	for (i = 1; i < 5; i++)
	{
		GUI_DrawCircle(x,y,i*5);
	}
}

void TouchCalibrate ( void )
{
	uint16_t x, y, cnt, xL, yL;

	// Если калибровочные коэффициенты уже установлены - выход
	if ( !TouchVerifyCoef ( ) )
		return;

	// left top corner draw
	GUI_Clear();
	xL = LCD_GetXSize() / 2;
	yL = LCD_GetYSize() / 3;
	GUI_SetFont(GUI_FONT_COMIC24B_ASCII);
	GUI_DispStringHCenterAt("Calibration", xL, yL);

	for (cnt = 0; cnt < 4; cnt++)
	{
		GUI_SetColor(GUI_WHITE);
		DrawTarget(xCenter[cnt], yCenter[cnt]);
		GUI_DispStringHCenterAt("Push", xL, yL + 20);

		while (1)
		{
			while ( !isTouch ( ) );
			TouchReadXY ( &x, &y, false );
			if (x < 4090 && y < 4090)
			{
				xPos[cnt] = x;
				yPos[cnt] = y;
				break;
			}
		}

		GUI_DispStringHCenterAt("Pull", xL, yL + 20);

		while ( isTouch() );
		GUI_SetColor(GUI_BLACK);
		DrawTarget(xCenter[cnt], yCenter[cnt]);
	}

	// Расчёт коэффициентов
	axc[0] = (xPos[3] - xPos[0])/(xCenter[3] - xCenter[0]);
	bxc[0] = xCenter[0] - xPos[0]/axc[0];
	ayc[0] = (yPos[3] - yPos[0])/(yCenter[3] - yCenter[0]);
	byc[0] = yCenter[0] - yPos[0]/ayc[0];

	axc[1] = (xPos[2] - xPos[1])/(xCenter[2] - xCenter[1]);
	bxc[1] = xCenter[1] - xPos[1]/axc[1];
	ayc[1] = (yPos[2] - yPos[1])/(yCenter[2] - yCenter[1]);
	byc[1] = yCenter[1] - yPos[1]/ayc[1];

	// Сохранить коэффициенты
	TouchSetCoef ( axc[0], bxc[0], ayc[0], byc[0] );
	GUI_Clear();
	GUI_SetColor(GUI_WHITE);
	GUI_DispStringHCenterAt("Calibration Complete", xL, yL);
	GUI_Delay(1000);
	GUI_Clear();
}
