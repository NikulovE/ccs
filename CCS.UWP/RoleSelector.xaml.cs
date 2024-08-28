using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class RoleSelector : Page
    {
        public RoleSelector()
        {
            this.InitializeComponent();
        }

        private async void SelectedDriverModePassenger(object sender, RoutedEventArgs e)
        {
            try
            {
                Shared.ModelView.UserProfile.Default.IsDriver = false;
                if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }
            }
            catch { }
        }

        private async void SelectedDriverModeDriver(object sender, RoutedEventArgs e)
        {
            try
            {
                Shared.ModelView.UserProfile.Default.IsDriver = true;
                if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }
            }
            catch { }
        }
    }
}
