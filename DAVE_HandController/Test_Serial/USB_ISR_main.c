#include <DAVE.h>

#define ON 1
#define OFF 0

int8_t flag = OFF;
uint32_t Timer_20ms_Id;
int8_t tx_buffer[64] = { 0 };
int32_t state = OFF;
int8_t channel_a;
void CB_Timer_20ms()
	// 특정버튼을 누르면 핀 인터럽트가 실행되서 state의 값을 On 시켜주고, 그로인해 20초마다 flag 의 값을 변경시켜줌
{
	if(state == ON)
	{
		flag = !flag;
	}
}

int main(void)
{
	DAVE_STATUS_t status;
	status = DAVE_Init();           /* Initialization of DAVE APPs  */

	if(status != DAVE_STATUS_SUCCESS)
	{
		/* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
		XMC_DEBUG("DAVE APPs initialization failed\n");

		while(1U)
		{

		}
	}

	USBD_VCOM_Connect();	//USB통신을위한 가상 COM 포트 연결
	Timer_20ms_Id = SYSTIMER_CreateTimer(20000, SYSTIMER_MODE_PERIODIC, (void*) CB_Timer_20ms, NULL);	//20ms 마다
	SYSTIMER_StartTimer(Timer_20ms_Id);
		/* Placeholder for user application code. The while loop below can be replaced with user application code. */
	while(1U)
	{
		if(flag == ON)	// flag가 세워져있으면
						// tx_buffer배열에 adc 결과값을 하나씩 넣는다.
						// Serial 통신으로 tx 버퍼 값을 넣는다. LED에 불을 High로 켜주고 플래그를 내린다.
		{
			channel_a = ADC_MEASUREMENT_GetResult(&ADC_MEASUREMENT_Channel_A);
			tx_buffer[0] = channel_a;

			USBD_VCOM_SendByte(tx_buffer[0]);
			DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);
			flag = !flag;
		}
		else
			// flag가 내려져있으면 LED의 불을 끈다.
		{
			DIGITAL_IO_SetOutputLow(&dhDIGITAL_OUT_0);
		}
	}
return 1U;
}

void ISR_dhPIN_INTERRUPT_0(void)
	// 버튼을 누를때만 값이 들어갈수 있도록
{
	state = !state;
}
