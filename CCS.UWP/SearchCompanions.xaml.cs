using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CCS.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindCompanions : Page
    {
        public FindCompanions()
        {
            this.InitializeComponent();
            Shared.Actions.initializeMap = MoveMapToCenter;
            MoveMapToCenter();
        }

        private void MoveMapToCenter()
        {
            try
            {
                MainMap.Center = Shared.ModelView.UIBinding.Default.CurrentCenter;
            }
            catch (Exception) { }
        }

        private void SearchCompanionsToHome(object sender, RoutedEventArgs e)
        {

        }

        private void SearchCompanionsToWork(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void SearchToHome(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = true;
            Shared.ViewModel.Companions.FindCompanions(MainMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }

        private void SearchToWork(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = false;
            Shared.ViewModel.Companions.FindCompanions(MainMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }
    }
}
