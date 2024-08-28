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
    public sealed partial class Registration : Page
    {
        public Registration()
        {
            this.InitializeComponent();
            RegistrationGrid.DataContext = Shared.ModelView.UIBinding.Default;
            SendKey.IsEnabled = true;
            Shared.Model.LocalStorage.ProfileVersion = 0;
            Shared.Model.LocalStorage.Reset();
            Shared.ModelView.UIBinding.Default.OutPut = "";
        }


        private void CheckEmail(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.CheckEmail(InputedMail, SendKey, RegistrationGrid, ShowConfirmationGrid);
        }

        private void ShowConfirmationGrid(object sender, object e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Confirmation));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.BackStack.Clear();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
        }


    }
}
