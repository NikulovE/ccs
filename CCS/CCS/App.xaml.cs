using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Plugin.Geolocator;
using System.Globalization;
using Microsoft.AspNet.SignalR.Client;
using Shared.Model;

namespace CCS
{


    public partial class App : Application
	{
        

        public App ()
		{
			InitializeComponent();
		}

        public static bool isProfileLoaded = false;

        protected override void OnStart ()
		{
            switch (Shared.Model.LocalStorage.ProfileVersion)
            {
                case 0:
                    ShowRegistrationGrid();
                    break;
                case 1:
                    ShowConfirmationGrid();
                    break;
                case 2:
                    ShowFillingProfileGrid();
                    break;
                case 3:
                    ShowSelectDriverModeGrid();
                    break;
                default:                    
                    LaunchControl();
                    MainPage = new CCS.MainPage();
                    break;
            }
        }

        async void LaunchControl()
        {

            if (await Shared.ViewModel.Log.On())
            {
                InitMap();                
                LoadOrganizations();
                Shared.Actions.loadHome();

            }
            else RestoreAccess();
            //ConnectAsync();

           
        }

        private void SendSignalNewMessage(int obj)
        {
            try
            {
                //HubProxy.Invoke("SendMessageToUser", obj);
            }
            catch { }
        }

        private void SendInviteToUser(int obj)
        {
            try
            {
               // HubProxy.Invoke("SendInviteToUser", obj);
            }
            catch { }
        }

//        public IHubProxy HubProxy { get; set; }
//#if DEBUG
//        const string ServerURI = "http://localhost:59524";
//#else
//        const string ServerURI = "http://api.commutecarsharing.ru";
//#endif

//        public HubConnection Connection { get; set; }

//        private async void ConnectAsync()
//        {
//            try
//            {
//                Connection = new HubConnection(ServerURI);
//                //Connection.Closed += Connection_Closed;
//                HubProxy = Connection.CreateHubProxy("ChatHub");
//                //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
//                HubProxy.On("AddedMessage", async () =>
//                {
//                    try
//                    {
//                        await Shared.ViewModel.Messages.Load();
//                    }
//                    catch { }
//                });
//                HubProxy.On("AddedTrip", async () =>
//                {
//                    try
//                    {
//                        await Shared.ViewModel.Trips.LoadTrips();
//                    }
//                    catch { }
//                });
//                try
//                {
//                    try
//                    {

//                        await Connection.Start();
//                    }
//                    catch (Exception e) {
//                    }
//                    await HubProxy.Invoke("Register", LocalStorage.UID);
//                    try
//                    {
//                        Shared.Actions.SentMessage = SendSignalNewMessage;
//                        Shared.Actions.SentTrip = SendInviteToUser;
//                    }
//                    catch { }
//                }
//                catch(Exception e)
//                {

//                }
//            }
//            catch {
//            }

//        }


        private async void LoadOrganizations() {
            //await Shared.ViewModel.Organization.LoadOrganizations();

        }
        private async void InitMap()
        {
            await Shared.ViewModel.UserLocation.LoadLocation();
            Shared.Actions.initializeMap();
            try
            {
                var locator = CrossGeolocator.Current;
                await locator.StartListeningAsync(new TimeSpan(0, 0, 10), 100);
                var position = await locator.GetPositionAsync();
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(position.Latitude, position.Longitude);
                await Shared.ViewModel.UserLocation.UpdateLocation();
                await locator.StopListeningAsync();
                Shared.Actions.initializeMap();
            }
            catch (Exception) {
            }
            
        }




        async void RestoreAccess()
        {
            if (!await Shared.ViewModel.Registration.RestoreAccess())
            {
                ShowRegistrationGrid();
            }
            else
            {
                LaunchControl();
            }
        }

        private void ShowSelectDriverModeGrid()
        {
            MainPage = new CCS.RoleSelector();
        }

        private void ShowFillingProfileGrid()
        {
            MainPage = new CCS.FillingProfile();
        }

        private void ShowConfirmationGrid()
        {
            MainPage = new CCS.Confirmation();
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        void ShowRegistrationGrid()
        {
            MainPage = new CCS.Registration();

        }



    }

    public class ConfirmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "√";
            else return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "?";
            else return "√";
        }
    }
    public class ToHomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "to home";
            else return "to work";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "to work";
            else return "to home";
        }
    }


    

    public class FalseVisibile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value) return true;
            else return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return false;
            else return true;
        }
    }

    public class TrueVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return true;
            else return false;
        }
    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var culturex = CultureInfo.CurrentCulture;
            var s = new DateTime((long)value);
            return s.ToString(culturex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            try
            {
                var code = (int)value;
                switch (code)
                {
                    default:
                        return Shared.ConvertMessages.Message("x51000");
                    case 0:
                        return Shared.ConvertMessages.Message("x51000");
                    case 1:
                        return Shared.ConvertMessages.Message("x51001");
                    case 2:
                        return Shared.ConvertMessages.Message("x51002");
                    case 3:
                        return Shared.ConvertMessages.Message("x51003");
                    case 4:
                        return Shared.ConvertMessages.Message("x51004");
                    case 5:
                        return Shared.ConvertMessages.Message("x51005");
                    case 6:
                        return Shared.ConvertMessages.Message("x51006");
                    case 7:
                        return Shared.ConvertMessages.Message("x51007");
                    case 8:
                        return Shared.ConvertMessages.Message("x51008");
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
