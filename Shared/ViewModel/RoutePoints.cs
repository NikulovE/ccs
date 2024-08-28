
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#else
using System.Windows;

#if WPF
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Media;
#endif

#if XAMARIN
using Xamarin.Forms.Maps;
#endif
#endif

namespace Shared.ViewModel
{
    class RoutePoints
    {
        public static async Task<int> Save(double longtitude, double latitude, bool way, int PathID, Model.WeekActuality Actuality)
        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.SaveRoutePoint(longtitude, latitude, PathID);
            Shared.View.General.outLoading();
            if (apiflow.Item1 == true)
            {
                return apiflow.Item2;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                return 0;
            }
        }

#if XAMARIN
        public static async Task<bool> Load(Map RoutesLayer)
#else
#if WINDOWS_UWP
        public static async Task<bool> Load(MapControl RoutesLayer)
#else
        public static async Task<bool> Load(MapLayer RoutesLayer)
#endif
#endif

        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.LoadUserRoutePoints();
            Shared.View.General.outLoading();

            if (apiflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.Routes = apiflow.Item2;
                foreach (var Route in apiflow.Item2)
                {

#if XAMARIN
#else
                    Random r = new Random();
                    var RoutePointsBrush = new SolidColorBrush(Color.FromArgb(255, 30, (byte)r.Next(50, 200), 255));
#endif
                    foreach (var rp in Route.Points)
                    {
                        try
                        {
#if XAMARIN                          
#else
#if WINDOWS_UWP
                            var RoutePoint = View.MapsSymbols.RoutePoint(rp.IsHome, RoutesLayer, RoutePointsBrush);
                            RoutePoint.Tag = rp.PointID;
                            MapControl.SetLocation(RoutePoint, new Shared.Model.Location { Latitude = rp.Latitude, Longitude = rp.Longtitude });
#else                            
                            var RoutePoint = View.MapsSymbols.RoutePoint(Route.IsToHome, RoutesLayer, RoutePointsBrush);
                            RoutePoint.Tag = rp.PointID;
                            MapLayer.SetPosition(RoutePoint, new Location { Latitude = rp.Latitude, Longitude = rp.Longtitude });
#endif
                            RoutesLayer.Children.Add(RoutePoint);
#endif
                            
                            await Task.Delay(50);
                        }
                        catch (Exception)
                        { }
                    }
                }
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                return false;

            }
        }
#if XAMARIN
        public static async void RefreshPath(Map RoutesLayer, int SelectedPath)
#else
#if WINDOWS_UWP
        public static async void RefreshPath(MapControl RoutesLayer, int SelectedPath)
#else
        public static void RefreshPath(MapLayer RoutesLayer)
#endif
#endif
        {
            RoutesLayer.Children.Clear();
            foreach (var Route in Shared.ModelView.UIBinding.Default.Routes)
            {

#if XAMARIN
#else
                Random r = new Random();
                var RoutePointsBrush = new SolidColorBrush(Color.FromArgb(255, 30, (byte)r.Next(50, 200), 255));
#endif
                foreach (var rp in Route.Points)
                {
                    try
                    {
#if XAMARIN
#else
#if WINDOWS_UWP
                            var RoutePoint = View.MapsSymbols.RoutePoint(rp.IsHome, RoutesLayer, RoutePointsBrush);
                            RoutePoint.Tag = rp.PointID;
                            MapControl.SetLocation(RoutePoint, new Shared.Model.Location { Latitude = rp.Latitude, Longitude = rp.Longtitude });
#else
                        var RoutePoint = View.MapsSymbols.RoutePoint(Route.IsToHome, RoutesLayer, RoutePointsBrush);
                        RoutePoint.Tag = rp.PointID;
                        MapLayer.SetPosition(RoutePoint, new Location { Latitude = rp.Latitude, Longitude = rp.Longtitude });
#endif
                        RoutesLayer.Children.Add(RoutePoint);
#endif

                    }
                    catch (Exception)
                    { }
                }
            }
            }

        public static async Task<bool> Change(int RoutePointID, int SysCode)
        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.ChangeRoutePoint(SysCode, RoutePointID);
            Shared.View.General.outLoading();
            if (apiflow.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public static async Task<int> AddDirecation(bool isToHome)
        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.AddDirection(isToHome);
            Shared.View.General.outLoading();
            if (apiflow.Item1 == true)
            {
                return apiflow.Item2;
            }
            else
            {
                return -1;

            }
        }

        public static async Task<bool> ChangePath(int PathID, int SysCode, bool refresh = true, string newname = "")
        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.ChangePath(SysCode, PathID, newname);
            Shared.View.General.outLoading();
            if (apiflow.Item1 == true) { 

                try
                {
                    Actions.refreshRoutePoints();                    
                }
                catch { };
                return true;

            }
            else
            {
                return false;

            }
        }
    }
}
