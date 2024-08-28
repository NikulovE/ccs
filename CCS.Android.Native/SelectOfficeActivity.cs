using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Locations;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;

namespace CCS.Android.Native
{
    [Activity(Label = "Select office")]
    public class SelectOfficeActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap _map;

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            _map.UiSettings.CompassEnabled = true;
            _map.UiSettings.ZoomGesturesEnabled = false;
            MarkHome();
            MoveToLocation();            
            loadOffices(_map);
            _map.InfoWindowClick += _map_InfoWindowClick;//+= _map_MarkerClick;// 
            _map.CameraMove += _map_CameraMove;
        }


        private async void LoadHome()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressbar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            if (await Shared.ViewModel.UserHome.LoadHome(progressbar, output))
            {
                
            }
        }

        void CheckHomeIsInPlace()
        {
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0)
            {
                StartActivity(typeof(SetHomeActivity));
                this.Finish();
                return;
            }
        }

        private void _map_CameraMove(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(_map.CameraPosition.Target.Latitude, _map.CameraPosition.Target.Longitude);
        }

        private async void _map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            Shared.ModelView.UserProfile.Default.OfficeID = Convert.ToInt32(e.Marker.ZIndex);
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            if (await Shared.ViewModel.UserProfile.Update(progressBar, output))
            {
                var continuebut = FindViewById<Button>(Resource.Id.Continue);
                continuebut.Visibility = ViewStates.Visible;
                continuebut.Click += (ev, ar) =>
                {
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                };
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectOffice);
            LoadHome();
            InitMap();
        }

        private void InitMap()
        {
            var mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googleofficemap);
            mapFrag.GetMapAsync(this);
            SetupZoomInButton();
            SetupZoomOutButton();
        }


        private void SetupZoomInButton()
        {
            Button zoomInButton = FindViewById<Button>(Resource.Id.ZoomIN);
            zoomInButton.Click += (sender, e) =>
            {
                _map.AnimateCamera(CameraUpdateFactory.ZoomIn());
            };
        }

        private void SetupZoomOutButton()
        {
            Button zoomOutButton = FindViewById<Button>(Resource.Id.ZoomOut);
            zoomOutButton.Click += (sender, e) => {
                _map.AnimateCamera(CameraUpdateFactory.ZoomOut());
            };
        }

        async void loadOffices(GoogleMap map) {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.Organization.LoadOffices(progressBar, output);
            foreach(var office in Shared.ModelView.UserOrganizations.Default.Offices)
            {
                MarkerOptions nextoffice = new MarkerOptions();
                nextoffice.SetPosition(new LatLng(office.Latitude, office.Longtitude));
                nextoffice.SetTitle(office.Name);
                nextoffice.SetSnippet("Click to select");
                nextoffice.InvokeZIndex(Convert.ToSingle(office.ID));
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.office);
                nextoffice.SetIcon(icon);
                _map.AddMarker(nextoffice);
            }
            if (Shared.ModelView.UserOrganizations.Default.Offices.Count > 0)
            {
                ShowTip();
            }
            else {
                var state = FindViewById<TextView>(Resource.Id.Tips);
                state.Text = "There is no office near your home";
                var addoffice = FindViewById<Button>(Resource.Id.add);
                var officename = FindViewById<EditText>(Resource.Id.OfficeName);
                addoffice.Visibility = ViewStates.Visible;
                officename.Visibility = ViewStates.Visible;
                addoffice.Click += (ev, ar) =>
                {
                    _map.MapLongClick += _map_MapLongClick;
                    if (officename.Text == "") {
                        state.Text = "Firstly input officename";
                    }

                };
            }
        }

        private async void _map_MapLongClick(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.Organization.CreateOffice(Shared.ModelView.UserOrganizations.Default.OrganizationsList[0].TeamID, FindViewById<EditText>(Resource.Id.OfficeName).Text, e.Point.Longitude, e.Point.Latitude, progressBar, output);
        }

        private void ShowTip()
        {
            var state = FindViewById<TextView>(Resource.Id.Tips);
            state.Text = "Click on an office";
        }

        private void MarkHome()
        {
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0)
            {
                MarkerOptions homeMarker = new MarkerOptions();
                homeMarker.SetPosition(new LatLng(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude));
                homeMarker.SetTitle("Home");
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.home);
                homeMarker.SetIcon(icon);
                _map.Clear();
                _map.AddMarker(homeMarker);

            }
            CheckHomeIsInPlace();
        }

        private async void _map_InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            Shared.ModelView.UserProfile.Default.OfficeID = Convert.ToInt32(e.Marker.ZIndex);
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            if (await Shared.ViewModel.UserProfile.Update(progressBar, output)) {
                var continuebut = FindViewById<Button>(Resource.Id.Continue);
                continuebut.Visibility = ViewStates.Visible;
                continuebut.Click += (ev, ar) =>
                {
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                };
            }

        }

        private void MoveToLocation()
        {
            try
            {
                LatLng tlocation = new LatLng(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(tlocation);
                builder.Zoom(12);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                _map.MoveCamera(cameraUpdate);
            }
            catch { }
            Shared.ModelView.UIBinding.Default.CurrentCenter = Shared.ModelView.UIBinding.Default.HomeLocation;
        }

    }
}