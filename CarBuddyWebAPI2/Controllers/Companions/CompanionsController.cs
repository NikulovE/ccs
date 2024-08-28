using WebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2.Controllers.Companions
{
    public class CompanionsController : ApiController
    {
        [HttpPost]
        public Tuple<bool, List<OnMapPoint>> FindingCompanions(int SessionID, string Sign,[FromBody]CompanionsRequest Request)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new CompanionProcessing();
                result.FindCompanionsRoutePoints(Request.Date, Request.IsToHome);
                result.FindCompanionsHomePoints(Request.Date, Request.IsToHome);
                return new Tuple<bool, List<OnMapPoint>>(true, result.PointsArray);

            }
            else
            {
                return new Tuple<bool, List<OnMapPoint>>(false, new List<OnMapPoint>());
            }
        }
    }
}
