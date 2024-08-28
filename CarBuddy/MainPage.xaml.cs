using Bing.Maps;
using CCS.WinStore.Common;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CCS.WinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        List<FrameworkElement> MainUIs = new List<FrameworkElement>();
        List<FrameworkElement> AutoCollapsedElements = new List<FrameworkElement>();
        List<FrameworkElement> ExtendOrgPanel = new List<FrameworkElement>();
        List<FrameworkElement> ExtendGroupPanel = new List<FrameworkElement>();

        List<ToggleButton> Buttons = new List<ToggleButton>();
        bool isProfileLoaded = false;

        public MainPage()
        {
            //ApplicationData.Current.RoamingSettings.Values["ProfileVersion"] = 0;

            InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;


            LoadingAnimation.DataContext = Shared.ModelView.UIBinding.Default;            
            TopMenu.DataContext = Shared.ModelView.UserProfile.Default;
            MainMap.DataContext = Shared.ModelView.UIBinding.Default;
            ProfileGrid.DataContext = Shared.ModelView.UserProfile.Default;
            homeLayer.DataContext = Shared.ModelView.UIBinding.Default;
            //ScheduleGrid.DataContext = Shared.ModelView.UIBinding.Default;
            ScheduleGrid.DataContext = Shared.ModelView.UserSchedule.Default;
            MessagesGrid.DataContext = Shared.ModelView.UIBinding.Default;
            OrganizationGrid.DataContext = Shared.ModelView.UserOrganizations.Default;

            CarInfo.DataContext = Shared.ModelView.UserCar.Default;

            //CarInfo.DataContext = ModelView.UserCar.Default;
            TripsHeader.DataContext = Shared.ModelView.UserProfile.Default;

            WriteRouteBut.DataContext = Shared.ModelView.UserProfile.Default;
            CarTripHeader.DataContext = Shared.ModelView.UIBinding.Default;
            
            WizardAssistant.DataContext = Shared.ModelView.UIBinding.Default;
            //Settings.Default.Reset();
            //Settings.Default.Save();

            MainUIs.Add(MainGrid);




            //AutoCollapsedElements.Add(SettingsGrid);
            AutoCollapsedElements.Add(ProfileGrid);
            AutoCollapsedElements.Add(GroupsGrid);
            AutoCollapsedElements.Add(OrganizationGrid);
            //AutoCollapsedElements.Add(ExtendCompanyGrid);
            AutoCollapsedElements.Add(CarTripGrid);
            AutoCollapsedElements.Add(ScheduleGrid);
            AutoCollapsedElements.Add(MessagesGrid);
            AutoCollapsedElements.Add(NewOfficeOnMap);
            AutoCollapsedElements.Add(OfficesOnMap);

            Shared.View.General.HideElements(AutoCollapsedElements);

            ExtendOrgPanel.Add(ExtendOrgdHeader);
            ExtendOrgPanel.Add(ExtendOrgCloseButton);
            ExtendOrgPanel.Add(ExtendOrgPrefferedNameGreeting);
            ExtendOrgPanel.Add(ExtendOrgPrefferedName);
            ExtendOrgPanel.Add(WorkEmailGreeting);
            ExtendOrgPanel.Add(WorkMail);
            ExtendOrgPanel.Add(SendKeyRegistration);
            ExtendOrgPanel.Add(InputeKeyFromMailGreeting);
            ExtendOrgPanel.Add(KeyFromMail);
            ExtendOrgPanel.Add(ConfirmKey);


            Shared.View.General.HideElements(ExtendOrgPanel);


            Buttons.Add(WriteRouteBut);
            Buttons.Add(FindCompanionsBut);
            Buttons.Add(SetHomeBut);
           // Buttons.Add(SetCurrentPositionBut);


            Shared.Actions.initializeMap = MoveMapToCenter;
            Shared.Actions.showTripPointonMap = ShowTripPoint;
            Shared.Actions.showTripOffers = ShowTripOffers;
            Shared.Actions.refreshOffices = RefreshOffices;
            Shared.Actions.refreshOrganizations = RefreshOrganizations;
            Shared.Actions.refreshRoutePoints = RefreshRoutePoints;
            Shared.Actions.checkWizard = CheckWizard;
            Shared.Actions.refreshUserOffice = LoadUserOffice;
            LaunchControl();

        }

        private void MoveMapToCenter()
        {
            try
            {
                MainMap.Center = Shared.ModelView.UIBinding.Default.CurrentCenter;
            }
            catch (Exception) { }
        }

        public void ShowTripPoint()
        {
            Shared.View.TripOffer.ShowTripPointsOnMap(TripPoints);
        }

        public void ShowTripOffers()
        {
            Shared.View.TripOffer.FillTrips(Trips);

        }

        public async void RefreshOffices()
        {
            await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);

        }

        public async void RefreshOrganizations()
        {
            await Shared.ViewModel.Organization.LoadOrganizations();
        }

        public void RefreshRoutePoints(int SelectedPath)
        {
            Shared.ViewModel.RoutePoints.RefreshPath(RoutesOnMap, SelectedPath);
        }



       

        void ShowConfirmationGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Confirmation));
            Window.Current.Activate();
        }

       void ShowFillingProfileGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(FillingProfile));
        }

        void ShowRegistrationGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Registration));
        }

        void ShowSelectDriverModeGrid()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(RoleSelector));
        }
        //bool needLogon = true;
        async void LaunchControl()
        {
            AppBar.Visibility = Visibility.Visible;
            home.Visibility = Visibility.Collapsed;
            if (await Shared.ViewModel.Log.On())
            {
                if (await Shared.ViewModel.UserProfile.Load())
                {
                    await InitMap();
                    LoadUserOffice();
                    await Shared.ViewModel.UserHome.LoadHome();
                    await Shared.ViewModel.RoutePoints.Load(RoutesOnMap);
                    await Shared.ViewModel.Organization.LoadOrganizations();


                    await Shared.ViewModel.Trips.LoadTrips();
                    isProfileLoaded = true;

                    if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count == 0)
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(JoinOrganization));
                        return;
                    }
                    home.Visibility = Visibility.Visible;
                    CheckWizard();


                }
            }
            else RestoreAccess();
            Shared.View.General.HideElements(MainUIs);
            MainGrid.Visibility = Visibility.Visible;


        }


        public async void CheckWizard()
        {
            WizardAssistant.Visibility = Visibility.Collapsed;
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0)
            {
                AppBar.Visibility = Visibility.Collapsed;
                WizardAssistant.Visibility = Visibility.Visible;
                Shared.ModelView.UIBinding.Default.Assistant = ConvertMessages.Message("SetHomeAssistance");
                SetHomeBut.IsChecked = true;
                return;
            };
            if (Shared.ModelView.UserProfile.Default.OfficeID == -1)
            {
                AppBar.Visibility = Visibility.Collapsed;
                WizardAssistant.Visibility = Visibility.Visible;
                Shared.ModelView.UIBinding.Default.Assistant = ConvertMessages.Message("SelectOfficeAssistance");
                await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                this.BottomAppBar.IsOpen = true;
                return;
            };
            BottomAppBar.Visibility = Visibility.Collapsed;

            if (Shared.ModelView.UserProfile.Default.IsDriver == true && RoutesOnMap.Children.Count == 0)
            {
                AppBar.Visibility = Visibility.Collapsed;
                WizardAssistant.Visibility = Visibility.Visible;
                Shared.ModelView.UIBinding.Default.Assistant = ConvertMessages.Message("BuildRoutesAssistance");
                WriteRouteBut.IsChecked = true;
            }
            else
            {
                AppBar.Visibility = Visibility.Visible;
            }
        }

        public async void LoadUserOffice()
        {
            OfficesOnMap.Visibility = Visibility.Collapsed;
            await Shared.ViewModel.Organization.LoadOffices(SelectedOffice, true);
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


        bool MapIsInitiated = false;
        private async Task<bool> InitMap()
        {

            if (MapIsInitiated == false)
            {
                while (!MapIsInitiated)
                {

                    if (await Shared.ViewModel.InitMaps.InitBingMapsEngine(MainMap)) MapIsInitiated = true;
                }
            }

            if (!await Shared.ViewModel.UserLocation.LoadLocation())
            {
                try
                {
                    Geolocator geolocator = new Geolocator();// { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };

                    Geoposition pos = await geolocator.GetGeopositionAsync();

                    MainMap.Center = new Shared.Model.Location(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);

                    Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);
                    await Shared.ViewModel.UserLocation.UpdateLocation();
                }
                catch (Exception) { }

            }
            MainMap.Center = new Shared.Model.Location(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude);
            return true;
        }
        

        [Flags]
        public enum OrganizationTabMode
        {
            Empty = 0x01,
            CreateOrganization = 0x02,
            JoinOrganization = 0x03,
            CreateOffice = 0x04
        }
        public static OrganizationTabMode orgMode;
        
        private void ResetUI()
        {
            Shared.View.General.HideElements(AutoCollapsedElements);
            Shared.View.General.HideElements(ExtendOrgPanel);
            //Cursor = Cursors.Arrow;
            //MainMap.DoubleTappedOverride -= SetCurrentUserPositionOnMap;
            MainMap.DoubleTappedOverride -= SetHomeOnMap;
            MainMap.DoubleTappedOverride -= SetRoutePoint;
        }



        private void HideExpandedGroupGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElement(ExtendGroupsGrid);

        }

        

        private void AddOfficeToMap(object sender, DoubleTappedRoutedEventArgs e)
        {

            e.Handled = true;

            Point mousePosition = e.GetPosition((UIElement)sender);
            ////Convert the mouse coordinates to a locatoin on the map
            var pinLocation = new Bing.Maps.Location();
            MainMap.TryPixelToLocation(mousePosition, out pinLocation);
            // The pushpin to add to the map.
            //Pushpin pin = new Pushpin();
            //pin.Location = new Location(55.76507403304246, 49.167882632601888);


            //pin.Margin = new Thickness(0, 0, 0, 3);
            //Shared.ModelView.UIBinding.Default.NewOffice = ExtendOrgPrefferedName.Text;
            NewOfficeOnMap.Children.Clear();
            var OfficeSym = Shared.View.MapsSymbols.NewOffice();
            MapLayer.SetPosition(OfficeSym, new Shared.Model.Location { Latitude = pinLocation.Latitude, Longitude = pinLocation.Longitude });
            NewOfficeOnMap.Children.Add(OfficeSym);

            //  , pinLocation, new Point(-3, -5));


            //Cursor = Cursors.Arrow;
            MainMap.DoubleTappedOverride -= AddOfficeToMap;
            //Shared.View.General.HideElement(SendKeyRegistration);
            Shared.View.General.ShowElement(ConfirmKey);
            // Adds the pushpin to the map.
            //MainMap.Children.Add(pin);
            //MainMap.Tag
            NewOfficeOnMap.Tag = pinLocation.Longitude.ToString() + ';' + pinLocation.Latitude.ToString();

            Shared.ModelView.UserOrganizations.Default.ClickedLocation = pinLocation;



        }

        

        private async void SetHomeOnMap(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            Bing.Maps.Location l = new Shared.Model.Location();
            MainMap.TryPixelToLocation(mousePosition, out l);

            Shared.ModelView.UIBinding.Default.HomeLocation = l;

            await Shared.ViewModel.UserHome.SetHome();
            SetHomeBut.IsChecked = false;
            CheckWizard();
        }


        private async void SetRoutePoint(object sender, DoubleTappedRoutedEventArgs e)
        {
            var s = Shared.ModelView.UIBinding.Default.SelectedPath;
            var SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == s);


            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            var l = new Bing.Maps.Location();
            MainMap.TryPixelToLocation(mousePosition, out l);

            var RoutePoint = Shared.View.MapsSymbols.RoutePoint(SelectedPath.IsToHome, RoutesOnMap, new SolidColorBrush(Colors.DodgerBlue));
            var RoutePointID = await Shared.ViewModel.RoutePoints.Save(l.Longitude, l.Latitude, SelectedPath.IsToHome, SelectedPath.PathID, SelectedPath.Actuality);
            if (RoutePointID == 0) RoutesOnMap.Children.Remove(RoutePoint);
            else
            {
                MapLayer.SetPosition(RoutePoint, new Shared.Model.Location { Latitude = l.Latitude, Longitude = l.Longitude });
                RoutesOnMap.Children.Add(RoutePoint);
                RoutePoint.Tag = RoutePointID;

            }
            if (SelectedPath.IsToHome)
            {
                AppBar.Visibility = Visibility.Visible;
                WizardAssistant.Visibility = Visibility.Collapsed;
            }




        }
        

        private async void ResetCarModel(object sender, SelectionChangedEventArgs e)
        {
            if (CarBrandSelector.Visibility == Visibility.Visible)
            {
                var selectedBrand = ((ComboBoxItem)CarBrandSelector.SelectedItem).Content.ToString();
                if (Shared.ModelView.UserCar.Default.Brand != selectedBrand)
                {
                    CarModelSelector.SelectionChanged -= SelectCarModel;
                    int BrandID = (int.Parse(((ComboBoxItem)CarBrandSelector.SelectedItem).Tag.ToString()));
                    Shared.ModelView.UserCar.Default.ModelID = 0;
                    Shared.ModelView.UserCar.Default.BrandID = BrandID;
                    Shared.ModelView.UserCar.Default.Brand = ((ComboBoxItem)CarBrandSelector.SelectedItem).Content.ToString();
                    Shared.View.Car.ShowCarModels(CarModelSelector, BrandID);
                    await Shared.ViewModel.Car.UpdateCar();
                    CarModelSelector.SelectionChanged += SelectCarModel;
                }
            }

        }
        
        private async void SelectCarModel(object sender, SelectionChangedEventArgs e)
        {
            if (isProfileLoaded)
            {
                var Model = ((ComboBox)sender).SelectedItem as ComboBoxItem;
                try
                {
                    int ModelID = (int.Parse((Model).Tag.ToString()));
                    Shared.ModelView.UserCar.Default.ModelID = ModelID;
                    await Shared.ViewModel.Car.UpdateCar();
                }
                catch { }
            }
        }

        
        
       

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            navigationHelper.OnNavigatedTo(e);
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.BackStack.Clear();
            Shared.Actions.initializeMap();
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
    }
}
