#include <DAVE.h>

#define ON 1
#define OFF 0

int8_t flag = OFF;
uint32_t Timer_20ms_Id;
int8_t tx_buffer[64] = { 0 };
int32_t state = OFF;
int8_t channel_a;
void CB_Timer_20ms()
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
	Timer_20ms_Id = SYSTIMER_CreateTimer(20000, SYSTIMER_MODE_PERIODIC, (void*) CB_Timer_20ms, NULL);
	SYSTIMER_StartTimer(Timer_20ms_Id);
		/* Placeholder for user application code. The while loop below can be replaced with user application code. */
	while(1U)
	{
		if(flag == ON)
		{
			channel_a = ADC_MEASUREMENT_GetResult(&ADC_MEASUREMENT_Channel_A);
			tx_buffer[0] = channel_a;

			USBD_VCOM_SendByte(tx_buffer[0]);
			DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);
			flag = !flag;
		}
		else
		{
			DIGITAL_IO_SetOutputLow(&dhDIGITAL_OUT_0);
		}
	}
return 1U;
}

void ISR_dhPIN_INTERRUPT_0(void)
{
	state = !state;
}
