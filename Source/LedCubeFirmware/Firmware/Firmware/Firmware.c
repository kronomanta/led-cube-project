/*
 * Firmware.c
 *
 * Created: 2012.10.26. 15:53:59
 *  Author: Tibor Kanyó
 */ 
#include "rfc1055.h"

#include "conf_clock.h"
#include "conf_uart.h"

#include <util/setbaud.h>

#include <avr/io.h>
#include <avr/interrupt.h>


int main(void)
{
	USART_init(MYUBRR);
	PORTD = END;
}