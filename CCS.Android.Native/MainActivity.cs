using Android.App;
using Android.Widget;
using Android.OS;
using Shared;
using Shared.Model;
using Microsoft.AspNet.SignalR.Client;
using Android.Views;
using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Common;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Runtime;
using Android.Content;
using Firebase.Iid;
using WindowsAzure.Messaging;
//using Android.Util;

namespace CCS.Android.Native
{
    [Activity]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        public const string TAG = "MainActivity";

        public static readonly int InstallGooglePlayServicesId = 1000;
        private GoogleMap _map;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CheckPush();

            Window.RequestFeature(WindowFeatures.NoTitle);
            //Toast.MakeText(this, "started", ToastLength.Long).Show();
            SetContentView(Resource.Layout.Main);
            InitializeLocationManager();
            try
            {
                TestIfGooglePlayServicesIsInstalled(); 
            }
            catch { }

            InitializeUI();


        }

        void CheckPush() {
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        //Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            var mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFrag.GetMapAsync(this);
            LaunchControl();
        }

        private void MoveToLocation()
        {
            try
            {
                LatLng tlocation = new LatLng(Shared.ModelView.UIBinding.Default.CurrentUserPosition.Latitude, Shared.ModelView.UIBinding.Default.CurrentUserPosition.Longitude);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(tlocation);
                builder.Zoom(13);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                _map.MoveCamera(cameraUpdate);
            }
            catch { }
        }

        private void SetupMapIfNeeded()
        {
            if (_map == null)
            {
                if (_map != null)
                {
                    // We create an instance of CameraUpdate, and move the map to it.
                    //CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(VimyRidge, 15);
                    //_map.MoveCamera(cameraUpdate);
                }
            }
        }

        LocationManager _locationManager;

        string _locationProvider;


        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Low
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            RewriteMap();


        }
        //protected override void on

        protected override void OnPause()
        {
            base.OnPause();
        }

        private bool TestIfGooglePlayServicesIsInstalled()
        {
            try
            {
                int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
                if (queryResult == ConnectionResult.Success)
                {
                    return true;
                }
                if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
                {
                    var dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Problem with Google play");
                    alert.SetMessage("Google play service is not working on your device");
                    alert.SetButton("Close app", (c, ev) =>
                    {
                        System.Environment.Exit(0);
                    });
                    alert.Show();
                }
                return false;
            }
            catch {
                var dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Problem with Google play");
                alert.SetMessage("Google play service is not working properly on your device");
                alert.SetButton("Close app", (c, ev) =>
                {
                    System.Environment.Exit(0);
                });
                alert.Show();
                return false;
            }
        }

        NotificationHub hub;
        public void SendRegistrationToServer()
        {
            Task.Run(() =>
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                // Register with Notification Hubs
                hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);
                var tags = new List<string>() { Shared.Model.LocalStorage.UID.ToString() };
                try
                {
                    hub.Register(refreshedToken, tags.ToArray());
                }
                catch (Exception e)
                {

                }
            });

            //Log.Debug(TAG, $"Successful registration of ID {regID}");
        }

        async void LaunchControl()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            if (await Log.On())
            {
                if (await Shared.ViewModel.UserProfile.Load(progressbar,output))
                {
                    if (await LoadOrganizations())
                    {
                        LoadHome();
                        LoadUserOffice();
                        LoadRoutes();
                        LoadTrips();
                        SendRegistrationToServer();
                    }
                    ConnectAsync();
                }
            }
            else RestoreAccess();
            InitMap();
            await Task.Delay(2500);
            RewriteMap();
            CheckHomeIsInPlace();
        }

        private void LoadTrips()
        {
            RunOnUiThread(async () =>
            {
                var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
                var output = FindViewById<TextView>(Resource.Id.MainOutPut);
                if (await Shared.ViewModel.Trips.LoadTrips(progressbar, output))
                {
                    Shared.View.TripOffer.ShowTripsOnMap(_map);
                }
            });
        }

        private async void LoadRoutes()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            Shared.View.General.inLoading(progressbar, output);
            var apiflow = await Shared.Model.Requests.LoadUserRoutePoints();
            Shared.View.General.outLoading(progressbar);

            if (apiflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.Routes = apiflow.Item2;
            }
            else
            {
                output.Text = ConvertMessages.Message("x50000");
            }
        }
            
        private async void LoadUserOffice()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);

            if (Shared.ModelView.UserProfile.Default.OfficeID != -1)
            {
                if (!await Shared.ViewModel.Organization.LoadOffices(progressbar, output, true)) {
                    StartActivity(typeof(SelectOfficeActivity));
                    this.Finish();
                }
            }
            else
            {
                StartActivity(typeof(SelectOfficeActivity));
                this.Finish();

            }
        }
        bool homeisloaded = false;
        private async void LoadHome()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            if (await Shared.ViewModel.UserHome.LoadHome(progressbar, output)) {
                homeisloaded = true;
            }
        }

        void CheckHomeIsInPlace() {

            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0)
            {
                if (homeisloaded)
                {
                    StartActivity(typeof(SetHomeActivity));
                    this.Finish();
                    return;
                }
            }
        }

        private void SendSignalNewMessage(int obj)
        {
            try
            {
                HubProxy.Invoke("SendMessageToUser", obj);
            }
            catch { }
        }

        private void UpdateInformationAboutTrips(int obj)
        {
            try
            {
                HubProxy.Invoke("UpdateTripToUser", obj);
            }
            catch { }
        }

        public IHubProxy HubProxy { get; set; }
