using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using Windows.UI;

using Windows.Devices.Geolocation;
using Shared;
using Shared.Model;
using Shared.ModelView;
using Shared.View;
using Shared.ViewModel;
using Windows.UI.Xaml.Controls.Maps;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
//using Shared.ModelView;
//using Shared.
namespace CarBuddyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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

            RegistrationGrid.DataContext = Shared.ModelView.UIBinding.Default;
            ConfirmationGrid.DataContext = Shared.ModelView.UIBinding.Default;
            LoadingAnimation.DataContext = Shared.ModelView.UIBinding.Default;
            out1.DataContext = Shared.ModelView.UIBinding.Default;
            NickName.DataContext = Shared.ModelView.UserProfile.Default;
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
            IntroJoinToOrganizations.DataContext = Shared.ModelView.UIBinding.Default;
            WizardAssistant.DataContext = Shared.ModelView.UIBinding.Default;
            //Settings.Default.Reset();
            //Settings.Default.Save();

            MainUIs.Add(ConfirmationGrid);
            MainUIs.Add(RegistrationGrid);
            MainUIs.Add(FillingProfileGrid);
            MainUIs.Add(SelectDriverModeGrid);
            MainUIs.Add(IntroJoinToOrganizations);
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
            Buttons.Add(SetCurrentPositionBut);

            Shared.Actions.showTripPointonMap = ShowTripPoint;
            Shared.Actions.showTripOffers = ShowTripOffers;
            Shared.Actions.refreshOffices = RefreshOffices;
            Shared.Actions.refreshOrganizations = RefreshOrganizations;
            Shared.Actions.refreshRoutePoints = RefreshRoutePoints;
            Shared.Actions.checkWizard = CheckWizard;
            Shared.Actions.refreshUserOffice = LoadUserOffice;

            IntialiaseUI();

            //Task.Run(async () =>  // <- marked async
            //{
            //    while (true)
            //    {

            //        await Task.Delay(10000); // <- await with cancellation
            //    }
            //});
            

        }


        private void IntialiaseUI()
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
                    break;
            }

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

        private void CheckEmail(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.CheckEmail(InputedMail, SendKey, RegistrationGrid, ShowConfirmationGrid);
        }

        private void ConfirmRegistration(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.Confirm(ConfirmPassword, PasswordFromMail, ConfirmationGrid, ShowFilliningProfileGrid);
        }

        private async void CompleteFillingProfile(object sender, RoutedEventArgs e)
        {
            if (FNameBox.Text == "")
            {
                FNameBox.BorderBrush = new SolidColorBrush(Colors.Red);





                Shared.ModelView.UIBinding.Default.OutPut += ConvertMessages.Message("x50005");
                return;
            }
            if (LNameBox.Text == "")
            {
                LNameBox.BorderBrush = new SolidColorBrush(Colors.Red);
                Shared.ModelView.UIBinding.Default.OutPut += ConvertMessages.Message("x50006");
                return;
            }
            if (PhoneBox.Text != "")
            {
                try
                {
                    long.Parse(PhoneBox.Text);
                }
                catch (Exception)
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50008");
                    return;
                }
            }
            if (FNameBox.Text != "" && LNameBox.Text != "")
            {

                if (await Shared.ViewModel.UserProfile.Update())
                {
                    Shared.View.General.ShowNextRegistrationGrid(FillingProfileGrid, ShowSelectDriverMode);
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50009");
                }
            }


        }

        void ShowConfirmationGrid()
        {
            ConfirmPassword.IsEnabled = true;

            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(ConfirmationGrid);
        }

        async void ShowFillingProfileGrid()
        {
            ContinueFillingProfile.IsEnabled = true;
            if (!isProfileLoaded) await Shared.ViewModel.UserProfile.Load();
            isProfileLoaded = true;
            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(FillingProfileGrid);
        }

        void ShowRegistrationGrid()
        {
            SendKey.IsEnabled = true;

            Shared.Model.LocalStorage.Reset();
            Shared.ModelView.UIBinding.Default.OutPut = "";
            AppBar.Visibility = Visibility.Collapsed;
            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(RegistrationGrid);
        }

        async void ShowSelectDriverModeGrid()
        {
            if (!isProfileLoaded) await Shared.ViewModel.UserProfile.Load();
            isProfileLoaded = true;
            Shared.View.General.HideElements(MainUIs);
            Shared.ModelView.UIBinding.Default.OutPut = "";
            Shared.View.General.ShowElement(SelectDriverModeGrid);
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

                    RegistrationGrid = null;
                    ConfirmationGrid = null;
                    FillingProfileGrid = null;
                    SelectDriverModeGrid = null;

                    if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count == 0)
                    {
                        Shared.View.General.HideElements(MainUIs);
                        Shared.View.General.ShowElement(IntroJoinToOrganizations);
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
                return;
            };


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

        void ShowConfirmationGrid(object sender, object e)
        {
            ShowConfirmationGrid();
        }

        private void BackToRegistration(object sender, object e)
        {
            ShowRegistrationGrid();
        }

        void ShowFilliningProfileGrid(object sender, object e)
        {

            ShowFillingProfileGrid();
        }

        private void ShowSelectDriverMode(object sender, object e)
        {
            ShowSelectDriverModeGrid();
        }

        private void ShowMainGrid(object sender, object e)
        {
            isProfileLoaded = false;
            LaunchControl();
        }


        private void BackToRegistration(object sender, RoutedEventArgs e)
        {
            Shared.View.General.ShowPreviousRegistrationGrid(ConfirmationGrid, BackToRegistration);
        }



        //private async Task<bool> LoadProfile()
        //{
        //    return await ViewModel.Loading.LoadProfile();
        //}


        async void RestoreAccess()
        {
            if (!await Shared.ViewModel.Registration.RestoreAccess())
            {
                if (await CheckServer.CheckAvailability()) ShowRegistrationGrid();
            }
            else
            {
                LaunchControl();
            }
        }

        private void CheckPhoneNumber(object sender, TextChangedEventArgs e)
        {
            // ViewModel.CheckFormat.CheckPhone(PhoneBox);
        }

        bool MapIsInitiated = false;
        private async Task<bool> InitMap()
        {

            if (MapIsInitiated == false)
            {
                while (!MapIsInitiated)
                {

                   // if (await Shared.ViewModel.InitMaps.InitBingMapsEngine(MainMap)) MapIsInitiated = true;
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

        private void EmailChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //SignOnBut.Visibility = Visibility.Collapsed;
                SendKey.Visibility = Visibility.Visible;
            }
            catch (Exception) { }
        }

        //private void SignOn(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.Registration.CheckEmail(InputedMail, SendKey, RegistrationGrid, ShowConfirmationGrid,true);
        //}

        private void HideProfileSettings(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(ProfileGrid, AutoCollapsedElements);
        }

        private void ShowProfileSettings(object sender, RoutedEventArgs e)
        {
            if (ProfileGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(ProfileGrid, AutoCollapsedElements);
            }
            else
            {
                Shared.View.General.ShowLeftMenu(ProfileGrid, AutoCollapsedElements);
            }
        }

        private void ShowSettingsGrid(object sender, RoutedEventArgs e)
        {
            //Shared.View.General.ShowLeftMenu(SettingsGrid, AutoCollapsedElements);
        }

        private void HideSettingsGrid(object sender, RoutedEventArgs e)
        {
            // Shared.View.General.HideLeftMenu(SettingsGrid, AutoCollapsedElements);
        }
        bool isScheduleLoaded = false;
        private async void ShowScheduleGrid(object sender, RoutedEventArgs e)
        {

            if (isScheduleLoaded == false)
            {
                while (!isScheduleLoaded)
                {
                    if (await Shared.ViewModel.UserSchedule.Load()) isScheduleLoaded = true;
                }
                //if (await ViewModel.UserSchedule.LoadSchedule()) isScheduleLoaded = true;
                //else 
            }
            if (ScheduleGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(ScheduleGrid, AutoCollapsedElements);
            }
            else
            {
                Shared.View.General.ShowLeftMenu(ScheduleGrid, AutoCollapsedElements);
            }

        }

        private void HideScheduleGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(ScheduleGrid, AutoCollapsedElements);
        }

        private async void ShowMessagesGrid(object sender, RoutedEventArgs e)
        {

            if (MessagesGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(MessagesGrid, AutoCollapsedElements);
            }
            else
            {
                await Shared.ViewModel.Messages.Load();
                Shared.View.General.ShowLeftMenu(MessagesGrid, AutoCollapsedElements);
            }
        }

        private void HideMessagesGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(MessagesGrid, AutoCollapsedElements);
        }

        private void ShowGroupsGrid(object sender, RoutedEventArgs e)
        {
            if (GroupsGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(GroupsGrid, AutoCollapsedElements);
            }
            else
            {
                Shared.View.General.ShowLeftMenu(GroupsGrid, AutoCollapsedElements);
                Shared.View.General.HideElement(ExtendGroupsGrid);
                //ViewModel.Organization.LoadOffices(MyOfficeList, MainMap);
            }
        }

        private async void ShowTripCarGrid(object sender, RoutedEventArgs e)
        {
            await Shared.ViewModel.Car.LoadCar();
            BrandX.Content = Shared.ModelView.UserCar.Default.Brand;
            ModelX.Content = Shared.ModelView.UserCar.Default.Model;
            //GovNumber.Text = Shared.ModelView.UserCar.Default.GovNumber;
            //PassPLaces.Text = Shared.ModelView.UserCar.Default.Places.ToString();
            await Shared.ViewModel.Trips.LoadTrips();
            if (CarTripGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(CarTripGrid, AutoCollapsedElements);
            }
            else
            {
                Shared.View.General.ShowLeftMenu(CarTripGrid, AutoCollapsedElements);
            }
        }

        private void FieldChanged(object sender, TextChangedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isProfileChanged = true;
        }

        private void FieldChanged(object sender, SelectionChangedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isProfileChanged = true;
        }

        private void HideExtendGrid(object sender, RoutedEventArgs e)
        {
            NewOfficeOnMap.Visibility = Visibility.Collapsed;
            OfficesOnMap.Visibility = Visibility.Visible;
            Shared.View.General.HideElement(ExtendCompanyGrid);
        }

        private void ShowCreateTeamGrid(object sender, RoutedEventArgs e)
        {
            ExtendTeamHeader.Text = ConvertMessages.Message("CreateTeam");
            GreetingTeam.Text = ConvertMessages.Message("PrefferedName");
            SubmitTeamCreation.Content = ConvertMessages.Message("SubmitCreation");
            Shared.View.General.ShowElement(ExtendGroupsGrid);
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

        private void SelectCreateOrganization(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElements(ExtendOrgPanel);
            Shared.View.General.ShowElements(new List<FrameworkElement> { ExtendCompanyGrid, ExtendOrgdHeader, ExtendOrgCloseButton, ExtendOrgPrefferedNameGreeting, ExtendOrgPrefferedName, WorkEmailGreeting, WorkMail, SendKeyRegistration });
            ExtendOrgdHeader.Text = ConvertMessages.Message("CreateCompany");
            ConfirmKey.Content = ConvertMessages.Message("CreateCompany");
            SendKeyRegistration.Content = ConvertMessages.Message("SendKey");
            orgMode = OrganizationTabMode.CreateOrganization;
            Shared.View.General.CleanValue(ExtendOrgPrefferedName);
            Shared.View.General.CleanValue(WorkMail);
            Shared.View.General.CleanValue(KeyFromMail);

        }

        private void SelectJoinOrganization(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElements(ExtendOrgPanel);
            Shared.View.General.ShowElements(new List<FrameworkElement> { ExtendCompanyGrid, ExtendOrgdHeader, ExtendOrgCloseButton, WorkEmailGreeting, WorkMail, SendKeyRegistration });
            ExtendOrgdHeader.Text = ConvertMessages.Message("JoinOrganization");
            SendKeyRegistration.Content = ConvertMessages.Message("SendKey");
            ConfirmKey.Content = ConvertMessages.Message("Join");
            orgMode = OrganizationTabMode.JoinOrganization;
            Shared.View.General.CleanValue(WorkMail);
            Shared.View.General.CleanValue(KeyFromMail);

        }


        private void SelectCreateOffice(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElements(ExtendOrgPanel);
            Shared.View.General.ShowElements(new List<FrameworkElement> { ExtendCompanyGrid, ExtendOrgdHeader, ExtendOrgCloseButton, ExtendOrgPrefferedNameGreeting, ExtendOrgPrefferedName, SendKeyRegistration });
            ExtendOrgdHeader.Text = ConvertMessages.Message("CreateOffice");
            ConfirmKey.Content = ConvertMessages.Message("SubmitCreation");
            SendKeyRegistration.Content = ConvertMessages.Message("SetOffice");

            orgMode = OrganizationTabMode.CreateOffice;
            Shared.View.General.CleanValue(ExtendOrgPrefferedName);
        }

        private void ShowJoinTeamGrid(object sender, RoutedEventArgs e)
        {
            ExtendTeamHeader.Text = ConvertMessages.Message("JoinTeam");
            GreetingTeam.Text = ConvertMessages.Message("InputInvite");
            SubmitTeamCreation.Content = ConvertMessages.Message("Join");
            Shared.View.General.ShowElement(ExtendGroupsGrid);
        }


        private void HideOrganizationsSettings(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(OrganizationGrid, AutoCollapsedElements);
            LoadUserOffice();
        }

        private async void ShowOrganizationGrid(object sender, RoutedEventArgs e)
        {
            if (OrganizationGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(OrganizationGrid, AutoCollapsedElements);

                LoadUserOffice();
            }
            else
            {
                Shared.View.General.ShowLeftMenu(OrganizationGrid, AutoCollapsedElements);
                Shared.View.General.HideElements(ExtendOrgPanel);
                await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                await Shared.ViewModel.Organization.LoadOrganizations();
            }
        }

        private void ResetUI()
        {
            Shared.View.General.HideElements(AutoCollapsedElements);
            Shared.View.General.HideElements(ExtendOrgPanel);
            //Cursor = Cursors.Arrow;
            MainMap.DoubleTapped -= SetCurrentUserPositionOnMap;
            MainMap.DoubleTapped -= SetHomeOnMap;
            MainMap.DoubleTapped -= SetRoutePoint;
        }



        private void HideExpandedGroupGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElement(ExtendGroupsGrid);

        }


        private void CreateTeam(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateInvite(object sender, RoutedEventArgs e)
        {
            //ExtendTeamHeader.Text = Properties.Resources.GenerateInvite;
            //GreetingTeam.Text = "";
            //SubmitTeamCreation.Content = Properties.Resources.GenerateInvite;
            //Shared.View.General.ShowElement(ExtendGroupsGrid);
        }


        private async void SendKeyRegistrationToMail(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.OutPut = "";
            if (orgMode == OrganizationTabMode.CreateOrganization)
            {
                if (await Shared.ViewModel.Organization.Registration(WorkMail.Text.Replace(" ", ""), ExtendOrgPrefferedName.Text))
                {
                    Shared.View.General.HideElements(new List<FrameworkElement> { ExtendOrgPrefferedNameGreeting, ExtendOrgPrefferedName, WorkEmailGreeting, WorkEmailGreeting, WorkMail, SendKeyRegistration });
                    Shared.View.General.ShowElements(new List<FrameworkElement> { InputeKeyFromMailGreeting, KeyFromMail, ConfirmKey });
                };
            }
            if (orgMode == OrganizationTabMode.JoinOrganization)
            {
                if (await Shared.ViewModel.Organization.StartJoin(WorkMail.Text.Replace(" ", "")))
                {
                    Shared.View.General.HideElements(new List<FrameworkElement> { WorkEmailGreeting, WorkMail, SendKeyRegistration });
                    Shared.View.General.ShowElements(new List<FrameworkElement> { InputeKeyFromMailGreeting, KeyFromMail, ConfirmKey });
                };
            }
            if (orgMode == OrganizationTabMode.CreateOffice)
            {
                OfficesOnMap.Visibility = Visibility.Collapsed;
                NewOfficeOnMap.Visibility = Visibility.Visible;
                SelectedOffice.Visibility = Visibility.Collapsed;
                NewOfficeOnMap.Children.Clear();
                if (ExtendOrgPrefferedName.Text == "")
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50011");
                }
                else
                {
                    //SendKeyRegistration.Tag = ExtendOrgPrefferedName.Text + ";" + Shared.ModelView.UserOrganizations.Default.SelectedOrganization.GetType();
                    //Cursor = Cursors.Cross;
                    MainMap.DoubleTapped += AddOfficeToMap;
                    //MainMap.ta
                };
            }
        }

        private void AddOfficeToMap(object sender, DoubleTappedRoutedEventArgs e)
        {

            e.Handled = true;

            Point mousePosition = e.GetPosition((UIElement)sender);
            var pinLocation = new Location();
            //MainMap.TryPixelToLocation(mousePosition, out pinLocation);
           
            NewOfficeOnMap.Children.Clear();
            var OfficeSym = Shared.View.MapsSymbols.NewOffice();
            MapControl.SetLocation(OfficeSym, new Shared.Model.Location { Latitude = pinLocation.Latitude, Longitude = pinLocation.Longitude });
            NewOfficeOnMap.Children.Add(OfficeSym);


            MainMap.DoubleTapped -= AddOfficeToMap;
            Shared.View.General.ShowElement(ConfirmKey);
            NewOfficeOnMap.Tag = pinLocation.Longitude.ToString() + ';' + pinLocation.Latitude.ToString();

            Shared.ModelView.UserOrganizations.Default.ClickedLocation = pinLocation;



        }


        private async void CompleteOrgActitivity(object sender, RoutedEventArgs e)
        {
            if (orgMode == OrganizationTabMode.CreateOrganization)
            {
                if (await Shared.ViewModel.Organization.Confirmation(WorkMail.Text.Split('@')[1], KeyFromMail.Password.Replace(" ", "")))
                {
                    Shared.View.General.CleanValue(KeyFromMail);
                    Shared.View.General.HideElements(new List<FrameworkElement> { InputeKeyFromMailGreeting, KeyFromMail, ConfirmKey, ExtendCompanyGrid });

                    await Shared.ViewModel.Organization.LoadOrganizations();
                    await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                };
            }
            if (orgMode == OrganizationTabMode.JoinOrganization)
            {
                if (await Shared.ViewModel.Organization.CompleteJoiner(WorkMail.Text.Split('@')[1], KeyFromMail.Password.Replace(" ", "")))
                {
                    Shared.View.General.HideElements(new List<FrameworkElement> { WorkEmailGreeting, WorkMail, SendKeyRegistration, ExtendCompanyGrid });
                    Shared.View.General.CleanValue(KeyFromMail);
                    await Shared.ViewModel.Organization.LoadOrganizations();
                    await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                };
            }
            if (orgMode == OrganizationTabMode.CreateOffice)
            {
                if (NewOfficeOnMap.Tag != null)
                {
                    var OfficeInfo = NewOfficeOnMap.Tag.ToString();
                    NewOfficeOnMap.Tag = null;
                    if (await Shared.ViewModel.Organization.CreateOffice(Shared.ModelView.UserOrganizations.Default.SelectedOrganization, Shared.ModelView.UserOrganizations.Default.NewOfficeName, Shared.ModelView.UserOrganizations.Default.ClickedLocation.Longitude, Shared.ModelView.UserOrganizations.Default.ClickedLocation.Latitude))
                    {
                        Shared.View.General.HideElements(new List<FrameworkElement> { WorkEmailGreeting, WorkMail, SendKeyRegistration, ExtendCompanyGrid, NewOfficeOnMap });
                        await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                    };
                }
            }
        }

        private void StartSetHomeOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, SetHomeBut);
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;
            home.Visibility = Visibility.Collapsed;
            MainMap.DoubleTapped += SetHomeOnMap;
        }

        private async void SetHomeOnMap(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            var l = new Location();
            //MainMap.TryPixelToLocation(mousePosition, out l);

            Shared.ModelView.UIBinding.Default.HomeLocation = l;

            await Shared.ViewModel.UserHome.SetHome();
            SetHomeBut.IsChecked = false;
            CheckWizard();
        }
        private void EndSetHomeOnMap(object sender, RoutedEventArgs e)
        {
            MainMap.DoubleTapped -= SetHomeOnMap;
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
            home.Visibility = Visibility.Visible;
        }

        private void HideGroupsGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(GroupsGrid, AutoCollapsedElements);
        }

        private void StartWriteMyRoutesOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.PathControl.ShowPathControl(PathControl);
            Shared.View.General.UnCheckButton(Buttons, WriteRouteBut);
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
            StartWriteRoute(sender, e);
        }

        private void CompleteWritingRoutes(object sender, RoutedEventArgs e)
        {
            //Cursor = Cursors.Arrow;
            MainMap.DoubleTapped -= SetRoutePoint;
        }


        private async void SetRoutePoint(object sender, DoubleTappedRoutedEventArgs e)
        {
            var s = Shared.ModelView.UIBinding.Default.SelectedPath;
            var SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == s);


            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            var l = new Location();
            //MainMap.TryPixelToLocation(mousePosition, out l);

            var RoutePoint = Shared.View.MapsSymbols.RoutePoint(SelectedPath.IsToHome, RoutesOnMap, new SolidColorBrush(Colors.DodgerBlue));
            var RoutePointID = await Shared.ViewModel.RoutePoints.Save(l.Longitude, l.Latitude, SelectedPath.IsToHome, SelectedPath.PathID, SelectedPath.Actuality);
            if (RoutePointID == 0) RoutesOnMap.Children.Remove(RoutePoint);
            else
            {
                MapControl.SetLocation(RoutePoint, new Location { Latitude = l.Latitude, Longitude = l.Longitude });
                RoutesOnMap.Children.Add(RoutePoint);
                RoutePoint.Tag = RoutePointID;

            }
            if (SelectedPath.IsToHome)
            {
                AppBar.Visibility = Visibility.Visible;
                WizardAssistant.Visibility = Visibility.Collapsed;
            }




        }

        private void StartWriteRoute(object sender, RoutedEventArgs e)
        {
            MainMap.DoubleTapped -= SetRoutePoint;
            //Cursor = Cursors.Cross;

            MainMap.DoubleTapped += SetRoutePoint;
        }

        private void StartFindCompanions(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, FindCompanionsBut);

            Shared.ViewModel.Companions.FindCompanions(CompanionsOnMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);


            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;

        }
        private void EndFindCompanions(object sender, RoutedEventArgs e)
        {
            CompanionsOnMap.Children.Clear();
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
        }

        private void SearchCompanionsToHome(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = true;
            if (isProfileLoaded) Shared.ViewModel.Companions.FindCompanions(CompanionsOnMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }

        private void SearchCompanionsToWork(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = false;
            Shared.ViewModel.Companions.FindCompanions(CompanionsOnMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }

        private void HideCarTripGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(CarTripGrid, AutoCollapsedElements);
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

        private void StartSelectCarBrand(object sender, object e)
        {
            Shared.View.Car.ShowCarBrands(CarBrandSelector);
            CarBrandSelector.SelectionChanged += ResetCarModel;
        }


        private async void ChangeDriverMode(object sender, RoutedEventArgs e)
        {
            if (ProfileGrid.Visibility == Visibility.Visible) await Shared.ViewModel.UserProfile.ChangeDriverMode();
        }

        private void FinalSelectCarBrand(object sender, object e)
        {
            CarBrandSelector.SelectionChanged -= ResetCarModel;
        }

        private async void SelectCarModel(object sender, SelectionChangedEventArgs e)
        {
            if (isProfileLoaded)
            {
                var Model = ((ComboBox)sender).SelectedItem as ComboBoxItem;
                int ModelID = (int.Parse((Model).Tag.ToString()));
                Shared.ModelView.UserCar.Default.ModelID = ModelID;
                await Shared.ViewModel.Car.UpdateCar();
            }
        }

        //private async void UpdateCarNumber(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    await ViewModel.Car.UpdateCar();
        //}

        private async void ExpandPassangerPlaces(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places++;
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void DecreasePassangerPlaces(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places--;
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void LeaveSelectedOrganization(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Organization.LeaveOrganization();
            await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
        }

        private void CheckOrg(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (OrganizationGrid.Visibility == Visibility.Collapsed)
            {
                ResetUI();
                if (Shared.ModelView.UserProfile.Default.OfficeID == -1) OfficesOnMap.Children.Clear();
            }
        }



        private async void SelectedDriverModePassenger(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserProfile.Default.IsDriver = false;
            if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
            {
                Shared.View.General.ShowNextRegistrationGrid(SelectDriverModeGrid, ShowMainGrid);
            }
        }

        private async void SelectedDriverModeDriver(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserProfile.Default.IsDriver = true;
            if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
            {
                Shared.View.General.ShowNextRegistrationGrid(SelectDriverModeGrid, ShowMainGrid);
            }
        }



        private async void UpdateSchedule(object sender, RoutedEventArgs e)
        {
            if (ScheduleGrid.Visibility == Visibility.Visible && isProfileLoaded) await Shared.ViewModel.UserSchedule.Update();
        }

        private async void UpdateSchedule(object sender, TimePickerValueChangedEventArgs e)
        {
            if (ScheduleGrid != null)
            {
                if (ScheduleGrid.Visibility == Visibility.Visible && isProfileLoaded) await Shared.ViewModel.UserSchedule.Update();
            }
        }

        private void StartSetCurrentPositionOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, SetCurrentPositionBut);
            currenposonmap.Visibility = Visibility.Collapsed;
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;

            MainMap.DoubleTapped += SetCurrentUserPositionOnMap;
        }

        private void EndSetCurrentPositionOnMap(object sender, RoutedEventArgs e)
        {

            MainMap.DoubleTapped -= SetCurrentUserPositionOnMap;
            currenposonmap.Visibility = Visibility.Visible;
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
        }

        private async void SetCurrentUserPositionOnMap(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            var l = new Location();
            //MainMap.TryPixelToLocation(mousePosition, out l);

            Shared.ModelView.UIBinding.Default.CurrentUserPosition = l;

            await Shared.ViewModel.UserLocation.UpdateLocation();
            SetCurrentPositionBut.IsChecked = false;
        }

        private void OpenPrivacyPolicy(object sender, RoutedEventArgs e)
        {
            try
            {
                //Process.Start("http://unicarbuddy.ru/about/PrivacyPolicy");
            }
            catch { }
        }

        private async void AutoJoin(object sender, RoutedEventArgs e)
        {
            try
            {
                await Shared.ViewModel.Organization.AutoJoin("unicarbuddy@icl-services.com");
            }
            catch (Exception) { }
        }

        private void AddNewPath(object sender, RoutedEventArgs e)
        {
            var latestPath = Shared.ModelView.UIBinding.Default.Routes.OrderBy(req => req.PathID).Last();
            var weekactual = new Shared.Model.WeekActuality { Monday = true, Friday = true, Saturday = false, Sunday = false, Thursday = true, Tuesday = true, Wednesday = true };
            var newRoute = new Shared.Model.Path { IsFri = weekactual.Friday, IsMon = weekactual.Monday, IsSat = weekactual.Saturday, IsSun = weekactual.Sunday, IsThu = weekactual.Thursday, IsToHome = !latestPath.IsToHome, IsTue = weekactual.Tuesday, IsWed = weekactual.Wednesday, PathID = (latestPath.PathID + 1), Actuality = weekactual };
            var temparr = Shared.ModelView.UIBinding.Default.Routes;
            temparr.Add(newRoute);
            Shared.ModelView.UIBinding.Default.Routes = temparr;
            Shared.View.PathControl.ShowPathControl(PathControl);
        }

        private async void AutoSaveProfile(object sender, RoutedEventArgs e)
        {
            var Grid = sender as Grid;
            if (Grid.Visibility == Visibility.Collapsed && Shared.ModelView.UIBinding.Default.isProfileChanged) await Shared.ViewModel.UserProfile.Update();
        }

        private async void StartIntroJoinOrg(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.StartJoin(IntroOrgBox.Text.Replace(" ", "")))
            {
                Shared.View.General.HideElement(OrgJoinIntro);
                Shared.View.General.ShowElement(OrgJoinConfrimationStack);
            }
            else
            {
                OrgJoinComplete.Visibility = Visibility.Visible;
            }
        }

        private async void ConfirmIntroOrgJoin(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.CompleteJoiner(IntroOrgBox.Text.Split('@')[1], IntroOrgJoinPassBox.Password.Replace(" ", "")))
            {
                Shared.View.General.HideElement(ManualJoinOrgStackPanel);
                Shared.View.General.CleanValue(KeyFromMail);
                Shared.View.General.ShowElement(OrgJoinComplete);
                Shared.View.General.ShowElement(SuccessStoryLabel);
                await Shared.ViewModel.Organization.LoadOrganizations();
                await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
            };
        }


        private void ContinueIntro(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideElements(MainUIs);
            MainGrid.Visibility = Visibility.Visible;
            CheckWizard();

        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int i = int.Parse(s.Tag.ToString());
            string texts = s.Name;
            await Shared.ViewModel.Messages.Send(i, texts);
        }

        private void OfficesLoadd(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).SelectedIndex = Shared.ModelView.UIBinding.Default.SelectedOffice;
        }
    }
}
