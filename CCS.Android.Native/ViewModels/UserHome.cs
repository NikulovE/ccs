
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;


namespace Shared.ViewModel
{
    class UserHome
    {

        public static async Task<bool> LoadHome(ProgressBar progressBar, TextView OutPut)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var joinerflow = await Model.Requests.LoadHome();
            Shared.View.General.outLoading(progressBar);
            if (joinerflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.HomeLocation = new Shared.Model.Location(joinerflow.Item3, joinerflow.Item2);
                //Shared.Actions.HomeLoaded();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> SetHome()
        {
            var joinerflow = await Model.Requests.SetHome(Shared.ModelView.UIBinding.Default.HomeLocation.Longitude, Shared.ModelView.UIBinding.Default.HomeLocation.Latitude);
            if (joinerflow.Item1 == true)
            {
                return true;
            }
            else
            {
                //OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }

    }
}
