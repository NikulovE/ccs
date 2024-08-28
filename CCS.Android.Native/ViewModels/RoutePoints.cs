using Android;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using System.Threading.Tasks;

namespace Shared.ViewModel
{
    class RoutePoints
    {
        public static async Task<int> Save(ProgressBar progressBar, TextView OutPut, double longtitude, double latitude, bool way, int PathID, Model.WeekActuality Actuality)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var apiflow = await Model.Requests.SaveRoutePoint(longtitude, latitude, PathID);
            Shared.View.General.outLoading(progressBar);
            if (apiflow.Item1 == true)
            {
                return apiflow.Item2;
            }
            else
            {
                OutPut.Text = ConvertMessages.Message("x50000");
                return 0;
            }
        }



        public static void RefreshPath(GoogleMap RoutesLayer)
        {
            RoutesLayer.Clear();
            foreach (var Route in Shared.ModelView.UIBinding.Default.Routes)
            {
                foreach (var rp in Route.Points)
                {
                }
            }
            }

        public static async Task<bool> Change(ProgressBar progressBar, TextView OutPut, int RoutePointID, int SysCode)
        {
            Shared.View.General.inLoading(progressBar,OutPut);
            var apiflow = await Model.Requests.ChangeRoutePoint(SysCode, RoutePointID);
            Shared.View.General.outLoading(progressBar);
            if (apiflow.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public static async Task<int> AddDirecation(ProgressBar progressBar, TextView OutPut, bool isToHome)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var apiflow = await Model.Requests.AddDirection(isToHome);
            Shared.View.General.outLoading(progressBar);
            if (apiflow.Item1 == true)
            {
                return apiflow.Item2;
            }
            else
            {
                return -1;

            }
        }

        public static async Task<bool> ChangePath(ProgressBar progressBar, TextView OutPut,int PathID, int SysCode, bool refresh = true, string newname = "")
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var apiflow = await Model.Requests.ChangePath(SysCode, PathID, newname);
            Shared.View.General.outLoading(progressBar);
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
