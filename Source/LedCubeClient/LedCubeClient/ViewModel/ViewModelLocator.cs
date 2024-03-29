﻿using LedCubeClient.ViewModel.Main;
using LedCubeClient.ViewModel.Menu;

namespace LedCubeClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static MainViewModel mainViewModel;
        private static MenuViewModel menuViewModel;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            mainViewModel = new MainViewModel();
            menuViewModel = new MenuViewModel();
        }

        /// <summary>
        /// Gets the mainViewModel property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MenuViewModel MenuViewModel
        {
            get { return menuViewModel; }
        }
    }
}
