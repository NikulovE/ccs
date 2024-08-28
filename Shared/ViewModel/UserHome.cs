
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
    class UserHome
    {

        public static async Task<bool> LoadHome()
        {
            Shared.View.General.inLoading();
            var joinerflow = await Model.Requests.LoadHome();
            Shared.View.General.outLoading();
            if (joinerflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.HomeLocation = new Shared.Model.Location(joinerflow.Item3, joinerflow.Item2);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> SetHome()
        {
            Shared.View.General.inLoading();
            var joinerflow = await Model.Requests.SetHome(Shared.ModelView.UIBinding.Default.HomeLocation.Longitude, Shared.ModelView.UIBinding.Default.HomeLocation.Latitude);
            Shared.View.General.outLoading();
            if (joinerflow.Item1 == true)
            {
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }

    }
}
