using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CCS.Classic;
using Shared;
namespace CCS.Classic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#if DEBUG
        //const string ServerURI = "http://localhost:59524";
        const string ServerURI = "http://api.commutecarsharing.ru/";
#else
        const string ServerURI = "http://api.commutecarsharing.ru/";
#endif

        public static int Counter = 0;

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int i = int.Parse(s.Tag.ToString());
            string texts = s.Uid;
            await Shared.ViewModel.Messages.Send(i, texts);

            //Shared.Actions.SentMessage(i);
            //HubProxy.Invoke("Send", UserName, TextBoxMessage.Text);
        }

        private async void AcceptOffer(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int i = int.Parse(s.Tag.ToString());
            await Shared.ViewModel.Trips.AcceptTrip(i);
        }

        private async void RejectOffer(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int i = int.Parse(s.Tag.ToString());
            await Shared.ViewModel.Trips.RejectTrip(i);
        }

        private async void SelectOffice(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ListViewItem office = sender as ListViewItem;
                Shared.ModelView.UserProfile.Default.OfficeID = int.Parse(office.Tag.ToString());
                await Shared.ViewModel.UserProfile.Update();
            }
            catch (Exception) { }
        }

        private void SelectPath(object sender, RoutedEventArgs e)
        {         
            try
            {
                Shared.ModelView.UIBinding.Default.SelectedPath = PathID(sender);
            }
            catch { }
        }

        private void CheckLast(object sender, EventArgs e)
        {
            ToggleButton a = sender as ToggleButton;
            a.IsChecked = true;
        }

        private async void SwithRoutePathWayToHome(object sender, RoutedEventArgs e)
        {
            int pathID = (PathID(sender));
            await Shared.ViewModel.RoutePoints.ChangePath(pathID, 10);
            Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == pathID).IsToHome = true;
        }

        private int PathID(object sender) {
            ToggleButton a = sender as ToggleButton;
            var PathID = int.Parse(a.Tag.ToString());
            return PathID;
        }

        private int UID(object sender)
        {
            ToggleButton a = sender as ToggleButton;
            var UID = int.Parse(a.Uid.ToString());
            return UID;
        }

        private async void SwithRoutePathWayToWork(object sender, RoutedEventArgs e)
        {
            int pathID = (PathID(sender));
            await Shared.ViewModel.RoutePoints.ChangePath(pathID, 0);
            Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == pathID).IsToHome = false;

        }

        private async void EnableActuality(object sender, RoutedEventArgs e)
        {
            var o = sender as ToggleButton;
            int pathID = (PathID(sender));
            int dayofweek = UID(sender);
            if (o.IsInitialized)
            {
                await Shared.ViewModel.RoutePoints.ChangePath(pathID, UID(sender) * -1, false);
                var selectedpath= Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == pathID);
                switch (dayofweek) {

                    case 1:
                        selectedpath.Actuality.Monday = false;
                        break;
                    case 2:
                        selectedpath.Actuality.Tuesday = false;
                        break;
                    case 3:
                        selectedpath.Actuality.Wednesday = false;
                        break;
                    case 4:
                        selectedpath.Actuality.Thursday = false;
                        break;
                    case 5:
                        selectedpath.Actuality.Friday = false;
                        break;
                    case 6:
                        selectedpath.Actuality.Saturday = false;
                        break;
                    case 7:
                        selectedpath.Actuality.Sunday = false;
                        break;

                    
                }
                
            }
        }

        private async void DisableActuality(object sender, RoutedEventArgs e)
        {
            var o = sender as ToggleButton;
            int pathID = (PathID(sender));
            int dayofweek = UID(sender);
            if (o.IsInitialized)
            {
                await Shared.ViewModel.RoutePoints.ChangePath(pathID, dayofweek, false);
                var selectedpath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == pathID);
                switch (dayofweek)
                {
                    case 1:
                        selectedpath.Actuality.Monday = true;
                        break;
                    case 2:
                        selectedpath.Actuality.Tuesday = true;
                        break;
                    case 3:
                        selectedpath.Actuality.Wednesday = true;
                        break;
                    case 4:
                        selectedpath.Actuality.Thursday = true;
                        break;
                    case 5:
                        selectedpath.Actuality.Friday = true;
                        break;
                    case 6:
                        selectedpath.Actuality.Saturday = true;
                        break;
                    case 7:
                        selectedpath.Actuality.Sunday = true;
                        break;
                }
            }
            
        }

    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class ConvertStringMessages : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            try
            {
                var code = (int)value;
                switch (code) {
                    default:
                        return ConvertMessages.Message("x51000");
                    case 0:
                        return ConvertMessages.Message("x51000");
                    case 1:
                        return ConvertMessages.Message("x51001");
                    case 2:
                        return ConvertMessages.Message("x51002");
                    case 3:
                        return ConvertMessages.Message("x51003");
                    case 4:
                        return ConvertMessages.Message("x51004");
                    case 5:
                        return ConvertMessages.Message("x51005");
                    case 6:
                        return ConvertMessages.Message("x51006");
                    case 7:
                        return ConvertMessages.Message("x51007");
                    case 8:
                        return ConvertMessages.Message("x51008");
                }
            }
            catch (Exception){
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class ConvertVisibilityMode : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Hidden;
            else return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class TrueVisibile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class FalseVisibile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if (!(bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Hidden;
            else return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(String), typeof(bool))]
    public class ConfirmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if ((bool)value) return "\uE0E7";
            else return "\uE897";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(String), typeof(bool))]
    public class ToHomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if ((bool)value) return "\uE80F";
            else return "\uE821";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(String), typeof(DateTime))]
    public class DayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if ((string)value.)
            if ((new DateTime((long)value)).Hour<12) return "\uED39";
            else return "\uEC46";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
    }

    [ValueConversion(typeof(String), typeof(DateTime))]
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

    [ValueConversion(typeof(int), typeof(bool))]
    public class SelectedOffice : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == Shared.ModelView.UserProfile.Default.OfficeID)
            {
                return true;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(DateTime))]
    public class LocalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var k = new DateTime((long)value);
            var culturex = CultureInfo.CurrentCulture;
            var s = k.ToLocalTime();
            return s.ToString(culturex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
    [ValueConversion(typeof(int), typeof(bool))]
    public class SelectPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == Shared.ModelView.UIBinding.Default.SelectedPath)
            {
                return true;
            }
            else return false;

           // return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //return false;
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class WriteRouteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Properties.Resources.WriteRoute + " " +(int)value;           

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }

    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    public class VisibilityInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var a = (value as Button).Visibility;
            if ((Visibility)value == Visibility.Visible) return Visibility.Collapsed;
            else return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
