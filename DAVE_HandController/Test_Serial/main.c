//	Define Header Files		-헤더파일 선언부
#include <DAVE.h>
#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <stdbool.h>

//	Defines		-선언부

//	Global Variables		-전역변수 선언하겠다.

//	Delay		-Delay  관련 전역변수
volatile uint32_t i = 0;

//	Transmit		-TX쪽 전역변수
//const unsigned char ucArrayMenu[] =
//		"\n"   "1 ... LEDs ON\n"
//		"2 ... LEDs OFF\n"
//		"3 ... LEDs Toggle\n"
//		"4 ... LEDs Blinking\n"   "\n"
//		"your choice: ";

//char mb[200];	//MessageBuffer for sprintf()	-sprintf 를 위한 메세지버퍼
unsigned int iMainMenuLoopCounter = 0;	//-메인메뉴 루프가 몇번돌았는지 카운트하는 변수

//	Receive			-RX 쪽 전역변수
uint16_t bytesReceived = 0;
uint8_t ucUartReceivedData = '\0';
int a = 1;
int b = 0;

// ***** Function Prototypes: ***** 	-함수 정의 해놓는 부분

// Delay:		-딜레이 관련 함수 정의
static void delay(uint32_t cycles);	//싸이클을 입력변수로 받는 딜레이함수 정의

// Transmit: 	-TX관련 함수 정의

void main(void)
{
	DAVE_Init();  //데이브 앱 초기화 하는 함수
	USBD_VCOM_Connect(); 		//USB앱 관련 함수인데 가상 COM 연결하는 함수인듯

	while (!USBD_VCOM_IsEnumDone());  	//가상COM포트가 연결안되어있으면 반복하는듯
	while(!cdc_event_flags.line_encoding_event_flag); 	//??이벤트플래그세운다고?

	delay(0xffff);		//입력변수를 왜 이따구로넣는거야!

	iMainMenuLoopCounter++;

	while(1)
	{
////////////////////host로부터 값을 받아서 XMC로 나타내는 코드
//		bytesReceived = USBD_VCOM_BytesReceived();	//USB host로부터 받은 데이터의 바이트의 수를 변수에 저장함
//
//		if (bytesReceived == 1)	//받은 데이터의 바이트수가 1이라면
//		{
//			USBD_VCOM_ReceiveByte(&ucUartReceivedData);	//미리 선언해둔 변수에 host로 부터 값을 받는다.
//
//			switch (ucUartReceivedData)
//			{
//			case '1': DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);
//			break;
//			case '2': DIGITAL_IO_SetOutputLow(&dhDIGITAL_OUT_0);
//			break;
//			default :  ;
//			break;
//			}
//
//			iMainMenuLoopCounter++;
//		}
////////////////////host로부터 값을 받아서 XMC로 나타내는 코드

		bytesReceived = USBD_VCOM_BytesReceived();	//USB host로부터 받은 데이터의 바이트의 수를 변수에 저장함
		if (bytesReceived == 1)	//받은 데이터의 바이트수가 1이라면
		{
			USBD_VCOM_SendByte(ucUartReceivedData);
//			if(DIGITAL_IO_GetInput(&dhDIGITAL_IN_0) == 0)
//			{
//				USBD_VCOM_SendByte(a);
//				DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);
//			}
//			else
//			{
//				USBD_VCOM_SendByte(b);
//				DIGITAL_IO_SetOutputLow(&dhDIGITAL_OUT_0);
//			}
			CDC_Device_USBTask(&USBD_VCOM_cdc_interface);
		}
	}
}

static void delay(uint32_t cycles)
{
	for(i = 0; i < cycles; ++i)
	{
		__NOP();
	}
}
























