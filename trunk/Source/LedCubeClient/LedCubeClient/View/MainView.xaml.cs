using System;
using System.Windows.Threading;

namespace LedCubeClient.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent(); 
            //Dispatcher.BeginInvoke((Action)WindowLoaded, DispatcherPriority.Loaded, null);
        }

        private static void WindowLoaded()
        {
            //Dummy, just to begin background working
        }
    }
}
