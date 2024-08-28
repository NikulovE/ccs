using Microsoft.AspNet.SignalR.Client;
using Microsoft.WindowsAzure.Messaging;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CCS.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public IHubProxy HubProxy { get; set; }
#if DEBUG
        const string ServerURI = "http://api.commutecarsharing.ru";//*/ "http://localhost:59524";
        
#else
        const string ServerURI = "http://api.commutecarsharing.ru";
#endif

        public HubConnection Connection { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            try
            {
                Shared.Actions.initializeMap = MoveMapToCenter;
                Shared.Actions.showTripPointonMap = ShowTripPoint;
                Shared.Actions.SentMessage = SendSignalNewMessage;
                Shared.Actions.UpdateTrip = SendInviteToUser;

                INitUI();
                ConnectAsync();
            }
            catch { }
        }

        async void INitUI() {
            try
            {
                CoreDispatcherPriority priority = CoreDispatcherPriority.High;
                await Dispatcher.RunAsync(priority, () =>
                {

                    LoadTrips();
                    LoadHome();
                    LoadUserOffice();
                });
            }
            catch { }
            }

        async void LoadUserOffice()
        {
            try
            {
                await Shared.ViewModel.Organization.LoadOffices(true);
                SetOffice();
            }
            catch { }
        }
        private void SendSignalNewMessage(int obj)
        {
            try
            {
                HubProxy.Invoke("SendMessageToUser", obj);
            }
            catch(Exception e)
            {

            }
        }

        private void SendInviteToUser(int obj)
        {
            try
            {
                HubProxy.Invoke("UpdateTripToUser", obj);
            }
            catch
            {
            }
        }

        private async void ConnectAsync()
        {
            try
            {
                Connection = new HubConnection(ServerURI);
                //Connection.Closed += Connection_Closed;
                HubProxy = Connection.CreateHubProxy("ChatHub");
                //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
                HubProxy.On("AddedMessage", () =>
                {
                    try
                    {
                        Shared.Actions.refreshMessages();
                    }
                    catch (Exception e)
                    {

                    }
                });
                HubProxy.On("UpdateTrip", async () =>
                {
                    try
                    {
                        CoreDispatcherPriority priority = CoreDispatcherPriority.Normal;
                        await Dispatcher.RunAsync(priority, async () =>
                        {
                            await Shared.ViewModel.Trips.LoadTrips();
                            SetHome();
                            SetOffice();
                        });
                    }
                    catch (Exception e)
                    {
                    }
                });
                try
                {
                    await Connection.Start();
                    await HubProxy.Invoke("Register", Shared.Model.LocalStorage.UID);
                }
                catch (Exception e)
                {

                }
                //HubProxy.
            }
            catch (Exception e)
            {

            }

        }


        private async void LoadTrips() {
            try
            {
                await Shared.ViewModel.Trips.LoadTrips();
            }
            catch { }
        }

        private void MoveMapToCenter()
        {
            try
            {
                MainMap.Center = Shared.ModelView.UIBinding.Default.CurrentCenter;
                if (!App.isGPSInitialized)
                {
                    Shared.Actions.InitializeGPS();
                }


            }
            catch (Exception) { }
        }



        public void ShowTripPoint()
        {
            try
            {
                Shared.View.TripOffer.ShowTripPointsOnMap(MainMap);
            }
            catch { }
        }

        private async void LoadHome()
        {
            try
            {
                await Shared.ViewModel.UserHome.LoadHome();

                if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame = new Frame();
                    rootFrame.Navigate(typeof(SetHome));
                    Window.Current.Content = rootFrame;
                    Window.Current.Activate();
                    return;
                }
                else
                {
                    SetHome();

                }
            }
            catch { }
        }

       

        private void ShowProfileSettings(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(EditProfile));
        }



        private void ShowMessagesGrid(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Messages));
        }

        private void ShowTripCarGrid(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CheckTrips));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.BackStack.Clear();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                Shared.Actions.initializeMap();
            }
            catch { }
            
            

        }

        private void SearchCompanions(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(FindCompanions));
        }

        private void UpdateSchedule(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Schedule));
        }

        void SetHome() {
            try
            {
                var CombinedSymbols = new Grid();
                CombinedSymbols.Margin = new Thickness(-13, -13.5, 0, 0);
                CombinedSymbols.Width = 25;
                CombinedSymbols.Height = 25;
                var HouseSymbol = new TextBlock();


                HouseSymbol.FontFamily = new FontFamily("Segoe MDL2 Assets");
                HouseSymbol.FontWeight = FontWeights.ExtraBold;

                // CCS.Classic; component / Resources /#Segoe UI Symbol
                HouseSymbol.Foreground = new SolidColorBrush(Colors.White);
                HouseSymbol.HorizontalAlignment = HorizontalAlignment.Center;
                HouseSymbol.VerticalAlignment = VerticalAlignment.Center;

                var El = new Ellipse();
                El.Stroke = new SolidColorBrush(Colors.White);
                El.Fill = new SolidColorBrush(Colors.LimeGreen);
                HouseSymbol.Text = "\uEA8A";

                CombinedSymbols.Children.Add(El);
                CombinedSymbols.Children.Add(HouseSymbol);


                var Tooltip = new ToolTip();
                Tooltip.Content = "Home";
                MapControl.SetLocation(CombinedSymbols, new Geopoint(new BasicGeoposition { Latitude = Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Longitude = Shared.ModelView.UIBinding.Default.HomeLocation.Longitude }));
                MainMap.Children.Add(CombinedSymbols);
            }
            catch { }
        }

        void SetOffice() {
            try
            {
                var Office = new StackPanel();
                //if(set)Office.Margin = new Thickness(-10, -75, 0, 0);
                //else Office.Margin = new Thickness(-12, -19, 0, 0);
                Office.Orientation = Orientation.Horizontal;

                var CombinedSymbols = new Grid();
                CombinedSymbols.Width = 23;
                CombinedSymbols.Height = 23;
                var OfficeSymbol = new TextBlock();

                OfficeSymbol.Text = "\uEB4E";
                OfficeSymbol.FontFamily = new FontFamily("Segoe MDL2 Assets");
                OfficeSymbol.Foreground = new SolidColorBrush(Colors.White);
                OfficeSymbol.HorizontalAlignment = HorizontalAlignment.Center;
                OfficeSymbol.VerticalAlignment = VerticalAlignment.Center;

                var El = new Ellipse();
                El.Stroke = new SolidColorBrush(Colors.White);
                El.Fill = new SolidColorBrush(Colors.Orange);

                CombinedSymbols.Children.Add(El);
                CombinedSymbols.Children.Add(OfficeSymbol);



                Office.Children.Add(CombinedSymbols);

                var officeName = new TextBlock();
                //officeName.DataContext = ModelView.UserOrganizations.Default;
                officeName.FontSize = 16;
                officeName.FontWeight = FontWeights.ExtraBold;
                officeName.Margin = new Thickness(3, 0, 0, 0);
                officeName.Text = Shared.ModelView.UserOrganizations.Default.CurrentOffice.Name;


                officeName.VerticalAlignment = VerticalAlignment.Center;
                Office.Children.Add(officeName);
                MapControl.SetLocation(Office, new Geopoint(new BasicGeoposition { Latitude = Shared.ModelView.UserOrganizations.Default.CurrentOffice.Latitude, Longitude = Shared.ModelView.UserOrganizations.Default.CurrentOffice.Longtitude }));
                MainMap.Children.Add(Office);
                Office.Visibility = Visibility.Visible;
            }
            catch { }
        }

    }
}
