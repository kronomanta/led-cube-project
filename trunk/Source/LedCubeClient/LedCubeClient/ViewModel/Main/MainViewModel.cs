using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LedCubeClient.CommunicationSystem;
using LedCubeClient.ViewModel.Common;

namespace LedCubeClient.ViewModel.Main
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private CommunicationManager communicationManager = new CommunicationManager();
        private readonly IProtocol protocol = new Rfc1055();

        private ObservableCollection<string> commMsg = new ObservableCollection<string>();
        private bool isPortOpen;
        private string portName;
        private string parity;
        private string stopBit;
        private string dataBit;
        private string baudRate;
        private string statusbar;
        private bool isFrameControllerEnabled;
        private MessageType messageType;
        private ControlType controlType;
        

        #region Properties

        public bool IsControlTypeGroupBoxIsEnabled { get; set; }

        public bool IsPortOpen
        {
            get { return isPortOpen; }
            set
            {
                if (isPortOpen == value) return;
                isPortOpen = value;
                RaisePropertyChanged("IsPortOpen");
                RaisePropertyChanged("ConnectionButtonText");
            }
        }

        public string PortName
        {
            get { return portName; }
            set
            {
                if (portName == value) return;
                portName = value;
                RaisePropertyChanged("PortName");
            }
        }

        public string Parity
        {
            get { return parity; }
            set
            {
                if (parity == value) return;
                parity = value;
                RaisePropertyChanged("Parity");
            }
        }
        
        public string StopBit
        {
            get { return stopBit; }
            set
            {
                if (stopBit == value) return;
                stopBit = value;
                RaisePropertyChanged("StopBit");
            }
        }
       
        public string DataBit
        {
            get { return dataBit; }
            set
            {
                if (dataBit == value) return;
                dataBit = value;
                RaisePropertyChanged("DataBit");
            }
        }

        public string BaudRate
        {
            get { return baudRate; }
            set
            {
                if (baudRate == value) return;
                baudRate = value;
                RaisePropertyChanged("BaudRate");
            }
        }

        public string Statusbar
        {
            get { return statusbar; }
            set
            {
                if (statusbar == value) return;
                statusbar = value;
                RaisePropertyChanged("Statusbar");
            }
        }

        
        public MessageType MessageType
        {
            get { return messageType; }
            set
            {
                if (messageType == value) return;
                messageType = value;
                RaisePropertyChanged("MessageType");

                IsControlTypeGroupBoxIsEnabled = messageType == MessageType.Control;
                RaisePropertyChanged("IsControlTypeGroupBoxIsEnabled");
            }
        }

        public ControlType ControlType
        {
            get { return controlType; }
            set
            {
                if (controlType == value) return;
                controlType = value;
                RaisePropertyChanged("ControlType");
            }
        }

        public string[] PortNames{get { return CommunicationManager.GetPortNameValues(); } }

        public string[] ParityValues{get { return CommunicationManager.GetParityValues(); }}

        public string[] StopBitValues{get { return CommunicationManager.GetStopBitValues(); }}

        public string[] DataBitValues{get { return CommunicationManager.GetDataBitValues(); }}

        public string[] BaudRateValues{get { return CommunicationManager.GetBaudRateValues(); }}

        public string ConnectionButtonText
        {
            get { return IsPortOpen ? "Close port" : "Open port"; }
        }

        public bool IsFrameControllerEnabled
        {
            get { return isFrameControllerEnabled; }
            set
            {
                if (isFrameControllerEnabled == value) return;
                isFrameControllerEnabled = value;
                RaisePropertyChanged("IsFrameControllerEnabled");
            }
        }

        public ObservableCollection<string> CommunicationMessages
        {
            get
            {
                return commMsg;
            }
            set
            {
                commMsg = value;
                RaisePropertyChanged("CommunicationMessages");
            }
        }
        #endregion

       
        public ICommand OpenCloseConnCommand { get; private set; }
        
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitGui();
            Messenger.Default.Register<Message>(this, HandleMessage);
        }

        private void HandleMessage(Message msg)
        {
            CommunicationMessages.Add(msg.Text);
            if(msg.Sender == SenderType.Menu)
            {
                IsFrameControllerEnabled = msg.Status;
            }
        }

        private void InitGui()
        {
            CommunicationMessages = new ObservableCollection<string>();
            CommunicationManager.HistoryLog.CollectionChanged += HistoryChanged;

            //Binds the attribute Onclick to method
            OpenCloseConnCommand = new RelayCommand(OpenCloseConnection, () => true);

            InitFrameCommands();
        }
        
        private void HistoryChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if(Dispatcher.CurrentDispatcher.CheckAccess())
            {
                Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
                {
                    foreach (string item in args.NewItems)
                    {
                        CommunicationMessages.Add(item);
                    }
                }), DispatcherPriority.Render, null);
            }else
            {
                foreach (string item in args.NewItems)
                {
                    CommunicationMessages.Add(item);
                }
            }       
        }

        private void OpenCloseConnection()
        {
            try
            {
                if(!IsPortOpen)
                {
                    communicationManager = new CommunicationManager(BaudRate, Parity, StopBit, DataBit, PortName,protocol);
                    
                    IsPortOpen = communicationManager.OpenPort();    
                }else
                {
                    IsPortOpen = !communicationManager.ClosePort();
                }
                
            }catch(Exception e)
            {
                CommunicationMessages.Add("Error: "+e.Message);
            }
            
        }
    }
}
