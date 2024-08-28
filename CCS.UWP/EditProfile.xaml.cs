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
    public sealed partial class EditProfile : Page
    {
        bool textchanged = false;
        bool profileloaded = false;
        public EditProfile()
        {
            this.InitializeComponent();
            ProfileGrid.DataContext = Shared.ModelView.UserProfile.Default;
            LoadProfile();
        }

        async void LoadProfile() {
            await Shared.ViewModel.UserProfile.Load();
            profileloaded = true;
        }

        private void FieldChanged(object sender, TextChangedEventArgs e)
        {            
            if (textchanged) Shared.ModelView.UIBinding.Default.isProfileChanged = true;
        }

        private async void ChangeDriverMode(object sender, RoutedEventArgs e)
        {
           if(profileloaded) await Shared.ViewModel.UserProfile.ChangeDriverMode();
        }


        private void FieldChanged(object sender, SelectionChangedEventArgs e)
        {
            if(textchanged) Shared.ModelView.UIBinding.Default.isProfileChanged = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isProfileChanged = false;
            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
        }
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if(Shared.ModelView.UIBinding.Default.isProfileChanged) await Shared.ViewModel.UserProfile.Update();
        }

        private void OpenOrganizations(object sender, RoutedEventArgs e)
        {
          

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Organization));
        }

        //private void UpdateSchedule(object sender, RoutedEventArgs e)
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;
        //    rootFrame.Navigate(typeof(Schedule));
        //}

        private void SetHome(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(SetHome));
        }

        private async void CheckMyCar(object sender, RoutedEventArgs e)
        {
            await Shared.ViewModel.Car.LoadCar();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(EditCar));
        }



        private void CheckThatProfileEdited(object sender, KeyRoutedEventArgs e)
        {
            textchanged = true;
        }

        private void BuildRoutes(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BuildRoutes));
        }
    }
}
