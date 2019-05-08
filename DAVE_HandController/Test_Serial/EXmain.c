#include <DAVE.h>

#define BUTTON_1_ON 0
#define BUTTON_2_ON 2

volatile uint32_t i = 0;
 int8_t rx_buffer[64] = { 0 };
 int8_t tx_buffer[64] = { 0 };
 int state = 0;
 int load_state = 0;

 static void delay(uint32_t cycles);	//싸이클을 입력변수로 받는 딜레이함수 정의
 __STATIC_INLINE uint32_t Digital_Get_Input(const DIGITAL_IO_t *const handler1, const DIGITAL_IO_t *const handler2);


int main(void)
{
	DAVE_Init();

	USBD_VCOM_Connect();
	delay(0xffff);

	while(1U)
	{
		state = Digital_Get_Input(&dhDIGITAL_IN_0,&dhDIGITAL_IN_1);	//버튼의 입력 신호를 저장하는 변수

		switch(state)
		{
			case BUTTON_1_ON:
				tx_buffer[0] = 1;	//보낼 데이터가 저장된 배열
				USBD_VCOM_SendByte(tx_buffer[0]);	//값 전송
				DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);	//실행되고있는지 확인하기위한 LED
				break;
			case BUTTON_2_ON:
				tx_buffer[0] = 2;	//보낼 데이터가 저장된 배열
				USBD_VCOM_SendByte(tx_buffer[0]);	//값 전송
				DIGITAL_IO_SetOutputLow(&dhDIGITAL_OUT_0);	//실행되고있는지 확인하기위한 LED
				break;
			default:	;
				break;
		}
	delay(0xffff);
	//CDC_Device_USBTask(&USBD_VCOM_cdc_interface);	//뭔지모르겠음..
	}
return 1U;
}

static void delay(uint32_t cycles)
{
	for(i = 0; i < cycles; ++i)
	{
		__NOP();
	}
}

__STATIC_INLINE uint32_t Digital_Get_Input(const DIGITAL_IO_t *const handler1, const DIGITAL_IO_t *const handler2)
{
	uint32_t button_1;
	uint32_t button_2;
	XMC_ASSERT("DIGITAL_IO_GetInput: handler null pointer", handler1 != NULL);
	button_1 = XMC_GPIO_GetInput(handler1->gpio_port, handler1->gpio_pin);
	XMC_ASSERT("DIGITAL_IO_GetInput: handler null pointer", handler2 != NULL);
	button_2 = XMC_GPIO_GetInput(handler2->gpio_port, handler2->gpio_pin);

	if(button_1 == 0)
	{
		return BUTTON_1_ON;
	}
	else if(button_2 == 0)
	{
		return BUTTON_2_ON;
	}
}


