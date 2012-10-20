namespace LedCubeClient.CommunicationSystem
{
    public class MessageHeader
    {
        public MessageType Type { get; set; }

        //uses only its lower 6 bits
        public byte Misc { get; set; }

        /// <summary>
        /// Used for sending real datas
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataLength"></param>
        public MessageHeader(MessageType type, int dataLength)
        {
            Type = type;
            Misc = (byte) (dataLength - 1);
        }

        /// <summary>
        /// Contains header information for the protocol 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cType">can be null if MessageType is not a control</param>
        public MessageHeader(MessageType type, ControlType cType)
        {
            Type = type;
            if(type == MessageType.Control)
            {
                Misc = (byte) cType;
            }
        }

        public byte ToByte()
        {
            return (byte) (Type + Misc);
        }
    }

    public enum MessageType
    {
        ToRealTime = 0x00,
        ToMemory = 0x40,
        Control = 0x80,
        Reserved = 0xC0
    };

    public enum ControlType
    {
        Eeprom = 0,
        Pc = 1
    };
}