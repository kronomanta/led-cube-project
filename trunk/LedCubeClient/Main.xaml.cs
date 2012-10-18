using System;
using System.Windows.Threading;

namespace LedCubeClient
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main
    {
        public Main()
        {
            InitializeComponent();
            Dispatcher.BeginInvoke((Action)WindowLoaded, DispatcherPriority.Loaded, null);
        }

        private static void WindowLoaded()
        {
            //Dummy, just to begin background working
        }
    }
}
