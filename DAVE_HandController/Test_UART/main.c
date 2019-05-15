///*
// * main.c
// *
// *  Created on: 2019 May 13 17:52:10
// *  Author: Lenovo
// */
//
//#include <DAVE.h>
//
////1.4 Rx  -> Orange
////1.5 Tx  -> brown
//
//#define ON 1
//#define OFF 0
//#define SIGNAL_CHECK 0
//
//int8_t flag = OFF;
//uint32_t Timer_20ms_Id;
//uint8_t Tx_Data[] = { SIGNAL_CHECK,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18 };
//uint8_t result = 1;
//uint8_t read_data[]   = {0x0};
//
//void CB_Timer_20ms()
//{
//	flag = !flag;
//}
//
//int main(void)
//{
//	DAVE_STATUS_t status;
//	status = DAVE_Init();           /* Initialization of DAVE APPs  */
//
//	if(status != DAVE_STATUS_SUCCESS)
//	{
//		/* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
//		XMC_DEBUG("DAVE APPs initialization failed\n");
//
//		while(1U)
//		{
//			//Is Error
//		}
//	}
//
//	Timer_20ms_Id = SYSTIMER_CreateTimer(20000, SYSTIMER_MODE_PERIODIC, (void*) CB_Timer_20ms, NULL);	//creat 20ms timmer
//	SYSTIMER_StartTimer(Timer_20ms_Id);	//20ms start Timer!
//	UART_Receive(&UART_0, read_data, 1);	// 입력받을때 문자형으로 입력받고, 아스키코드표를 따라서 16진수로 변환해서 저장함.
//	while(read_data[0] == 0x0);		// 값을 받을때 까지 존버
//		/* Placeholder for user application code. The while loop below can be replaced with user application code. */
//	while(1U)
//	{
//		if(flag == ON)
//		{
//			if(UART_IsTxBusy(&UART_0) != true)	//Tx가 바쁜가?
//			{
//				//DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);	// Check run LED
//				UART_Transmit(&UART_0, Tx_Data, sizeof(Tx_Data));	// Tx_Data's data send
//			}
//			flag = !flag;
//		}
//	}
//return 1U;
//}
//
//void ISR_Adc_Measurement_0()
//{
//	result = ADC_MEASUREMENT_GetResult(&ADC_MEASUREMENT_Channel_A);
//	if(result == SIGNAL_CHECK)
//	{
//		result++;
//	}
//	Tx_Data[1] = result;
//	ADC_MEASUREMENT_StartConversion(&dhADC_MEASUREMENT_0);
//}


/*
 * main.c
 *
 *  Created on: 2019 May 13 17:52:10
 *  Author: Lenovo
 */

#include <DAVE.h>

//1.4 Rx  -> Orange
//1.5 Tx  -> brown

#define ON 1
#define OFF 0
#define START_CHECK 0
#define END_CHECK 1

int8_t flag = OFF;
uint32_t Timer_20ms_Id;
uint8_t Tx_Data[] = { START_CHECK,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,END_CHECK };
uint8_t result = 1;
uint8_t read_data[]   = {0x0};

void CB_Timer_20ms()
{
	UART_Receive(&UART_0, read_data, 1);	// 입력받을때 문자형으로 입력받고, 아스키코드표를 따라서 16진수로 변환해서 저장함.
	if(read_data[0] != 0x0)		// 값을 받을때 까지 존버
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
			if(UART_IsTxBusy(&UART_0) != true)	//Tx가 바쁜가?
			{
				DIGITAL_IO_SetOutputHigh(&dhDIGITAL_OUT_0);	// Check run LED
				UART_Transmit(&UART_0, Tx_Data, sizeof(Tx_Data));	// Tx_Data's data send
				read_data[0] = 0x0;
				flag = OFF;
			}
		}
	}
return 1U;
}

void ISR_Adc_Measurement_0()
{
	result = ADC_MEASUREMENT_GetResult(&ADC_MEASUREMENT_Channel_A);
	if(result == START_CHECK)
	{
		result++;
	}
	Tx_Data[1] = result;
	ADC_MEASUREMENT_StartConversion(&dhADC_MEASUREMENT_0);
}
