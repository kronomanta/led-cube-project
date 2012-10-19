using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LedCubeClient.CommunicationSystem;

namespace LedCubeClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private readonly AnimationHandler animationHandler = new AnimationHandler();
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
        private bool frameControllerEnable;

        #region Properties
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

        public string[] PortNames{get { return CommunicationManager.GetPortNameValues(); } }

        public string[] ParityValues{get { return CommunicationManager.GetParityValues(); }}

        public string[] StopBitValues{get { return CommunicationManager.GetStopBitValues(); }}

        public string[] DataBitValues{get { return CommunicationManager.GetDataBitValues(); }}

        public string[] BaudRateValues{get { return CommunicationManager.GetBaudRateValues(); }}

        public string ConnectionButtonText
        {
            get { return IsPortOpen ? "Close port" : "Open port"; }
        }

        public bool FrameControllerEnable
        {
            get { return frameControllerEnable; }
            set
            {
                if (frameControllerEnable == value) return;
                frameControllerEnable = value;
                RaisePropertyChanged("FrameControllerEnable");
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
        }

        private void InitGui()
        {
            CommunicationMessages = new ObservableCollection<string>();
            CommunicationManager.HistoryLog.CollectionChanged += HistoryLogCollectionChanged;

            //Binds the attribute Onclick to method
            OpenCloseConnCommand = new RelayCommand(OpenCloseConnection, () => true);

            InitFrameCommands();
        }


        void HistoryLogCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (string item in e.NewItems)
            {
                CommunicationMessages.Add(item);
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
