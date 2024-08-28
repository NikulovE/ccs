using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Maps
{
    public class MapsController : ApiController
    {
        [HttpGet]
        public Tuple<bool, String> BingMapsAPIKey(int SessionID, String Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                return new Tuple<bool, String>(true, BingMap.BingMapsAPI(SessionID));

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001");
            }
        }
    }
}
