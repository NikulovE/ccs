using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
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
    public sealed partial class Confirmation : Page
    {
        private async void InitNotificationsAsync()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                var hub = new NotificationHub("CCS", "Endpoint=sb://commutecarsharing.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=3A3MrIVF46nQ3lQs80Kz6mJRvEgqQeioKiOrsrikhQw=");
                var tags = new List<String>();
                tags.Add(Shared.Model.LocalStorage.UID.ToString());
                await hub.RegisterNativeAsync(channel.Uri, tags);
            }
            catch (Exception e)
            {

            }


        }

        public Confirmation()
        {
            this.InitializeComponent();
            ConfirmationGrid.DataContext = Shared.ModelView.UIBinding.Default;
        }

        private void BackToRegistration(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Registration));
        }

        private void ConfirmRegistration(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.Confirm(ConfirmPassword, PasswordFromMail, ConfirmationGrid, ShowFilliningProfileGrid);
        }

        private void ShowFilliningProfileGrid(object sender, object e)
        {
            InitNotificationsAsync();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(FillingProfile));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.BackStack.Add(new PageStackEntry(typeof(Registration), null, null));

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
        }

    }
}
