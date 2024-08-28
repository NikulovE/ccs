using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.RouteC
{
    public class RouteController : ApiController
    {
        [HttpPut]
        public Tuple<bool, int> SaveRoutePoint(int SessionID, string Sign, [FromBody]Models.OnMapPoint RoutePoint)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points();
                if (NewPoint.SaveRoutePoint(RoutePoint.Longtitude, RoutePoint.Latitude, RoutePoint.PathID))
                {
                    return new Tuple<bool, int>(true, NewPoint.RouteID); //saved N;
                }
                else
                {
                    return new Tuple<bool, int>(false, 0); //saved
                }
            }
            else
            {
                return new Tuple<bool, int>(false, 0); //saved
            }
        }
        [HttpGet]
        public Tuple<bool, List<Direction>> LoadRoutePoints(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var PointsArr = new MapPoints.Points();
                if (PointsArr.LoadUserRoutePoints())
                {
                    return new Tuple<bool, List<Direction>>(true, PointsArr.RoutePointsArr);
                }
                else
                {
                    return new Tuple<bool, List<Direction>>(false, new List<Direction>());
                }
            }
            else
            {
                return new Tuple<bool, List<Direction>>(false, new List<Direction>());
            }
        }

        [HttpPost]
        public Tuple<bool, string> ChangeRoutePoint(int SessionID, string Sign, int SysCode, [FromBody]int RouteID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points { RouteID = RouteID };
                if (NewPoint.ChangeRoutePoint(SysCode))
                {
                    return new Tuple<bool, String>(true, "x17001"); //Success
                }
                else
                {
                    return new Tuple<bool, String>(false, "x17000");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }


    }
    public class PathController : ApiController
    {
        [HttpPost]
        public Tuple<bool, string> ChangePath(int SessionID, string Sign, int SysCode, String SetName, [FromBody]int PathID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points { PathID = PathID };
                if (NewPoint.ChangePath(SysCode, SetName))
                {
                    return new Tuple<bool, String>(true, "x17001"); //Success
                }
                else
                {
                    return new Tuple<bool, String>(false, "x17000");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        [HttpPut]
        public Tuple<bool, int> AddPath(int SessionID, string Sign, [FromBody]bool IsToHome)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points();
                if (NewPoint.AddPath(IsToHome))
                {
                    return new Tuple<bool, int>(true, NewPoint.PathID); //Success
                }
                else
                {
                    return new Tuple<bool, int>(false, 0);
                }
            }
            else
            {
                return new Tuple<bool, int>(false, 0); //wrong sign
            }
        }
    }
}
