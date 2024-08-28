using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Device.Location;
using CCS.Classic.Properties;
using System.Net.Http;
using Microsoft.Maps.MapControl.WPF;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using Shared.Model;
using Shared.ModelView;
using Shared.View;
using Shared.ViewModel;
using Shared;
using Microsoft.AspNet.SignalR.Client;

namespace CCS.Classic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        List<FrameworkElement> MainUIs = new List<FrameworkElement>();
        List<FrameworkElement> AutoCollapsedElements = new List<FrameworkElement>();
        List<FrameworkElement> ExtendOrgPanel = new List<FrameworkElement>();
        List<FrameworkElement> ExtendGroupPanel = new List<FrameworkElement>();

        List<ToggleButton> Buttons = new List<ToggleButton>();
        bool isProfileLoaded = false;

        public IHubProxy HubProxy { get; set; }
#if DEBUG
        const string ServerURI = "http://api.commutecarsharing.ru";//*/ "http://localhost:59524";
        
#else
        const string ServerURI = "http://api.commutecarsharing.ru";
#endif

        public HubConnection Connection { get; set; }

        public MainWindow()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");

            //CheckServer.CheckLocation();
            InitializeComponent();


            Autoupgrade.CheckLatestVersion();
            //ScheduleGrid.DataContext = Shared.ModelView.UserSchedule.Default;
            OrganizationGrid.DataContext = Shared.ModelView.UserOrganizations.Default;
            //Settings.Default.Reset();
            //Settings.Default.Save();

            MainUIs.Add(ConfirmationGrid);
            MainUIs.Add(RegistrationGrid);
            MainUIs.Add(FillingProfileGrid);
            MainUIs.Add(SelectDriverModeGrid);
            MainUIs.Add(IntroJoinToOrganizations);
            MainUIs.Add(MainGrid);

            
            AutoCollapsedElements.Add(ProfileGrid);
            AutoCollapsedElements.Add(GroupsGrid);
            AutoCollapsedElements.Add(OrganizationGrid);
            AutoCollapsedElements.Add(ExtendCompanyGrid);
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
            //ExtendOrgPanel.Add(CreateOffice);
            //ExtendOrgPanel.Add(LeaveOrganization);
            ExtendOrgPanel.Add(InputeKeyFromMailGreeting);
            ExtendOrgPanel.Add(KeyFromMail);
            ExtendOrgPanel.Add(ConfirmKey);

            Shared.View.General.HideElements(ExtendOrgPanel);


            Buttons.Add(WriteRouteBut);
            Buttons.Add(FindCompanionsBut);
            Buttons.Add(SetHomeBut);
            Buttons.Add(SetCurrentPositionBut);

            IntialiaseUI();

            Shared.Actions.showTripPointonMap = ShowTripPoint;
            Shared.Actions.refreshOffices = RefreshOffices;
            Shared.Actions.refreshOrganizations = RefreshOrganizations;
            Shared.Actions.refreshRoutePoints = RefreshRoutePoints;
            Shared.Actions.checkWizard = CheckWizard;
            Shared.Actions.refreshUserOffice = LoadUserOffice;
            Shared.Actions.SentMessage = SendSignalNewMessage;
            Shared.Actions.UpdateTrip = SendInviteToUser;

        }

        private void SendSignalNewMessage(int obj)
        {
            try
            {
                HubProxy.Invoke("SendMessageToUser", obj);
            }
            catch {

            }
        }

        private void SendInviteToUser(int obj)
        {
            try
            {
                HubProxy.Invoke("UpdateTripToUser", obj);
            }
            catch {
            }
        }

        private async void IntialiaseUI()
        {
            if (await CheckServer.isInOrgInternalNetwork("icl-services.com")) Shared.ModelView.UIBinding.Default.AutoJoinVisibility = Visibility.Visible;
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

        private void ShowTripPoint()
        {

            Shared.View.TripOffer.ShowTripPointsOnMap(TripPoints);

        }


        private async void RefreshOffices()
        {
            await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);

        }

        private async void RefreshOrganizations()
        {
            await Shared.ViewModel.Organization.LoadOrganizations();
        }

        private void RefreshRoutePoints()
        {
            Shared.ViewModel.RoutePoints.RefreshPath(RoutesOnMap);
        }

        private void CheckEmail(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.CheckEmail(InputedMail, SendKey, RegistrationGrid, ShowConfirmationGrid);
        }

        private void ConfirmRegistration(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Registration.Confirm( ConfirmPassword, PasswordFromMail, ConfirmationGrid, ShowFilliningProfileGrid);
        }

        private async void CompleteFillingProfile(object sender, RoutedEventArgs e)
        {
            if (FNameBox.Text == "")
            {
                FNameBox.BorderBrush = Brushes.Red;

                Shared.ModelView.UIBinding.Default.OutPut += Properties.Resources.x50005;
                return;
            }
            if (LNameBox.Text == "")
            {
                LNameBox.BorderBrush = Brushes.Red;
                Shared.ModelView.UIBinding.Default.OutPut += Properties.Resources.x50006;
                return;
            }
            if (PhoneBox.Text != "") {
                try {
                    long.Parse(PhoneBox.Text);
                }
                catch (Exception) {
                    UIBinding.Default.OutPut = Properties.Resources.x50008;
                    return;
                }
            }
            if (FNameBox.Text != "" && LNameBox.Text != "")
            {

                if (await Shared.ViewModel.UserProfile.Update())
                {
                    Shared.View.General.ShowNextRegistrationGrid(FillingProfileGrid, ShowSelectDriverMode);
                }
                else {
                    UIBinding.Default.OutPut = Properties.Resources.x50009;
                }
            }


        }

        void ShowConfirmationGrid() {
            PasswordFromMail.Password = "";
            MainWND.Width = 800;
            MainWND.Height = 480;
            ConfirmPassword.IsEnabled = true;

            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(ConfirmationGrid);
        }

        async void ShowFillingProfileGrid()
        {
            MainWND.Width = 800;
            MainWND.Height = 480;
            ContinueFillingProfile.IsEnabled = true;
            if (!isProfileLoaded) await Shared.ViewModel.UserProfile.Load();
            isProfileLoaded = true;
            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(FillingProfileGrid);
        }

        void ShowRegistrationGrid()
        {
            MainWND.Width = 800;
            MainWND.Height = 480;
            SendKey.IsEnabled = true;
            Settings.Default.Reset();
            Settings.Default.Save();
            UIBinding.Default.OutPut = "";

            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(RegistrationGrid);
        }
        
        async void ShowSelectDriverModeGrid()
        {
            MainWND.Width = 800;
            MainWND.Height = 480;
            if (!isProfileLoaded) await Shared.ViewModel.UserProfile.Load();
            isProfileLoaded = true;
            Shared.View.General.HideElements(MainUIs);
            Shared.View.General.ShowElement(SelectDriverModeGrid);
            UIBinding.Default.OutPut = "";
        }

        async void LaunchControl()
        {
            AppBar.Visibility = Visibility.Visible;
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
                    CheckWizard();
                }
            }
            else RestoreAccess();
            Shared.View.General.HideElements(MainUIs);
            MainGrid.Visibility = Visibility.Visible;
            ConnectAsync();


        }

        private async void ConnectAsync()
        {
            try
            {
                Connection = new HubConnection(ServerURI);
                //Connection.Closed += Connection_Closed;
                HubProxy = Connection.CreateHubProxy("ChatHub");
                //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
                HubProxy.On("AddedMessage", async () =>
                {
                    try
                    {
                        await Shared.ViewModel.Messages.Load();
                    }
                    catch(Exception e) {

                    }
                });
                HubProxy.On("UpdateTrip", async () =>
                {
                    try
                    {
                        await Shared.ViewModel.Trips.LoadTrips();
                    }
                    catch(Exception e) {
                    }
                });
                try
                {
                    await Connection.Start();
                    await HubProxy.Invoke("Register", LocalStorage.UID);
                }
                catch(Exception e)
                {

                }
            }
            catch(Exception e) {

            }

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
                if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count > 0)
                {
                    AppBar.Visibility = Visibility.Collapsed;
                    WizardAssistant.Visibility = Visibility.Visible;
                    Shared.ModelView.UIBinding.Default.Assistant = ConvertMessages.Message("SelectOfficeAssistance");
                    await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                    return;
                }
            };


            //if (Shared.ModelView.UserProfile.Default.IsDriver == true && RoutesOnMap.Children.Count == 0)
            //{
            //    AppBar.Visibility = Visibility.Collapsed;
            //    WizardAssistant.Visibility = Visibility.Visible;
            //    Shared.ModelView.UIBinding.Default.Assistant = ConvertMessages.Message("BuildRoutesAssistance");
            //    WriteRouteBut.IsChecked = true;
            //}
            //else
            //{
            //    AppBar.Visibility = Visibility.Visible;
            //}
        }


        async void LoadUserOffice()
        {
            OfficesOnMap.Visibility = Visibility.Collapsed;
            await Shared.ViewModel.Organization.LoadOffices(SelectedOffice, true);
        }

        void ShowConfirmationGrid(object sender, EventArgs e)
        {
            ShowConfirmationGrid();
        }

        private void BackToRegistration(object sender, EventArgs e)
        {
            ShowRegistrationGrid();
        }

        void ShowFilliningProfileGrid(object sender, EventArgs e)
        {
            
            ShowFillingProfileGrid();
        }

        private void ShowSelectDriverMode(object sender, EventArgs e)
        {
            ShowSelectDriverModeGrid();
        }

        private void ShowMainGrid(object sender, EventArgs e)
        {
            isProfileLoaded = false;
            LaunchControl();
        }


        private void BackToRegistration(object sender, RoutedEventArgs e)
        {
            Shared.View.General.ShowPreviousRegistrationGrid(ConfirmationGrid, BackToRegistration);
        }

        async void RestoreAccess() {
            if (!await Shared.ViewModel.Registration.RestoreAccess()) {
                ShowRegistrationGrid();
            }
            else
            {
                LaunchControl();
            }
        }

        private void CheckPhoneNumber(object sender, TextChangedEventArgs e)
        {
            Shared.ViewModel.CheckFormat.CheckPhone(PhoneBox);
        }

        bool MapIsInitiated = false;
        private async Task<bool> InitMap() {

            if (MapIsInitiated == false)
            {
                if (await Shared.ViewModel.InitMaps.InitBingMapsEngine(MainMap)) MapIsInitiated = true;
            }

            if (!await Shared.ViewModel.UserLocation.LoadLocation())
            {
                string externalip = new WebClient().DownloadString("http://icanhazip.com");
                try
                {
                    var request = WebRequest.Create("http://ip-api.com/json/" + externalip.Split('\\')[0]);
                    var response = await request.GetResponseAsync();

                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Shared.GeoPosition.jsonItem));

                    var GeoIP = (Shared.GeoPosition.jsonItem)ser.ReadObject(response.GetResponseStream());
                    Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(GeoIP.lat, GeoIP.lon);
                    Shared.ModelView.UIBinding.Default.CurrentCenter=new Shared.Model.Location(GeoIP.lat, GeoIP.lon);
                    await Shared.ViewModel.UserLocation.UpdateLocation();
                }
                catch { }
            }
            return true;
        }

        private void EmailChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).IsInitialized)
            {
                try
                {
                    SendKey.Visibility = Visibility.Visible;
                }
                catch (Exception) { }
            }
        }


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
           // View.General.ShowLeftMenu(SettingsGrid, AutoCollapsedElements);
        }

        private void HideSettingsGrid(object sender, RoutedEventArgs e)
        {
            //View.General.HideLeftMenu(SettingsGrid, AutoCollapsedElements);
        }
        bool isScheduleLoaded = false;
        private async void ShowScheduleGrid(object sender, RoutedEventArgs e)
        {
            if (isScheduleLoaded == false)
            {
                if (await Shared.ViewModel.UserSchedule.Load(WeeklySchedulesStack)) isScheduleLoaded = true;
                
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
            }
        }
        bool isCarLoaded = false;
        private async void ShowTripCarGrid(object sender, RoutedEventArgs e)
        {
            if (isCarLoaded == false)
            {
                if (Shared.ModelView.UserProfile.Default.IsDriver)
                {
                    if (await Shared.ViewModel.Car.LoadCar()) isCarLoaded = true;
                    Shared.View.Car.ShowCarModels(CarModelSelector, Shared.ModelView.UserCar.Default.BrandID);
                    Shared.View.Car.ShowCarColors(CarColorSelector, Shared.ModelView.UserCar.Default.ColorID);
                }
            }

            await Shared.ViewModel.Trips.LoadTrips();
            if (CarTripGrid.Visibility == Visibility.Visible)
            {
                Shared.View.General.HideLeftMenu(CarTripGrid, AutoCollapsedElements);
            }
            else
            {
                ResetUI();
                Shared.View.General.ShowLeftMenu(CarTripGrid, AutoCollapsedElements);
            }
            Shared.View.Car.ShowCarModels(CarModelSelector, Shared.ModelView.UserCar.Default.BrandID);
            Shared.View.Car.ShowCarColors(CarColorSelector, Shared.ModelView.UserCar.Default.ColorID);
        }

        private void FieldChanged(object sender, TextChangedEventArgs e)
        {
            UIBinding.Default.isProfileChanged = true;
        }

        private void FieldChanged(object sender, SelectionChangedEventArgs e)
        {
            UIBinding.Default.isProfileChanged = true;
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
            Shared.ModelView.UIBinding.Default.OutPut = "";
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

        private void ResetUI() {
            Shared.View.General.HideElements(AutoCollapsedElements);
            Shared.View.General.HideElements(ExtendOrgPanel);
            Cursor = Cursors.Arrow;
            MainMap.MouseDoubleClick -= SetCurrentUserPositionOnMap;
            MainMap.MouseDoubleClick -= SetHomeOnMap;
            MainMap.MouseDoubleClick -= SetRoutePoint;
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
            ExtendTeamHeader.Text = Properties.Resources.GenerateInvite;
            GreetingTeam.Text = "";
            SubmitTeamCreation.Content = Properties.Resources.GenerateInvite;
            Shared.View.General.ShowElement(ExtendGroupsGrid);
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
                    SendKeyRegistration.Tag = ExtendOrgPrefferedName.Text + ";" + Shared.ModelView.UserOrganizations.Default.SelectedOrganization.ToString();
                    Cursor = Cursors.Cross;
                    MainMap.MouseDoubleClick += AddOfficeToMap;
                };
            }
        }

        private void AddOfficeToMap(object sender, MouseButtonEventArgs e)
        {
            
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            Microsoft.Maps.MapControl.WPF.Location pinLocation = MainMap.ViewportPointToLocation(mousePosition);
            Shared.ModelView.UserOrganizations.Default.ClickedLocation = new Shared.Model.Location(pinLocation.Latitude, pinLocation.Longitude);
            NewOfficeOnMap.Children.Clear();
            //
            NewOfficeOnMap.AddChild(Shared.View.MapsSymbols.NewOffice(), pinLocation, new Point(-3,-5));
            
            Cursor = Cursors.Arrow;
            MainMap.MouseDoubleClick -= AddOfficeToMap;
            Shared.View.General.ShowElement(ConfirmKey);
            NewOfficeOnMap.Tag = pinLocation.Longitude.ToString() + ';' + pinLocation.Latitude.ToString();

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
#if DEBUG
                    var o = Shared.ModelView.UserOrganizations.Default.NewOfficeName;
#endif
                    if (await Shared.ViewModel.Organization.CreateOffice(Shared.ModelView.UserOrganizations.Default.SelectedOrganization, Shared.ModelView.UserOrganizations.Default.NewOfficeName, Shared.ModelView.UserOrganizations.Default.ClickedLocation.Longitude, Shared.ModelView.UserOrganizations.Default.ClickedLocation.Latitude))
                    {
                        Shared.View.General.HideElements(new List<FrameworkElement> { WorkEmailGreeting, WorkMail, SendKeyRegistration, ExtendCompanyGrid, NewOfficeOnMap });
                        Shared.Actions.refreshOffices();
                        //await Shared.ViewModel.Organization.LoadOffices(OfficesOnMap);
                    };
                }
            }

        }

        private void StartSetHomeOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, SetHomeBut);
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;
            Cursor = Cursors.Cross;
            MainMap.MouseDoubleClick += SetHomeOnMap;
        }

        private async void SetHomeOnMap(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);

            UIBinding.Default.HomeLocation= (Shared.Model.Location)MainMap.ViewportPointToLocation(mousePosition);

            await Shared.ViewModel.UserHome.SetHome();
            SetHomeBut.IsChecked = false;
            CheckWizard();
        }

        private void HideGroupsGrid(object sender, RoutedEventArgs e)
        {
            Shared.View.General.HideLeftMenu(GroupsGrid, AutoCollapsedElements);
        }

        private void StartWriteMyRoutesOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons,WriteRouteBut);
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
            StartWriteRoute(sender, e);
            Shared.View.PathControl.ShowPathControl(PathControl);
        }

        private void CompleteWritingRoutes(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MainMap.MouseDoubleClick -= SetRoutePoint;
        }


        private async void SetRoutePoint(object sender, MouseButtonEventArgs e)
        {
            var s = Shared.ModelView.UIBinding.Default.SelectedPath;
            var SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == s);

            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);
            Microsoft.Maps.MapControl.WPF.Location pinLocation = MainMap.ViewportPointToLocation(mousePosition);
            var RoutePoint = Shared.View.MapsSymbols.RoutePoint(SelectedPath.IsToHome, RoutesOnMap, Brushes.DodgerBlue);

            RoutesOnMap.AddChild(RoutePoint, pinLocation, new Point(-3, -5));
            var RoutePointID = await Shared.ViewModel.RoutePoints.Save(pinLocation.Longitude, pinLocation.Latitude, SelectedPath.IsToHome, SelectedPath.PathID, SelectedPath.Actuality);
            if (RoutePointID == 0) RoutesOnMap.Children.Remove(RoutePoint);
            else RoutePoint.Tag = RoutePointID;
            if (SelectedPath.IsToHome) {
                AppBar.Visibility = Visibility.Visible;
                WizardAssistant.Visibility = Visibility.Collapsed;
            }

        }

        private void StartWriteRoute(object sender, RoutedEventArgs e)
        {
            MainMap.MouseDoubleClick -= SetRoutePoint;
            Cursor = Cursors.Cross;
           
            MainMap.MouseDoubleClick += SetRoutePoint;
        }


        private void StartFindCompanions(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, FindCompanionsBut);

            Shared.ViewModel.Companions.FindCompanions(CompanionsOnMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Now);


            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;

        }

        private void SearchCompanionsToHome(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = true;
            if (isProfileLoaded) Shared.ViewModel.Companions.FindCompanions(CompanionsOnMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Now);
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

        private void StartSelectCarColor(object sender, EventArgs e)
        {
            Shared.View.Car.ShowCarColors(CarColorSelector);
            CarColorSelector.SelectionChanged += CarColorChanged;
        }

        private async void CarColorChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectetColor = ((ComboBoxItem)CarColorSelector.SelectedItem).Content.ToString();
                if (Shared.ModelView.UserCar.Default.Color != selectetColor)
                {
                    int ColorID = (int.Parse(((ComboBoxItem)CarColorSelector.SelectedItem).Tag.ToString()));
                    Shared.ModelView.UserCar.Default.ColorID = ColorID;
                    Shared.ModelView.UserCar.Default.Color = ((ComboBoxItem)CarColorSelector.SelectedItem).Content.ToString();

                    await Shared.ViewModel.Car.UpdateCar();
                    CarColorSelector.SelectionChanged -= CarColorChanged;
                }
            }
            catch { }
        }

        private void StartSelectCarBrand(object sender, EventArgs e)
        {

            Shared.View.Car.ShowCarBrands(CarBrandSelector);
            CarBrandSelector.SelectionChanged += ResetCarModel;
        }

        private async void ChangeDriverMode(object sender, RoutedEventArgs e)
        {
            await Shared.ViewModel.UserProfile.ChangeDriverMode();
        }

        private void FinalSelectCarBrand(object sender, EventArgs e)
        {
            CarBrandSelector.SelectionChanged -= ResetCarModel;
        }

        private async void SelectCarModel(object sender, SelectionChangedEventArgs e)
        {
            if (isProfileLoaded)
            {
                try
                {
                    int ModelID = (int.Parse(((ComboBoxItem)CarModelSelector.SelectedItem).Tag.ToString()));
                    Shared.ModelView.UserCar.Default.ModelID = ModelID;
                    await Shared.ViewModel.Car.UpdateCar();
                }
                catch { }
            }
        }

        private async void UpdateCarNumber(object sender, KeyboardFocusChangedEventArgs e)
        {
            await Shared.ViewModel.Car.UpdateCar();
        }

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

        private void EndFindCompanions(object sender, RoutedEventArgs e)
        {
            CompanionsOnMap.Children.Clear();
            if(Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
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

        private void EndSetHomeOnMap(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MainMap.MouseDoubleClick -= SetHomeOnMap;
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
        }

        //private async void UpdateSchedule(object sender, RoutedEventArgs e)
        //{
        //    if (ScheduleGrid.Visibility == Visibility.Visible && isProfileLoaded) await Shared.ViewModel.UserSchedule.Update(int.Parse(((CheckBox)sender).Tag.ToString()));
        //}

        private void StartSetCurrentPositionOnMap(object sender, RoutedEventArgs e)
        {
            ResetUI();
            Shared.View.General.UnCheckButton(Buttons, SetCurrentPositionBut);
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Collapsed;
            Cursor = Cursors.Cross;
            MainMap.MouseDoubleClick += SetCurrentUserPositionOnMap;
        }

        private void EndSetCurrentPositionOnMap(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MainMap.MouseDoubleClick -= SetCurrentUserPositionOnMap;
            if (Shared.ModelView.UserProfile.Default.IsDriver) Shared.ModelView.UIBinding.Default.DriverRoutesVisibility = Visibility.Visible;
        }

        private async void SetCurrentUserPositionOnMap(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition((UIElement)sender);

            UIBinding.Default.CurrentUserPosition = (Shared.Model.Location)MainMap.ViewportPointToLocation(mousePosition);

            await Shared.ViewModel.UserLocation.UpdateLocation();
            SetCurrentPositionBut.IsChecked = false;
        }

        private void OpenPrivacyPolicy(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://unicarbuddy.ru/about/PrivacyPolicy");
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

        private async void AddNewPath(object sender, RoutedEventArgs e)
        {
            var latestPath = Shared.ModelView.UIBinding.Default.Routes.OrderBy(req => req.PathID).Last();
            var newDirection=await Shared.ViewModel.RoutePoints.AddDirecation(!latestPath.IsToHome);
            var weekactual = new Shared.Model.WeekActuality { Monday = true, Friday = true, Saturday = false, Sunday = false, Thursday = true, Tuesday = true, Wednesday = true };
            var newRoute = new Shared.Model.Direction { IsFri = weekactual.Friday, IsMon = weekactual.Monday, IsSat = weekactual.Saturday, IsSun = weekactual.Sunday, IsThu = weekactual.Thursday, IsToHome = !latestPath.IsToHome, IsTue = weekactual.Tuesday, IsWed = weekactual.Wednesday, PathID = newDirection, Actuality = weekactual };
            var temparr = Shared.ModelView.UIBinding.Default.Routes;
            temparr.Add(newRoute);
            Shared.ModelView.UIBinding.Default.Routes = temparr;
            Shared.View.PathControl.ShowPathControl(PathControl);
        }

        private async void AutoSaveProfile(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Grid = sender as Grid;
            if(Grid.Visibility==Visibility.Collapsed && Shared.ModelView.UIBinding.Default.isProfileChanged)   await Shared.ViewModel.UserProfile.Update();
        }

        private async void StartIntroJoinOrg(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.StartJoin(IntroOrgBox.Text.Replace(" ", "")))
            {
                Shared.View.General.HideElement(OrgJoinIntro);
                Shared.View.General.ShowElement(OrgJoinConfrimationStack);
            }
            else{
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

        private async void LoadedIntroOrgJoin(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IntroJoinToOrganizations.Visibility == Visibility.Visible && Shared.ModelView.UIBinding.Default.AutoJoinVisibility == Visibility.Visible)
            {
                ManualJoinOrgStackPanel.Visibility = Visibility.Collapsed;
                if (await Shared.ViewModel.Organization.AutoJoin("unicarbuddy@icl-services.com")) {
                    Shared.View.General.ShowElement(SuccessStoryLabel);
                    Shared.View.General.ShowElement(OrgJoinComplete);
                }
                else ManualJoinOrgStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void ContinueIntro(object sender, RoutedEventArgs e)
        {
            MainWND.Width = 1280;
            MainWND.Height = 720;
            Shared.View.General.HideElements(MainUIs);
            MainGrid.Visibility = Visibility.Visible;
            CheckWizard();

        }
    }
    
}
