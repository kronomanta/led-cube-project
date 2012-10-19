﻿using System.Collections.Generic;

namespace LedCubeClient.CommunicationSystem
{
    public class Rfc1055: IProtocol
    {
        const char END = '\u0300';    /* indicates end of packet */
        const char ESC = '\u0333';    /* indicates byte stuffing */
        const char ESC_END = '\u0334';    /* ESC ESC_END means END data byte */
        const char ESC_ESC = '\u0335';    /* ESC ESC_ESC means ESC data byte */

        public override void SendPacket(string p)
        {
            SendPacket(p.ToCharArray(),p.Length);    
        }

        public override void SendPacket(char[] p, int len)
        {
            if (SendChar == null) return;
            /* send an initial END character to flush out any data that may
            * have accumulated in the receiver due to line noise
            */
            SendChar(END);
            /* for each byte in the packet, send the appropriate character sequence */
            for (int i = 0; i < len; i++)
            {
                switch (p[i])
                {
                    /* if it’s the same code as an END character, we send a
                    * special two character code so as not to make the
                    * receiver think we sent an END
                    */
                    case END:
                        SendChar(ESC);
                        SendChar(ESC_END);
                        break;

                    /* if it’s the same code as an ESC character,
                    * we send a special two character code so as not
                    * to make the receiver think we sent an ESC
                    */
                    case ESC:
                        SendChar(ESC);
                        SendChar(ESC_ESC);
                        break;

                    /* otherwise, we just send the character
                */
                    default:
                        SendChar(p[i]);
                        break;
                }
            }
        }


        /* RECV_PACKET: receives a packet into the buffer located at "p".
        *      If more than len bytes are received, the packet will
        *      be truncated.
        *      Returns the number of bytes stored in the buffer.
        */
        public char[] RecvPacket(int len)
        {
            if (ReadChar == null) return null;

            var p = new List<char>();
            char c;
            int received = 0;

            /* sit in a loop reading bytes until we put together
             * a whole packet.
             * Make sure not to copy them into the packet if we
             * run out of room.
             */
            while (true)
            {
                /* get a character to process*/
                c = ReadChar();

                /* handle bytestuffing if necessary*/
                switch (c)
                {
                    /* if it's an END character then we're done with the packet */
                    case END:
                        /* a minor optimization: if there is no data in the packet, ignore it. This is
                        * meant to avoid bothering IP with all the empty packets generated by the
                        * duplicate END characters which are in turn sent to try to detect line noise.
                        */
                        if (p.Count > 0) return p.ToArray();
                        break;

                    /* if it's the same code as an ESC character, wait
                    * and get another character and then figure out
                    * what to store in the packet based on that.
                    */
                    case ESC:
                        c = ReadChar();
                        /* if "c" is not one of these two, then we have a protocol violation.  
                         * The best bet seems to be to leave the byte alone and 
                         * just stuff it into the packet */
                        switch (c)
                        {
                            case ESC_END:
                                c = END;
                                break;
                            case ESC_ESC:
                                c = ESC;
                                break;
                        }
                        break;
                    /* here we fall into the default handler and let it store the character for us */
                    default:
                        if (received < len)
                            p[received++] = c;
                        break;
                }
            }
        }

    }
}
