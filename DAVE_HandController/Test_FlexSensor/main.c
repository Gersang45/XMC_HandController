#include <DAVE.h>                 //Declarations from DAVE Code Generation (includes SFR declaration)


#define TICKS_PER_SECOND (1000U)

uint32_t g_var;

uint32_t Timer_1ms_Id;

void CB_Timer_1ms(void)
{
    static uint32_t ticks = 0;

    ticks++;

    if (ticks == TICKS_PER_SECOND)
    {
         ticks = 0;

         g_var = ADC_MEASUREMENT_GetResult(&ADC_MEASUREMENT_Channel_A);

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

    g_var = 0;

    Timer_1ms_Id = SYSTIMER_CreateTimer(1000, SYSTIMER_MODE_PERIODIC, (void*) CB_Timer_1ms, NULL);

    SYSTIMER_StartTimer(Timer_1ms_Id);

    /* Placeholder for user application code. The while loop below can be replaced with user application code. */
    while(1U)
    {

    }
}
