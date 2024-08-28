using System;
using System.Threading.Tasks;
using Shared.Model;

#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif
//#if ANDROID
//using Android.Opengl;
//#endif

namespace Shared.ViewModel
{
    class UserProfile
    {
        public static async Task<bool> Load()
        {

            View.General.inLoading();
            var profile = await Model.Requests.LoadProfile();
            View.General.outLoading();
            if (profile.Item1 == true)
            {

                ModelView.UserProfile.Default.FirstName = profile.Item2.FirstName;
                ModelView.UserProfile.Default.LastName = profile.Item2.LastName;
                ModelView.UserProfile.Default.OfficeID = profile.Item2.OfficeID;
                ModelView.UserProfile.Default.IsDriver = profile.Item2.IsDriver;
                try
                {
                    var AESEngine = new Shared.Crypting.AES();
                    ModelView.UserProfile.Default.Phone = AESEngine.Decrypt(profile.Item2.Phone, LocalStorage.SessionKey);
                }
                catch (Exception)
                {
                    ModelView.UserProfile.Default.Phone = "";
                }
                try
                {
                    var AESEngine = new Shared.Crypting.AES();
                    ModelView.UserProfile.Default.Extension = AESEngine.Decrypt(profile.Item2.Extension, LocalStorage.SessionKey);
                }
                catch (Exception)
                {
                    ModelView.UserProfile.Default.Extension = "";
                }
                ModelView.UserProfile.Default.Payment = profile.Item2.Payment;
                Model.LocalStorage.ProfileVersion = profile.Item2.Version;
                ModelView.UIBinding.Default.isProfileChanged = false;

                if (ModelView.UserProfile.Default.IsDriver)
                {
#if XAMARIN
                    ModelView.UIBinding.Default.DriverRoutesVisibility = true;
#else
                    ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
#endif
                    ModelView.UIBinding.Default.CarTripMode = ConvertMessages.Message("CarAndTrip");
                }
                else
                {
#if XAMARIN
                    ModelView.UIBinding.Default.DriverRoutesVisibility = false;
#else
                    ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;
#endif

                    ModelView.UIBinding.Default.CarTripMode = ConvertMessages.Message("trip");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> Update()
        {

            View.General.inLoading();
            var query = await Model.Requests.UpdateProfile(ModelView.UserProfile.Default);
            View.General.outLoading();
            if (query.Item1 == true)
            {
                ModelView.UIBinding.Default.isProfileChanged = false;
                Model.LocalStorage.ProfileVersion = int.Parse(query.Item2);
                return true;
            }
            else
            {
                ModelView.UIBinding.Default.isProfileChanged = true;
                return false;
            }
        }

        public static async Task<bool> ChangeDriverMode()
        {

            View.General.inLoading();
            var query = await Model.Requests.ChangeDriverMode();
            View.General.outLoading();
            if (query.Item1 == true)
            {
                Model.LocalStorage.ProfileVersion = int.Parse(query.Item2);
                if (ModelView.UserProfile.Default.IsDriver)
                {
                    await Shared.ViewModel.Car.LoadCar();
                    ModelView.UIBinding.Default.CarTripMode = ConvertMessages.Message("CarAndTrip");
#if XAMARIN
                    ModelView.UIBinding.Default.DriverRoutesVisibility = true;
#else
                     ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
#endif

                }
                else
                {
                    ModelView.UIBinding.Default.CarTripMode = ConvertMessages.Message("trip");
#if XAMARIN
                    ModelView.UIBinding.Default.DriverRoutesVisibility = false;
#else
                   ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;
#endif

                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
