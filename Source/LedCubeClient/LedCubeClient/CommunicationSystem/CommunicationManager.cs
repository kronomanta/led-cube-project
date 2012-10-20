using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace LedCubeClient.CommunicationSystem
{
    public class CommunicationManager
    {
        private readonly SerialPort comPort = new SerialPort();

        #region Manager Enums

        /// <summary>
        /// enumeration to hold our transmission types
        /// </summary>
        public enum TransmissionType
        {
            Text,
            Hex
        }

        /// <summary>
        /// enumeration to hold our message types
        /// </summary>
        public enum MessageType
        {
            Incoming,
            Outgoing,
            Normal,
            Warning,
            Error
        };

        #endregion

        #region Manager Properties

        /// <summary>
        /// Property to hold the BaudRate
        /// of our manager class
        /// </summary>
        public string BaudRate { get; private set; }

        /// <summary>
        /// property to hold the Parity
        /// of our manager class
        /// </summary>
        public string Parity { get; private set; }

        /// <summary>
        /// property to hold the StopBits
        /// of our manager class
        /// </summary>
        public string StopBits { get; private set; }

        /// <summary>
        /// property to hold the DataBits
        /// of our manager class
        /// </summary>
        public string DataBits { get; private set; }

        /// <summary>
        /// property to hold the PortName
        /// of our manager class
        /// </summary>
        public string PortName { get; private set; }

        /// <summary>
        /// property to check whether port is open
        /// </summary>
        public bool IsPortOpen
        {
            get { return comPort.IsOpen; }
        }

        /// <summary>
        /// property to hold our TransmissionType
        /// of our manager class
        /// </summary>
        public TransmissionType CurrentTransmissionType { get; set; }

        /// <summary>
        /// property to hold our feedback log
        /// value
        /// </summary>
        public static ObservableCollection<string> HistoryLog = new ObservableCollection<string>();

        /// <summary>
        /// property to hold our protocol type
        /// </summary>
        private IProtocol protocol;

        public IProtocol Protocol
        {
            get { return protocol; }
            set
            {
                if (protocol == value) return;
                protocol = value;
                protocol.ReadByte = ReadByte;
                protocol.SendByte = WriteByte;
            }
        }

        #endregion

        #region Manager Constructors

        /// <summary>
        /// Constructor to set the properties of our Manager Class
        /// </summary>
        /// <param name="baud">Desired BaudRate</param>
        /// <param name="par">Desired Parity</param>
        /// <param name="sBits">Desired StopBits</param>
        /// <param name="dBits">Desired DataBits</param>
        /// <param name="name">Desired PortName</param>
        /// <param name="protocol">Desired Protocol</param>
        public CommunicationManager(string baud, string par, string sBits, string dBits, string name, IProtocol protocol)
        {
            BaudRate = baud;
            Parity = par;
            StopBits = sBits;
            DataBits = dBits;
            PortName = name;
            Protocol = protocol;
            //now add an event handler
            comPort.DataReceived += ComPortDataReceived;
        }

        /// <summary>
        /// Comstructor to set the properties of our
        /// serial port communicator to nothing
        /// </summary>
        public CommunicationManager()
        {
            //add event handler
            comPort.DataReceived += ComPortDataReceived;
        }

        #endregion

        #region WriteDataWithProtocol
        public void WriteDataWithProtocol(byte[] data,MessageHeader header)
        {
            try
            {
                //first make sure the port is open
                //if its not open then open it
                if (!comPort.IsOpen)
                    comPort.Open();
                if (!comPort.IsOpen) throw new Exception("Failed to open the port " + PortName);
                //send the message tothe port
                Protocol.SendPacket(data,data.Length,header);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageType.Error + " | " + ex.Message);
            }
        }

        public void WriteDataWithProtocol(string msg, MessageHeader header)
        {
            try
            {
                //first make sure the port is open
                //if its not open then open it
                if (!comPort.IsOpen) 
                    comPort.Open();
                if(!comPort.IsOpen) throw new Exception("Failed to open the port " + PortName);
                //send the message tothe port
                Protocol.SendPacket(msg, header);
            }catch(Exception ex)
            {
                throw new Exception(MessageType.Error + " | " + ex.Message);
            }
        }
        #endregion

        #region Read/Write 
        private void WriteByte(byte c)
        {
            comPort.Write(new []{c},0,1);
        }

        private byte ReadByte()
        {
            return (byte)comPort.ReadByte();
        }
        #endregion

        #region WriteData
        public void WriteData(string msg)
        {
            try
            {
            switch (CurrentTransmissionType)
            {
                case TransmissionType.Text:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!comPort.IsOpen) comPort.Open();
                    //send the message tothe port
                    comPort.Write(msg);
                    //display the message
                    //HistoryLog.Add("Type: "+MessageType.Outgoing+" | "+msg);
                    break;
                case TransmissionType.Hex:
                    //convert the message to byte array
                    byte[] newMsg = HexToByte(msg);
                    //send the message to the port
                    comPort.Write(newMsg, 0, newMsg.Length);
                    break;
                default:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!comPort.IsOpen) comPort.Open();
                    //send the message to the port
                    comPort.Write(msg);
                    break;
            }
        }catch(Exception ex)
        {
            throw new Exception(MessageType.Error + " | " + ex.Message);
        }
    }
        #endregion

        #region HexToByte
        /// <summary>
        /// method to convert hex string into a byte array
        /// </summary>
        /// <param name="msg">string to convert</param>
        /// <returns>a byte array</returns>
        private static byte[] HexToByte(string msg)
        {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");
            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            var comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte
                //and add to the array
                comBuffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 16);
            //return the array
            return comBuffer;
        }
        #endregion

        #region ByteToHex
        /// <summary>
        /// method to convert a byte array into a hex string
        /// </summary>
        /// <param name="comByte">byte array to convert</param>
        /// <returns>a hex string</returns>
        private static string ByteToHex(byte[] comByte)
        {
            //create a new StringBuilder object
            var builder = new StringBuilder(comByte.Length * 3);
            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            //return the converted value
            return builder.ToString().ToUpper();
        }
        #endregion      

        #region OpenPort
        public bool OpenPort()
        {
            try
            {
                //first check if the port is already open
                //if its open then close it
                if (comPort.IsOpen) comPort.Close();

                //set the properties of our SerialPort Object
                comPort.BaudRate = int.Parse(BaudRate);    //BaudRate
                comPort.DataBits = int.Parse(DataBits);    //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);    //Parity
                comPort.PortName = PortName;   //PortName
                //now open the port
                comPort.Open();
                //display message
                HistoryLog.Add(MessageType.Normal + " | Port opened at " + DateTime.Now);
                //return true
                return true;
            }
            catch (Exception ex)
            {
                HistoryLog.Add(MessageType.Error + " | " + ex.Message);
                return false;
            }
        }
        #endregion

        #region ClosePort
        public bool ClosePort()
        {
            try
            {
                if(comPort.IsOpen) comPort.Close();
                HistoryLog.Add(MessageType.Normal + " | Port closed at " + DateTime.Now);
                return true;
            }
            catch (Exception e)
            {
                HistoryLog.Add(MessageType.Error + " | " + e.Message);
                return false;
            }
        }
        #endregion

        public static string[] GetParityValues()
        {
            return Enum.GetNames(typeof (Parity));
        }
        
        public static string[] GetStopBitValues()
        {
            return Enum.GetNames(typeof (StopBits));
        }

        public static string[] GetPortNameValues()
        {
            return SerialPort.GetPortNames();
        }

        public static string[] GetBaudRateValues()
        {
            return new[]{"300","600","1200","2400","4800","9600","14400",
             "19200","28800","36000","38400","57600","115000","115200"};

        }

        public static string[] GetDataBitValues()
        {
            return new[]{"7","8","9"};
        }

        #region ComPortDataReceived
        /// <summary>
        /// method that will be called when theres data waiting in the buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //determine the mode the user selected (binary/string)
            switch (CurrentTransmissionType)
            {
                //user chose string
                case TransmissionType.Text:
                    //display the data to the user
                    HistoryLog.Add(MessageType.Incoming + " | " + comPort.ReadExisting());
                    break;
                //user chose binary
                case TransmissionType.Hex:
                    //retrieve number of bytes in the buffer
                    int bytes = comPort.BytesToRead;
                    //create a byte array to hold the awaiting data
                    var comBuffer = new byte[bytes];
                    //read the data and store it
                    comPort.Read(comBuffer, 0, bytes);
                    //display the data to the user
                    HistoryLog.Add(MessageType.Incoming + " | " + ByteToHex(comBuffer));
                    break;
                default:
                    //display the data to the user
                    HistoryLog.Add(MessageType.Incoming + " | " + comPort.ReadExisting());
                    break;
            }
        }
        #endregion

        static int newsTaken;
        public static IEnumerable<string> GetNews()
        {
            var news = HistoryLog.Skip(newsTaken).ToArray();
            newsTaken += news.Length;
            return news;
        }
    }
}
