using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LedCubeClient.ViewModel.Common;
using Microsoft.Win32;

namespace LedCubeClient.ViewModel.Menu
{
    public class MenuViewModel
    {
        public ICommand LoadAnimationCommand { get; private set; }

        public MenuViewModel()
        {
            LoadAnimationCommand = new RelayCommand(LoadAnimation, () => true);            
        }

        private static void LoadAnimation()
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
                    GlobalClasses.AnimationHandler.LoadFrames(openFileDialog.FileName);
                    Messenger.Default.Send(new Message{Status=true,Text = "System | Animation is loaded",Sender=SenderType.Menu});
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new Message { Status = false, Text = "Error | " + e.Message,Sender=SenderType.Menu});
            }
        }
        
    }
}
