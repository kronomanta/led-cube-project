using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace LedCubeClient.ViewModel
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

        private void LoadAnimation()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = ".lca",
                    Filter = "LedCube Animation (.lca)|*.lca"
                };

                bool? result = openFileDialog.ShowDialog();
                if (result == true)
                {
                    animationHandler.LoadFrames(openFileDialog.FileName);
                    FrameControllerEnable = true;
                    Statusbar = "Animation is loaded";
                }
            }
            catch (Exception e)
            {
                FrameControllerEnable = false;
                CommunicationMessages.Add("Error: " + e.Message);
            }
        }
        
        public ICommand LoadAnimationCommand { get; private set; }
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
            LoadAnimationCommand = new RelayCommand(LoadAnimation, () => true);
            FrameFirstCommand = new RelayCommand(() => animationHandler.FristFrame());
            FramePrevCommand = new RelayCommand(() => animationHandler.PrevFrame());
            FrameNextCommand = new RelayCommand(() => animationHandler.NextFrame());
            FrameLastCommand = new RelayCommand(() => animationHandler.LastFrame());
            FramePlayCommand = new RelayCommand(SendAnimation);
            FrameStopCommand = new RelayCommand(StopAnimation);
            FrameRepeatCommand = new RelayCommand(() => { animationHandler.Repeat = !animationHandler.Repeat; });
            SendFrameCommand = new RelayCommand(SendFrame,()=>true);
            animationHandler.CurrentFrameChanged += frame => CurrentAllFrame = (frame + 1) + "/" + animationHandler.AllFrame;
        }

        private void SendFrame()
        {
            try
            {
                foreach (int frame in animationHandler.GetCurrentFrame())
                {
                    communicationManager.WriteData(frame.ToString(CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                if(Application.Current.Dispatcher.CheckAccess())
                {
                    CommunicationMessages.Add("Error: " + ex.Message);
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                        (Action) (() => CommunicationMessages.Add("Error: " + ex.Message)), 
                        DispatcherPriority.Normal,null);
                }
            }
        }


        
        private void StopAnimation()
        {
            animationHandler.Stop();
            animationThread.Abort();
            Statusbar = "Animation stopped";
        }
        private void SendAnimation()
        {
            animationHandler.Play();
            animationThread = new Thread(SendAnimationBg);
            animationThread.Start();
            Statusbar = "Sending animation...";
        }

        private void SendAnimationBg()
        {
            while(animationHandler.IsRunning)
            {
                SendFrame();
                Thread.Sleep(17); 
                animationHandler.NextFrame();
            }

        }
    }
}
