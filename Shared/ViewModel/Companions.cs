
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;



#if NETFX_CORE
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
            using Bing.Maps;
#endif

#else
#if WPF
using Microsoft.Maps.MapControl.WPF;
#endif
#if XAMARIN
using Xamarin.Forms.Maps;
#endif
#endif

namespace Shared.ViewModel
{
    class Companions
    {
#if WINDOWS_UWP
        public static async void FindCompanions(MapControl maplayer, bool isWayToHome, DateTime date)
#else
#if XAMARIN
        public static async void FindCompanions(Map maplayer, bool isWayToHome, DateTime date)
#else
        public static async void FindCompanions(MapLayer maplayer, bool isWayToHome, DateTime date)
#endif
#endif

        {

            Shared.View.General.inLoading();
            var Request = await Model.Requests.StartFindCompanions(0.0, 0.0, isWayToHome, date);
            Shared.View.General.outLoading();
            if (Request.Item1 == true)
            {
#if XAMARIN
                View.MapsSymbols.ShowCompanions(maplayer, Request.Item2);
#else
                maplayer.Children.Clear();
                View.MapsSymbols.ShowCompanions(maplayer, Request.Item2);
#endif
            }
        }

        public static async Task<Model.UserCompanion> GetCompanionInfo(int CompanionID)
        {
            if (Shared.Model.General.CompanionsCache.Any(req => req.UID == CompanionID))
            {
                return Shared.Model.General.CompanionsCache.First(req => req.UID == CompanionID);
            }
            else
            {
                Shared.View.General.inLoading();
                var Request = await Model.Requests.GetUserInfo(CompanionID);
                Shared.View.General.outLoading();
                if (Request.Item1 == true)
                {
                    Shared.Model.General.CompanionsCache.Add(Request.Item2);
                    return Request.Item2;
                }
                else return new Model.UserCompanion();
            }
        }
    }
}
