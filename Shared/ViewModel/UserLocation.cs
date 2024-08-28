

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
#if WINDOWS_UWP
#else
using Bing.Maps;
#endif

#else
#if XAMARIN
#else
using Microsoft.Maps.MapControl.WPF;
#endif
#endif
namespace Shared.ViewModel
{
    class UserLocation
    {
        public static async Task<bool> UpdateLocation()
        {

            Shared.View.General.inLoading();
            var query = await Model.Requests.UpdatePosition(Shared.ModelView.UIBinding.Default.CurrentUserPosition.Longitude, Shared.ModelView.UIBinding.Default.CurrentUserPosition.Latitude);
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> LoadLocation()
        {

            Shared.View.General.inLoading();
            var query = await Model.Requests.LoadPosition();
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(query.Item2.Item2, query.Item2.Item1);
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(query.Item2.Item2, query.Item2.Item1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
