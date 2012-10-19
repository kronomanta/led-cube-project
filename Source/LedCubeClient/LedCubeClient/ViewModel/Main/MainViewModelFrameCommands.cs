using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using LedCubeClient.ViewModel.Common;

namespace LedCubeClient.ViewModel.Main
{
    public partial class MainViewModel
    {

        private Thread animationThread;
        private string currentAllFrame;

        public string CurrentAllFrame
        {
            get { return currentAllFrame; }
            set
            {
                if (currentAllFrame == value) return;
                currentAllFrame = value;
                RaisePropertyChanged("CurrentAllFrame");
            }
        }

        
        public ICommand FrameFirstCommand { get; set; }
        public ICommand FramePrevCommand { get; set; }
        public ICommand FrameNextCommand { get; set; }
        public ICommand FrameLastCommand { get; set; }
        public ICommand FramePlayCommand { get; set; }
        public ICommand FrameStopCommand { get; set; }
        public ICommand FrameRepeatCommand { get; set; }

        public ICommand SendFrameCommand { get; set; }

        public void InitFrameCommands()
        {
            FrameFirstCommand = new RelayCommand(() => GlobalClasses.AnimationHandler.FristFrame());
            FramePrevCommand = new RelayCommand(() => GlobalClasses.AnimationHandler.PrevFrame());
            FrameNextCommand = new RelayCommand(() => GlobalClasses.AnimationHandler.NextFrame());
            FrameLastCommand = new RelayCommand(() => GlobalClasses.AnimationHandler.LastFrame());
            FramePlayCommand = new RelayCommand(SendAnimation);
            FrameStopCommand = new RelayCommand(StopAnimation);
            FrameRepeatCommand = new RelayCommand(() => { GlobalClasses.AnimationHandler.Repeat = !GlobalClasses.AnimationHandler.Repeat; });
            SendFrameCommand = new RelayCommand(SendFrame,()=>true);
            GlobalClasses.AnimationHandler.CurrentFrameChanged += frame => CurrentAllFrame = (frame + 1) + "/" + GlobalClasses.AnimationHandler.AllFrame;
        }

        private void SendFrame()
        {
            foreach (int frame in GlobalClasses.AnimationHandler.GetCurrentFrame())
            {
                //communicationManager.WriteData(frame.ToString(CultureInfo.InvariantCulture));
                communicationManager.WriteDataWithProtocol(frame.ToString(CultureInfo.InvariantCulture));
            }
        }


        
        private void StopAnimation()
        {
            GlobalClasses.AnimationHandler.Stop();
            CommunicationMessages.Add("System | Animation stopped");
        }

        private void SendAnimation()
        {
            GlobalClasses.AnimationHandler.Play();
            animationThread = new Thread(SendAnimationBg);
            animationThread.Start();
            CommunicationMessages.Add("System | Sending animation...");
        }

        private void SendAnimationBg()
        {
            try 
	        {	        
		        while (GlobalClasses.AnimationHandler.IsRunning){
                    SendFrame();
                    Thread.Sleep(17);
                    GlobalClasses.AnimationHandler.NextFrame();
                }
	        }
            catch (Exception ex)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    CommunicationMessages.Add("Error: " + ex.Message);
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                        (Action)(() => CommunicationMessages.Add("Error: " + ex.Message)),
                        DispatcherPriority.Normal, null);
                }
            }
        }
    }
}
