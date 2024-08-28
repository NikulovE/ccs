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
    public sealed partial class Messages : Page
    {
        public Messages()
        {
            this.InitializeComponent();
            Shared.Actions.refreshMessages = LoadMessages;
            LoadMessages();
        }


        private async void LoadMessages()
        {
            CoreDispatcherPriority priority = CoreDispatcherPriority.Normal;
            await Dispatcher.RunAsync(priority, async() =>
            {
                await Shared.ViewModel.Messages.Load();
            });
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int i = int.Parse(s.Tag.ToString());
            string texts = (string)s.CommandParameter;
            await Shared.ViewModel.Messages.Send(i, texts);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MessageGrid.DataContext = Shared.ModelView.UIBinding.Default;
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }

            

        }

        private async void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {            
            await Shared.ViewModel.Messages.Load();
        }
    }
}
