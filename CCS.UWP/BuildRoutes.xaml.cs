using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class BuildRoutes : Page
    {
        public BuildRoutes()
        {
            this.InitializeComponent();
            Shared.Actions.initializeMap = MoveMapToCenter;
            LoadRoutePoints();
            MoveMapToCenter();
            RoutesOnMap.MapDoubleTapped += RoutesOnMap_MapDoubleTapped;
            


        }

        private async void RoutesOnMap_MapDoubleTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            var s = Shared.ModelView.UIBinding.Default.SelectedPath;
            var SelectedPath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == s);

            var RoutePoint = Shared.View.MapsSymbols.RoutePoint(SelectedPath.IsToHome, RoutesOnMap, new SolidColorBrush(Colors.DodgerBlue));
            var RoutePointID = await Shared.ViewModel.RoutePoints.Save(args.Location.Position.Longitude, args.Location.Position.Latitude, SelectedPath.IsToHome, SelectedPath.PathID, SelectedPath.Actuality);
            if (RoutePointID == 0) RoutesOnMap.Children.Remove(RoutePoint);
            else
            {
                MapControl.SetLocation(RoutePoint, args.Location);
                RoutesOnMap.Children.Add(RoutePoint);
                RoutePoint.Tag = RoutePointID;

            }
        }

        private void MoveMapToCenter()
        {
            try
            {
                RoutesOnMap.Center = Shared.ModelView.UIBinding.Default.CurrentCenter;
                if (!App.isGPSInitialized)
                {
                    Shared.Actions.InitializeGPS();
                }
            }
            catch (Exception) { }
        }

        async void LoadRoutePoints() {
            await Shared.ViewModel.RoutePoints.Load(RoutesOnMap);
            Shared.View.PathControl.ShowPathControl(PathControl);
        }

        private void AddNewPath(object sender, RoutedEventArgs e)
        {
            var latestPath = Shared.ModelView.UIBinding.Default.Routes.OrderBy(req => req.PathID).Last();
            var weekactual = new Shared.Model.WeekActuality { Monday = true, Friday = true, Saturday = false, Sunday = false, Thursday = true, Tuesday = true, Wednesday = true };
            var newRoute = new Shared.Model.Direction { IsFri = weekactual.Friday, IsMon = weekactual.Monday, IsSat = weekactual.Saturday, IsSun = weekactual.Sunday, IsThu = weekactual.Thursday, IsToHome = !latestPath.IsToHome, IsTue = weekactual.Tuesday, IsWed = weekactual.Wednesday, PathID = (latestPath.PathID + 1), Actuality = weekactual };
            var temparr = Shared.ModelView.UIBinding.Default.Routes;
            temparr.Add(newRoute);
            Shared.ModelView.UIBinding.Default.Routes = temparr;
            Shared.View.PathControl.ShowPathControl(PathControl);
        }
    }
}
