using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModel
{
    class Trips
    {


        public static async Task<bool> Send(int toUID, int RouteID, bool isToHome)
        {
            Shared.View.General.inLoading();
            var query = await Model.Requests.SendOffer(toUID, RouteID, isToHome);
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                await LoadTrips();
                try
                {
                    Shared.Actions.UpdateTrip(toUID);
                }
                catch { }
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(query.Item2);
                return false;
            }
        }

        public static async Task<bool> LoadTrips()
        {
            //Shared.ModelView.UIBinding.Default.OutPut = "started";
            Shared.View.General.inLoading();
            var query = await Model.Requests.LoadOffers();
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                //Shared.ModelView.UIBinding.Default.OutPut = "loaded";
                Shared.ModelView.UIBinding.Default.TripOffers = query.Item2;
                try
                {
                    Shared.Actions.showTripPointonMap();
                    Shared.Actions.showTripOffers();
                }
                catch  {
                    //Shared.ModelView.UIBinding.Default.OutPut = "in catch";
                }
                return true;
            }
            else
            {
                //Shared.ModelView.UIBinding.Default.OutPut = "no trips";
                return false;
            }

        }



        public static async Task<bool> AcceptTrip(int OfferID)
        {
            Shared.View.General.inLoading();
            var query = await Model.Requests.AcceptOffer(OfferID);
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                await LoadTrips();
                return true;
            }
            else
            {
                return false;
            }

        }

        public static async Task<bool> RejectTrip(int OfferID)
        {
            Shared.View.General.inLoading();
            var query = await Model.Requests.RejectOffer(OfferID);
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                await LoadTrips();
                try
                {
                    Shared.Actions.UpdateTrip(int.Parse(query.Item2));

                }
                catch { }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
