/*
 * conf_uart.h
 *
 * Created: 2012.10.26. 17:05:19
 *  Author: Tibor
 */ 

#include <avr/io.h>

#ifndef CONF_UART_H_INCLUDED
#define CONF_UART_H_INCLUDED

#define FOSC 20000000
#define BAUD 9600
#define MYUBRR FOSC/16/BAUD-1

void USART_init(unsigned int ubrr){
	
	/*Set baud rate*/
	UBRR0H = (unsigned char)(ubrr>>8);
	UBRR0 = (unsigned char)ubrr;
	
	/*Enabled receiver */
	UCSR0B = 1<<RXEN0;
	
	/*Set Frame format: 8 data bits, zero stop bits*/
	UCSR0C = 3<<UCSZ00;
}

unsigned char USAR_receive(){
	/*Wait for data to be received*/
	while(!(UCSR0A & (1<<RXEN0)));
	return UDR0;
}

#endif /* CONF_UART_H_INCLUDED */