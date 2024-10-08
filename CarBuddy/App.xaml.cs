﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Shared;
using Windows.Devices.Geolocation;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace CCS.WinStore
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private static bool isProfileLoaded = false;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
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
                        break;
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        async void LaunchControl()
        {

            if (await Shared.ViewModel.Log.On())
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
                InitMap();
                LoadOrganizations();
                App.isProfileLoaded = true;

            }
            else RestoreAccess();
        }

        private async void LoadOrganizations()
        {
            await Shared.ViewModel.Organization.LoadOrganizations();

        }

        private async void InitMap()
        {
            try
            {
                Geolocator geolocator = new Geolocator { ReportInterval = 300000 };
                Geoposition pos = await geolocator.GetGeopositionAsync();
                geolocator.PositionChanged += UpdatePosition;
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);
                Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);
                await Shared.ViewModel.UserLocation.UpdateLocation();
            }
            catch (Exception) {

            }
            Actions.initializeMap();
        }

        private async void UpdatePosition(Geolocator sender, PositionChangedEventArgs args)
        {
            Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(args.Position.Coordinate.Point.Position.Latitude, args.Position.Coordinate.Point.Position.Longitude);
            Shared.ModelView.UIBinding.Default.CurrentCenter = Shared.ModelView.UIBinding.Default.CurrentUserPosition;
            await Shared.ViewModel.UserLocation.UpdateLocation();
            Actions.initializeMap();

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
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(RoleSelector));
        }

        private void ShowFillingProfileGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(FillingProfile));
        }

        private void ShowConfirmationGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Confirmation));
        }

        void ShowRegistrationGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Registration));

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }




    }

    public class TrueVisibile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)

        {
            // if ((string)value.)
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();

        }
    }

    public class InvertTrueVisibile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)

        {
            // if ((string)value.)
            if ((bool)value) return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();

        }
    }
    public class ConvertStringMessages : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // if ((string)value.)
            try
            {
                var code = (int)value;
                switch (code)
                {
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
            catch (Exception)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            long ticks = (long)value;
            var k = new DateTime(ticks);
            var culturex = CultureInfo.CurrentCulture;
            var s = k.ToLocalTime();
            return s.ToString(culturex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }



}
