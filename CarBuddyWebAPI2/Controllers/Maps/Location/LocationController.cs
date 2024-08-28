using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Maps.Location
{
    public class LocationController : ApiController
    {
        [HttpPut]
        public Tuple<bool, string> UpdateLocation(int SessionID, string Sign, Tuple<double,double> gps)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var location = new LocationProcessing { longtitude = gps.Item1, latitude = gps.Item2 };
                if (location.UpdateLocation())
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x22001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x22002");
                    }
                }
                else return new Tuple<bool, String>(false, "x22002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
        [HttpGet]
        public Tuple<bool, Tuple<double, double>> LoadPreviousLocation(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var location = new LocationProcessing();
                if (location.LoadLocation())
                {
                    try
                    {
                        return new Tuple<bool, Tuple<double, double>>(true, new Tuple<double, double>(location.longtitude, location.latitude));
                    }
                    catch
                    {
                        return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
                    }
                }
                else return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));

            }
            else
            {
                return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
            }
        }
    }
}
