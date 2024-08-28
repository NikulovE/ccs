using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Trip
{
    public class TripController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<TripOffer>> LoadTrips(int SessionID, string Sign, long CurrentUserTime)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (Trip.LoadOffers(CurrentUserTime))
                {
                    return new Tuple<bool, List<TripOffer>>(true, Trip.Offers);
                }
                else return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());

            }
            else
            {
                return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());
            }
        }

        [HttpPut]
        public async Task<Tuple<bool, string>> SendTripOffer(int SessionID, string Sign, [FromBody]ShortOffer tripOffer)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing { isToHome = tripOffer.IsToHome };
                if (await Trip.SendOffer(tripOffer.ToUID, tripOffer.RouteID, tripOffer.AtTime))
                {
                    return new Tuple<bool, String>(true, "x25001");
                }
                else return new Tuple<bool, String>(false, "x25002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        
    }

    public class AcceptTripController : ApiController
    {
        [HttpGet]
        public async Task<Tuple<bool, string>> AcceptOffer(int SessionID, string Sign, int OfferID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (await Trip.AcceptOffer(OfferID))
                {
                    return new Tuple<bool, String>(true, "x26001");
                }
                else return new Tuple<bool, String>(false, "x26002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }

    public class RejectTripController : ApiController
    {
        [HttpGet]
        public async Task<Tuple<bool, string>> RejectOffer(int SessionID, string Sign, int OfferID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (await Trip.RejectOffer(OfferID))
                {
                    return new Tuple<bool, String>(true, Trip.CompanionID.ToString());
                }
                else return new Tuple<bool, String>(false, "x27002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
