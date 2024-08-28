using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Home
{
    public class HomeController : ApiController
    {
        [HttpPut]
        public Tuple<bool, string> SetHome(int SessionID, string Sign, [FromBody]Location gps)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (MapPoints.Houses.SetHome(gps.Longitude, gps.Latitude))
                {
                    return new Tuple<bool, String>(true, "x15000"); //created
                }
                else
                {
                    return new Tuple<bool, String>(false, "x15001"); //created
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
        [HttpGet]
        public Tuple<bool, double, double> LoadHome(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var UserHome = MapPoints.Houses.LoadHome();
                if (UserHome.Item1 != 0 && UserHome.Item2 != 0)
                {
                    return new Tuple<bool, double, double>(true, UserHome.Item1, UserHome.Item2); //created
                }
                else
                {
                    return new Tuple<bool, double, double>(false, 1, 0); //created
                }
            }
            else
            {
                return new Tuple<bool, double, double>(false, 0, 1); //created
            }
        }
    }
}
