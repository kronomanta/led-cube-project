namespace LedCubeClient.CommunicationSystem
{
    public delegate void SendCharDel(char c);
    public delegate char ReadCharDel();

    public abstract class IProtocol
    {
        public SendCharDel SendChar;
        public ReadCharDel ReadChar;
    }
}