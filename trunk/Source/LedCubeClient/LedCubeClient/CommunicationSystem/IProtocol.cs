namespace LedCubeClient.CommunicationSystem
{
    public delegate void SendCharDel(char c);
    public delegate char ReadCharDel();

    public abstract class IProtocol
    {
        public SendCharDel SendChar;
        public ReadCharDel ReadChar;
        public virtual void SendPacket(string p){}
        public virtual void SendPacket(char[] p, int len){}
    }
}