using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace CCS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildRoutes : ContentPage
    {
        List<int> removed = new List<int>();
        public BuildRoutes()
        {
            InitializeComponent();
            Shared.Actions.initializeMap = initmap;
            Shared.Actions.initializeMap();            
        }


        private async void initmap()
        {
            if (Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude != 0 && Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude != 0)
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude), Distance.FromMiles(1)));
            }
            await Shared.ViewModel.RoutePoints.Load(Map);
            ReinintRoutePoints();

        }

        private void PutRPToHome(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.IsToHome == true).PathID;
            Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(Map.VisibleRegion.Center.Latitude, Map.VisibleRegion.Center.Longitude);
            SetRoutePoint();
        }

        private void PutRPToWork(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.IsToHome == false).PathID;
            Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(Map.VisibleRegion.Center.Latitude, Map.VisibleRegion.Center.Longitude);
            SetRoutePoint();
        }
        void ReinintRoutePoints()
        {
            foreach (var Route in Shared.ModelView.UIBinding.Default.Routes)
            {
                foreach (var rp in Route.Points)
                {
                    try
                    {
                        var Pos = new Position(rp.Latitude, rp.Longtitude);
                        var RoutePoint = new Pin { Position = Pos };
                        RoutePoint.Label = "Click to remove point";
                        RoutePoint.Address = rp.Way ? "to home" : "to work";
                        RoutePoint.Clicked += (ev, ar) =>
                        {
                            RemovePoint(rp.PointID);
                            Map.Pins.Clear();
                            ReinintRoutePoints();
                        };
                        if(!removed.Contains(rp.PointID)) Map.Pins.Add(RoutePoint);
                    }
                    catch (Exception) { }
                }
            }
        }

        async void SetRoutePoint()
        {
            var pos = new Position(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude);
            var s = Shared.ModelView.UIBinding.Default.SelectedPath;
            var SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == s);


            var RoutePoint = new Pin { Position = pos };
            var RoutePointID = await Shared.ViewModel.RoutePoints.Save(pos.Longitude, pos.Latitude, SelectedPath.IsToHome, SelectedPath.PathID, SelectedPath.Actuality);
            RoutePoint.Label = "Click to remove point "+RoutePointID.ToString();
            RoutePoint.Label = SelectedPath.IsToHome ? "to home" : "to work";
            RoutePoint.Clicked += (ev, ar) =>
             {
                 RemovePoint(RoutePointID);
                 Map.Pins.Clear();
                 ReinintRoutePoints();
             };
            Map.Pins.Add(RoutePoint);
            

        }

        async void RemovePoint(int PointID) {

            removed.Add(PointID);
            await Shared.ViewModel.RoutePoints.Change(PointID, -1);
        }


    }
}