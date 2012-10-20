namespace LedCubeClient.CommunicationSystem
{
    public delegate void SendByteDel(byte c);
    public delegate byte ReadByteDel();

    public abstract class IProtocol
    {
        public SendByteDel SendByte;
        public ReadByteDel ReadByte;
        public virtual void SendPacket(string p, MessageHeader header){}
        public virtual void SendPacket(byte[] p, int len, MessageHeader header){}
    }
}