#if DEBUG
        //const string ServerURI = "http://localhost:59524";
        const string ServerURI = "http://api.commutecarsharing.ru";
#else
        const string ServerURI = "http://api.commutecarsharing.ru";
#endif

        public HubConnection Connection { get; set; }
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
                    //new Runnable()
                    try
                    {
                        Shared.Actions.refreshMessages();
                    }
                    catch { }
                });
                HubProxy.On("UpdateTrip", () =>
                {
                    try
                    {
                        Shared.Actions.refreshTrips();
                        Shared.Actions.rewrite();
                        

                    }
                    catch { }
                });
                try
                {
                    try
                    {

                        await Connection.Start();
                    }
                    catch
                    {
                    }
                    await HubProxy.Invoke("Register", LocalStorage.UID);
                    try
                    {
                        Shared.Actions.SentMessage = SendSignalNewMessage;
                        Shared.Actions.UpdateTrip = UpdateInformationAboutTrips;
                    }
                    catch { }
                }
                catch
                {

                }
            }
            catch
            {
            }

        }


        private async Task<bool> LoadOrganizations()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            await Shared.ViewModel.Organization.LoadOrganizations(progressbar, output);
            if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count == 0) {
                StartActivity(typeof(OrganizationJoinActivity));
                this.Finish();
                return false;
            }
            else
            {
                return true;
            }

        }

        public void OnMapReady(GoogleMap map)
        {
            _map = map;
            _map.UiSettings.CompassEnabled = true;
            _map.UiSettings.ZoomGesturesEnabled = false;
            Shared.Actions.InitializeGPS = MoveToLocation;
            _map.CameraMove += _map_CameraMove;
        }

        private void _map_CameraMove(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(_map.CameraPosition.Target.Latitude, _map.CameraPosition.Target.Longitude);
        }

        //private void 

        void RewriteMap() {
            try
            {
                try
                {
                    _map.Clear();
                }
                catch
                {
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            _map.Clear();
                            MarkRoutes();
                            MarkHome();
                            MarkCurrentOffice();
                            //MarkTrips();                        
                        }
                        catch { }
                    });
                }
                try
                {
                    MarkRoutes();
                }
                catch { }
                try
                {
                    MarkHome();
                }
                catch { }
                try
                {
                    MarkCurrentOffice();
                }
                catch { }
                try
                {
                    MarkTrips();
                }
                catch { }
            }
            catch { }
        }

        private void MarkTrips()
        {
            try
            {
                Shared.View.TripOffer.ShowTripsOnMap(_map);
            }
            catch (Exception e) {

            }
        }



        private void MarkHome()
        {
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0)
            {
                MarkerOptions homeMarker = new MarkerOptions();
                
                homeMarker.SetPosition(new LatLng(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude));
                homeMarker.SetTitle("Home");
                homeMarker.InvokeZIndex(500);
                //homeMarker.SetSnippet("HomeX");
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.home);
                homeMarker.SetIcon(icon);
                _map.AddMarker(homeMarker);
            }
        }

        void MarkRoutes()
        {
            if (Shared.ModelView.UserProfile.Default.IsDriver)
            {
                foreach (var Route in Shared.ModelView.UIBinding.Default.Routes)
                {
                    //Random r = new Random();

                    foreach (var rp in Route.Points)
                    {
                        MarkerOptions point = new MarkerOptions();
                        point.SetPosition(new LatLng(rp.Latitude, rp.Longtitude));
                        //point.SetTitle(rp.PointID.ToString());
                        if (Route.IsToHome)
                        {
                            BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.r_home);
                            point.SetIcon(icon);
                        }
                        else
                        {
                            BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.r_work);
                            point.SetIcon(icon);
                        }
                        _map.AddMarker(point);
                    }
                }
            }
        }

        private void MarkCurrentOffice()
        {
            if (Shared.ModelView.UserOrganizations.Default.CurrentOffice.Latitude != 0 && Shared.ModelView.UserOrganizations.Default.CurrentOffice.Longtitude != 0)
            {
                MarkerOptions office = new MarkerOptions();
                office.SetPosition(new LatLng(Shared.ModelView.UserOrganizations.Default.CurrentOffice.Latitude, Shared.ModelView.UserOrganizations.Default.CurrentOffice.Longtitude));
                office.SetTitle(Shared.ModelView.UserOrganizations.Default.CurrentOffice.Name);
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.office);
                office.InvokeZIndex(600);
                office.SetIcon(icon);
                _map.AddMarker(office);
            }
        }


        void InitializeUI()
        {
            Shared.Actions.refreshTrips = LoadTrips;
            Shared.Actions.rewrite = RewriteMap;
            var openprofilebut = FindViewById<Button>(Resource.Id.openprofile);
            Shared.View.General.SetSegoeMDLFont(openprofilebut, this.Assets);
            openprofilebut.Click += (ev, ar) =>
            {
                StartActivity(typeof(ProfileActivity));
            };
            var OpenWeeklySchedule = FindViewById<Button>(Resource.Id.OpenWeeklySchedule);
            Shared.View.General.SetSegoeMDLFont(OpenWeeklySchedule, this.Assets);
            OpenWeeklySchedule.Click += (ev, ar) => {
                StartActivity(typeof(OpenSchedulesActivity));
            };

            var OpenChat = FindViewById<Button>(Resource.Id.OpenChat);

            Shared.View.General.SetSegoeMDLFont(OpenChat, this.Assets);
            OpenChat.Click += (ev, ar) => {
                StartActivity(typeof(ChatActivity));
            };
            var OpenTrips = FindViewById<Button>(Resource.Id.OpenTrips);
            Shared.View.General.SetSegoeMDLFont(OpenTrips, this.Assets);
            OpenTrips.Click += (ev, ar) =>
            {
                StartActivity(typeof(TripsActivity));
            };

            var OpenOrganization = FindViewById<Button>(Resource.Id.OpenOrganization);
            Shared.View.General.SetSegoeMDLFont(OpenOrganization, this.Assets);
            OpenOrganization.Click += (ev, ar) =>
            {
                StartActivity(typeof(OpenOrganizationsActivity));
            };
            var StartSearchCompanions = FindViewById<ToggleButton>(Resource.Id.StartSearchCompanions);
            Shared.View.General.SetSegoeMDLFont(StartSearchCompanions, this.Assets);
            StartSearchCompanions.CheckedChange += StartSearchCompanions_Click;

            var GoToGPS = FindViewById<Button>(Resource.Id.GoToGPS);
            Shared.View.General.SetSegoeMDLFont(GoToGPS, this.Assets);

            GoToGPS.Click += GoToGPS_Click;

            var SearchMode = FindViewById<ToggleButton>(Resource.Id.SearchMode);
            SearchMode.CheckedChange += SearchMode_CheckedChange;
            SetupZoomInButton();
            SetupZoomOutButton();
        }

        private void StartSearchCompanions_Click(object sender, EventArgs e)
        {
            var IsCheked = (ToggleButton)sender;
            var SearchMode = FindViewById<ToggleButton>(Resource.Id.SearchMode);
            if (IsCheked.Checked)
            {
                var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
                var output = FindViewById<TextView>(Resource.Id.MainOutPut);
                Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = true;
                Shared.ViewModel.Companions.FindCompanions(_map, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Now, progressbar, output);
                SearchMode.Visibility = ViewStates.Visible;
                _map.InfoWindowClick += _map_InfoWindowClick; 

            }
            else {
                SearchMode.Visibility = ViewStates.Gone;
                RewriteMap();
                _map.InfoWindowClick -= _map_InfoWindowClick;
            }
        }

        private void SearchMode_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var SearchMode = (ToggleButton)sender;
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = SearchMode.Checked;
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            Shared.ViewModel.Companions.FindCompanions(_map, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Now, progressbar, output);

        }

        private async void _map_InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            var Info = e.Marker.Tag.ToString().Split(';');
            var UID = int.Parse(Info[0]);
            var isHome = bool.Parse(Info[1]);
            var PointId = int.Parse(Info[2]);
            await Shared.ViewModel.Trips.Send(progressbar, output, UID, PointId, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome);
        }

        private void GoToGPS_Click(object sender, EventArgs e)
        {
            LatLng tlocation = new LatLng(Shared.ModelView.UIBinding.Default.CurrentUserPosition.Latitude, Shared.ModelView.UIBinding.Default.CurrentUserPosition.Longitude);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(tlocation);
            builder.Zoom(13);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            _map.MoveCamera(cameraUpdate);
        }

        private void SetupZoomInButton()
        {
            Button zoomInButton = FindViewById<Button>(Resource.Id.ZoomIN);
            zoomInButton.Click += (sender, e) => {
                _map.AnimateCamera(CameraUpdateFactory.ZoomIn());
                
            };
        }
        
        private void SetupZoomOutButton()
        {
            Button zoomOutButton = FindViewById<Button>(Resource.Id.ZoomOut);
            zoomOutButton.Click += (sender, e) => { _map.AnimateCamera(CameraUpdateFactory.ZoomOut()); };
        }

        private void InitMap()
        {            
            GeoIP.GetGeoIP();
            checkLocationPermission();
            //_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);


            //await Shared.ViewModel.UserLocation.LoadLocation();
            //Shared.Actions.initializeMap();
            //try
            //{
            //    var locator = CrossGeolocator.Current;
            //    await locator.StartListeningAsync(new TimeSpan(0, 0, 10), 100);
            //    var position = await locator.GetPositionAsync();
            //    Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(position.Latitude, position.Longitude);
            //    await Shared.ViewModel.UserLocation.UpdateLocation();
            //    await locator.StopListeningAsync();
            //    Shared.Actions.initializeMap();
            //}
            //catch (Exception)
            //{
            //}

        }

        public const int MY_PERMISSIONS_REQUEST_LOCATION = 99;

        public void checkLocationPermission()
        {
            var GoToGPS = FindViewById<Button>(Resource.Id.GoToGPS);
            try
            {
                
                if (ActivityCompat.CheckSelfPermission(this,Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    
                    GoToGPS.Visibility = ViewStates.Gone;
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
                    {
                        var dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
                        alert.SetTitle("Access Fine Location");
                        alert.SetMessage("Grant access");
                        alert.SetButton("Grant", (c, ev) =>
                        {
                            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, MY_PERMISSIONS_REQUEST_LOCATION);
                            _locationManager.RequestLocationUpdates(_locationProvider, 12000, 1500, this);
                        });
                        alert.Show();

                    }
                    else
                    {
                        ActivityCompat.RequestPermissions(this,new String[] { Manifest.Permission.AccessFineLocation }, MY_PERMISSIONS_REQUEST_LOCATION);
                        _locationManager.RequestLocationUpdates(_locationProvider, 12000, 1500, this);
                    }
                }
                else
                {
                    _locationManager.RequestLocationUpdates(_locationProvider, 12000, 1500, this);
                }
            }
            catch (Exception e)
            {
                GoToGPS.Visibility = ViewStates.Gone;
            }

        }



        async void RestoreAccess()
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.MainprogressBar);
            var output = FindViewById<TextView>(Resource.Id.MainOutPut);
            if (!await Shared.ViewModel.Registration.RestoreAccess(progressbar, output))
            {
                Shared.Actions.showRegistrationGrid();
            }
            else
            {
                LaunchControl();
            }
        }

  




 
        public void OnLocationChanged(global::Android.Locations.Location location)
        {
            Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(location.Latitude, location.Longitude);
            var GoToGPS = FindViewById<Button>(Resource.Id.GoToGPS);
            if (location.Latitude != 0 && location.Longitude != 0)
            {                
                GoToGPS.Visibility = ViewStates.Visible;
            }
            else {
                GoToGPS.Visibility = ViewStates.Gone;
            }
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}

