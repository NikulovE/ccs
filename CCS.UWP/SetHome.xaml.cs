using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class SetHome : Page
    {
        public SetHome()
        {
            this.InitializeComponent();
            InitMap();
            Actions.initializeMap = MoveMapToCenter;
            MoveMapToCenter();
        }

        private void InitMap()
        {
            MapControl.SetLocation(home, new Geopoint(new BasicGeoposition { Longitude = Shared.ModelView.UIBinding.Default.HomeLocation.Longitude, Latitude = Shared.ModelView.UIBinding.Default.HomeLocation.Latitude }));


        }

        private void MoveMapToCenter() {
            try
            {
                MainMap.Center = Shared.ModelView.UIBinding.Default.CurrentCenter;
            }
            catch (Exception) { }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            await Shared.ViewModel.UserHome.LoadHome();
            MapControl.SetLocation(home, new Geopoint(new BasicGeoposition { Latitude = Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Longitude = Shared.ModelView.UIBinding.Default.HomeLocation.Longitude }));
            home.Visibility = Visibility.Visible;

            //string myPages = "";
            //foreach (PageStackEntry page in rootFrame.BackStack)
            //{
            //    myPages += page.SourcePageType.ToString() + "\n";
            //}
            //stackCount.Text = myPages;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
        }


        private async void SetHomeOnMapByTouch(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            home.Visibility = Visibility.Collapsed;
            Shared.ModelView.UIBinding.Default.HomeLocation = new Location(args.Location.Position.Latitude, args.Location.Position.Longitude);

            await Shared.ViewModel.UserHome.SetHome();
            Continue.Visibility = Visibility.Visible;

            MapControl.SetLocation(home, args.Location);
            home.Visibility = Visibility.Visible;
        }

        private void BackToMainPage(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }
    }
}
