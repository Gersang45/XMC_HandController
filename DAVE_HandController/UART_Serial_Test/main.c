#include <DAVE.h>

//1.4 Rx  -> Orange
//1.5 Tx  -> brown

#define ON 1
#define OFF 0
#define SIGNAL_CHECK 0

int8_t flag = OFF;
uint32_t Timer_20ms_Id;
uint8_t Tx_Data[] = { SIGNAL_CHECK,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18 };
//uint8_t Rx_Data[] = { 0};
//uint8_t state = OFF;

void CB_Timer_20ms()
{
	flag = !flag;
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
			//Is Error
		}
	}

	Timer_20ms_Id = SYSTIMER_CreateTimer(20000, SYSTIMER_MODE_PERIODIC, (void*) CB_Timer_20ms, NULL);	//creat 20ms timmer
	SYSTIMER_StartTimer(Timer_20ms_Id);	//20ms start Timer!
		/* Placeholder for user application code. The while loop below can be replaced with user application code. */
	while(1U)
	{
		if(flag == ON)
		{
			if(UART_IsTxBusy(&dhUART_0) != true)	//Tx
			{
				//DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);	//Check run LED
				UART_Transmit(&dhUART_0, Tx_Data, sizeof(Tx_Data));	//Tx_Data's data send
			}
			flag = !flag;
		}
	}
return 1U;
}
