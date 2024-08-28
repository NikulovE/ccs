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
    [Activity(Label = "Set Home")]
    public class SetHomeActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap _map;

        public void OnMapReady(GoogleMap map)
        {
            _map = map;
            _map.UiSettings.CompassEnabled = true;
            _map.UiSettings.ZoomGesturesEnabled = false;
            Shared.Actions.InitializeGPS = MoveToLocation;
            _map.CameraMove += _map_CameraMove;
            MarkHome();
            Shared.Actions.InitializeGPS();
        }

        private void _map_CameraMove(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(_map.CameraPosition.Target.Latitude, _map.CameraPosition.Target.Longitude);
        }

        private async void _map_MapLongClick(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            Shared.ModelView.UIBinding.Default.HomeLocation.Latitude = e.Point.Latitude;
            Shared.ModelView.UIBinding.Default.HomeLocation.Longitude = e.Point.Longitude;
            MarkHome();
            if (await Shared.ViewModel.UserHome.SetHome()) {
                CheckThatHouseIsThere();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetHomePage);
            InitMap();
            initUI();
        }


        private void CheckThatHouseIsThere() {
            var state = FindViewById<TextView>(Resource.Id.StateHome);
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0)
            {                
                state.Text = "House location is identified. You can set home by long click";
                var continuebut = FindViewById<Button>(Resource.Id.Continue);
                continuebut.Visibility = ViewStates.Visible;
                continuebut.Click += (ev, ar) =>
                {
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                };
            }
            else{
                state.Text = "set home by long click";
            }
        }


        protected override void OnResume()
        {
            base.OnResume();
        }


        void initUI() {
            Shared.Actions.HomeLoaded = MarkHome;
            SetupZoomInButton();
            SetupZoomOutButton();
        }

        private void MarkHome()
        {
            _map.MapLongClick += _map_MapLongClick;
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0)
            {
                MarkerOptions homeMarker = new MarkerOptions();
                homeMarker.SetPosition(new LatLng(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude));
                homeMarker.SetTitle("Home");
                var icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.home);
                homeMarker.SetIcon(icon);
                _map.Clear();
                _map.AddMarker(homeMarker);               
                
            }
            CheckThatHouseIsThere();
        }
      

        private void InitMap()
        {
            var mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlehomemap);
            mapFrag.GetMapAsync(this);
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
        private void MoveToLocation()
        {
            try
            {
                LatLng tlocation = new LatLng(Shared.ModelView.UIBinding.Default.CurrentUserPosition.Latitude, Shared.ModelView.UIBinding.Default.CurrentUserPosition.Longitude);
                if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0)
                {
                    tlocation = new LatLng(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude);
                }
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(tlocation);
                builder.Zoom(13);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                _map.MoveCamera(cameraUpdate);
            }
            catch { }
        }

        
    }
}