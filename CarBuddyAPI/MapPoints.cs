using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class MapPoints
    {


        public class Houses {
            public int RouteID;


            public static bool SetHome(double lon, double lat)
            {
                var dbo = new CarBuddyDataContext();

                try
                {
                    
                    var userhome = dbo.Homes.First(req => req.UID == App.UID);
                    userhome.longtitude = lon;
                    userhome.latitude = lat;
                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception) {
                    try
                    {
                        var Home = new Home();
                        Home.UID = App.UID;
                        Home.longtitude = lon;
                        Home.latitude = lat;
                        dbo.Homes.InsertOnSubmit(Home);
                        dbo.SubmitChanges();
                        return true;
                    }
                    catch (Exception) {
                        return false;
                    }

                }
                finally
                {
                    dbo.Connection.Close();
                }

            }

            public static Tuple<Double,Double> LoadHome()
            {
                var dbo = new CarBuddyDataContext();
                try
                {
                    //var UID = dbo.Sessions.Single(req => req.ID == SessionID).UID;
                    var userhome = dbo.Homes.First(req => req.Session.ID == App.SessionID);
                    return new Tuple<double, double>(userhome.longtitude, userhome.latitude);
                }
                catch
                {
                    return new Tuple<double, double>(0, 0);

                }
                finally
                {
                    dbo.Connection.Close();
                }

            }


        }

        public class Points {

            public int RouteID;
            public int PathID;
            public List<CarBuddyAPI.Route> RoutePointsArr = new List<CarBuddyAPI.Route>();
            
            public bool SaveRoutePoint(double longtitude, double latitude, bool Way, int pathID, WeekActuality Actuality) {
                var dbo = new CarBuddyDataContext();

                try
                {
                    var Point = new RoutePoint();
                    Point.UID = App.UID;
                    Point.latitude = latitude;
                    Point.longtitude = longtitude;
                    Point.way = Way;
                    Point.PathID = pathID;

                    Point.isActualMon = Actuality.Monday;
                    Point.isActualTue = Actuality.Tuesday;
                    Point.isActualWed = Actuality.Wednesday;
                    Point.isActualThu = Actuality.Thursday;
                    Point.isActualFri = Actuality.Friday; 
                    Point.isActualSun = Actuality.Sunday; 
                    Point.isActualSat = Actuality.Saturday;

                    dbo.RoutePoints.InsertOnSubmit(Point);
                    dbo.SubmitChanges();
                    RouteID=Point.RoutePointId;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    dbo.Connection.Close();
                }
            }


            public bool ChangeRoutePoint(int SysCode)
            {
                var dbo = new CarBuddyDataContext();

                try
                {

                    var Point = dbo.RoutePoints.First(req => req.RoutePointId == RouteID);

                    if (SysCode == -1) {

                        dbo.RoutePoints.DeleteOnSubmit(Point);

                    }
                    //if(SysCode > 0){
                    //    if (SysCode == 1) Point.isActualMon = !Point.isActualMon;
                    //    if (SysCode == 2) Point.isActualTue = !Point.isActualTue;
                    //    if (SysCode == 3) Point.isActualWed = !Point.isActualWed;
                    //    if (SysCode == 4) Point.isActualThu = !Point.isActualThu;
                    //    if (SysCode == 5) Point.isActualFri = !Point.isActualFri;
                    //    if (SysCode == 6) Point.isActualSat = !Point.isActualSat;
                    //    if (SysCode == 7) Point.isActualSun = !Point.isActualSun;
                    //}
                    //if (SysCode == 0) Point.way = null;
                    //if (SysCode == 20) Point.way = false;
                    //if (SysCode == 30) Point.way = true;
                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    dbo.Connection.Close();
                }
            }

            public bool ChangePath(int SysCode)
            {
                var dbo = new CarBuddyDataContext();

                try
                {

                    var RoutePoints = dbo.RoutePoints.Where(req => req.PathID == PathID && req.UID==App.UID);
                    var Point = RoutePoints.First();
                    switch (SysCode) {
                        case 1:
                            RoutePoints.ToList().ForEach(i => i.isActualMon = true);
                            break;
                        case -1:
                            RoutePoints.ToList().ForEach(i => i.isActualMon = false);
                            break;
                        case 2:
                            RoutePoints.ToList().ForEach(i => i.isActualTue = true);
                            break;
                        case -2:
                            RoutePoints.ToList().ForEach(i => i.isActualTue = false);
                            break;
                        case 3:
                            RoutePoints.ToList().ForEach(i => i.isActualWed = true);
                            break;
                        case -3:
                            RoutePoints.ToList().ForEach(i => i.isActualWed = false);
                            break;
                        case 4:
                            RoutePoints.ToList().ForEach(i => i.isActualThu = true);
                            break;
                        case -4:
                            RoutePoints.ToList().ForEach(i => i.isActualThu = false);
                            break;
                        case 5:
                            RoutePoints.ToList().ForEach(i => i.isActualFri = true);
                            break;
                        case -5:
                            RoutePoints.ToList().ForEach(i => i.isActualFri = false);
                            break;
                        case 6:
                            RoutePoints.ToList().ForEach(i => i.isActualSat = true);
                            break;
                        case -6:
                            RoutePoints.ToList().ForEach(i => i.isActualSat = false);
                            break;
                        case 7:
                            RoutePoints.ToList().ForEach(i => i.isActualSun = true);
                            break;
                        case -7:
                            RoutePoints.ToList().ForEach(i => i.isActualSun = false);
                            break;
                        case 10:
                            RoutePoints.ToList().ForEach(i => i.way = true);
                            break;
                        case 0:
                            RoutePoints.ToList().ForEach(i => i.way = false);
                            break;

                    }
                    //if (SysCode == -1)
                    //{

                    //    dbo.RoutePoints.DeleteAllOnSubmit(RoutePoints);

                    //}

                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    dbo.Connection.Close();
                }
            }

            public bool LoadUserRoutePoints()
            {
                var dbo = new CarBuddyDataContext();

                try
                {
                    
                    var Query = dbo.RoutePoints.Where(req => req.UID == App.UID);
                    var Routes = Query.Select(req => req.PathID).Distinct();
                    foreach (var r in Routes) {
                        int currentPathID = r;
                        Route currentRoute = new Route();
                        currentRoute.PathID = currentPathID;

                        List<OnMapPoint> pointsOfRoute = new List<OnMapPoint>();
                        
                        
                        foreach (var point in Query.Where(req=>req.PathID==r))
                        {         
                            var Actuality = new WeekActuality();
                            Actuality.Monday = point.isActualMon.Value;
                            Actuality.Tuesday = point.isActualTue.Value;
                            Actuality.Wednesday = point.isActualWed.Value;
                            Actuality.Thursday = point.isActualThu.Value;
                            Actuality.Friday = point.isActualFri.Value;
                            Actuality.Saturday = point.isActualSat.Value;
                            Actuality.Sunday = point.isActualSun.Value;

                            currentRoute.Actuality = Actuality;

                            currentRoute.IsToHome = point.way.Value; ;

                            var mappoint = new OnMapPoint();
                            mappoint.Latitude = point.latitude;
                            mappoint.Longtitude = point.longtitude;
                            mappoint.UID = App.UID;
                            mappoint.PointID = point.RoutePointId;
                            mappoint.Way = point.way.Value;
                            pointsOfRoute.Add(mappoint);
                            
                        }
                        currentRoute.Points = pointsOfRoute;
                        RoutePointsArr.Add(currentRoute);
                    }
                    if (RoutePointsArr.Count == 0) {
                        var DefaultRouteToHome = new Route();
                        DefaultRouteToHome.Actuality = new WeekActuality();
                        DefaultRouteToHome.PathID = 1;
                        DefaultRouteToHome.IsToHome = true;
                        DefaultRouteToHome.IsMon = true;
                        DefaultRouteToHome.IsTue = true;
                        DefaultRouteToHome.IsWed = true;
                        DefaultRouteToHome.IsThu = true;
                        DefaultRouteToHome.IsFri = true;
                        DefaultRouteToHome.IsSat = false;
                        DefaultRouteToHome.IsSun = false;
                        DefaultRouteToHome.Points = new List<OnMapPoint>();

                        var DefaultRouteToWork = new Route();
                        DefaultRouteToWork.Actuality = new WeekActuality();
                        DefaultRouteToWork.PathID = 2;
                        DefaultRouteToWork.IsToHome = false;
                        DefaultRouteToWork.IsMon = true;
                        DefaultRouteToWork.IsTue = true;
                        DefaultRouteToWork.IsWed = true;
                        DefaultRouteToWork.IsThu = true;
                        DefaultRouteToWork.IsFri = true;
                        DefaultRouteToWork.IsSat = false;
                        DefaultRouteToWork.IsSun = false;
                        DefaultRouteToWork.Points = new List<OnMapPoint>();
                        

                        RoutePointsArr.Add(DefaultRouteToHome);
                        RoutePointsArr.Add(DefaultRouteToWork);
                    }           
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    dbo.Connection.Close();
                }
            }



            
        }
    }
}