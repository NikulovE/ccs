using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class MapPoints
    {


        public class Houses {
            public int RouteID;


            public static bool SetHome(double lon, double lat)
            {
                var dbo = new AppDbDataContext();

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
                var dbo = new AppDbDataContext();
                try
                {
                    //var UID = dbo.Sessions.Single(req => req.ID == SessionID).UID;
                    var userhome = dbo.Homes.First(req => req.UID == App.UID);
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
            public List<Direction> RoutePointsArr = new List<Direction>();
            
            public bool SaveRoutePoint(double longtitude, double latitude, int pathID) {
                var dbo = new AppDbDataContext();
                try
                {
                    var Point = new RoutePoint();
                    Point.latitude = latitude;
                    Point.longtitude = longtitude;
                    Point.PathID = pathID;
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
                var dbo = new AppDbDataContext();

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

            public bool AddPath(bool isToHome)
            {
                var dbo = new AppDbDataContext();

                try
                {

                    var theNewPath = new Path();
                    theNewPath.PathName = "";
                    theNewPath.IsMon = true;
                    theNewPath.IsTue = true;
                    theNewPath.IsWed = true;
                    theNewPath.IsThu = true;
                    theNewPath.IsFri = true;
                    theNewPath.IsSat = false;
                    theNewPath.IsSun = false;
                    theNewPath.UID = App.UID;
                    theNewPath.way = isToHome;
                    dbo.Paths.InsertOnSubmit(theNewPath);
                    dbo.SubmitChanges();
                    PathID = theNewPath.PathID;

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

            public bool ChangePath(int SysCode, String SetName)
            {
                var dbo = new AppDbDataContext();

                try
                {

                    var Path = dbo.Paths.First(req => req.PathID == PathID && req.UID==App.UID);
                    //var Point = RoutePoints.First();
                    switch (SysCode) {
                        case 1:
                            Path.IsMon = true;
                            break;
                        case -1:
                            Path.IsMon = false;
                            break;
                        case 2:
                            Path.IsTue = true;
                            break;
                        case -2:
                            Path.IsTue = false;
                            break;
                        case 3:
                            Path.IsWed = true;
                            break;
                        case -3:
                            Path.IsWed = false;
                            break;
                        case 4:
                            Path.IsThu = true;
                            break;
                        case -4:
                            Path.IsThu = false;
                            break;
                        case 5:
                            Path.IsFri = true;
                            break;
                        case -5:
                            Path.IsFri = false;
                            break;
                        case 6:
                            Path.IsSat = true;
                            break;
                        case -6:
                            Path.IsSat = false;
                            break;
                        case 7:
                            Path.IsSun = true;
                            break;
                        case -7:
                            Path.IsSun = false;
                            break;
                        case 10:
                            Path.way = true;
                            break;
                        case 0:
                            Path.way = false;
                            break;
                        case 20:
                            Path.PathName = SetName;
                            break;
                        case 30:
                            dbo.Paths.DeleteOnSubmit(Path);
                            break;

                    }

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
                var dbo = new AppDbDataContext();

                try
                {


                    var Directions = dbo.Paths.Where(req => req.UID == App.UID);
                    foreach (var direction in Directions) {
                        int currentPathID = direction.PathID;

                        var currentDirection = new Direction();
                        currentDirection.PathID = currentPathID;

                        List<OnMapPoint> pointsOfRoute = new List<OnMapPoint>();

                        var Actuality = new WeekActuality();
                        Actuality.Monday = direction.IsMon;
                        Actuality.Tuesday = direction.IsTue;
                        Actuality.Wednesday = direction.IsWed;
                        Actuality.Thursday = direction.IsThu;
                        Actuality.Friday = direction.IsFri;
                        Actuality.Saturday = direction.IsSat;
                        Actuality.Sunday = direction.IsSun;

                        currentDirection.Actuality = Actuality;
                        currentDirection.IsToHome = direction.way;
                        currentDirection.DirectionName = direction.PathName;
                        foreach (var point in dbo.RoutePoints.Where(req=>req.PathID==currentPathID))
                        {                                    

                            var mappoint = new OnMapPoint();
                            mappoint.Latitude = point.latitude;
                            mappoint.Longtitude = point.longtitude;
                            mappoint.UID = App.UID;
                            mappoint.PointID = point.RoutePointId;
                            pointsOfRoute.Add(mappoint);
                            
                        }
                        currentDirection.Points = pointsOfRoute;
                        RoutePointsArr.Add(currentDirection);
                    }
                    if (RoutePointsArr.Count == 0) {

                        var deftoHome = new Path();
                        deftoHome.PathName = "";
                        deftoHome.IsMon = true;
                        deftoHome.IsTue = true;
                        deftoHome.IsWed = true;
                        deftoHome.IsThu = true;
                        deftoHome.IsFri = true;
                        deftoHome.IsSat = false;
                        deftoHome.IsSun = false;
                        deftoHome.way = true;
                        dbo.Paths.InsertOnSubmit(deftoHome);

                        var deftoWork = new Path();
                        deftoWork.PathName = "";
                        deftoWork.IsMon = true;
                        deftoWork.IsTue = true;
                        deftoWork.IsWed = true;
                        deftoWork.IsThu = true;
                        deftoWork.IsFri = true;
                        deftoWork.IsSat = false;
                        deftoWork.IsSun = false;
                        deftoWork.way = false;
                        dbo.Paths.InsertOnSubmit(deftoHome);
                        dbo.Paths.InsertOnSubmit(deftoWork);
                        dbo.SubmitChanges();
                        /////////////////////////////////
                        var DefaultRouteToHome = new Direction();
                        DefaultRouteToHome.PathID = deftoHome.PathID;
                        DefaultRouteToHome.DirectionName = deftoHome.PathName;
                        DefaultRouteToHome.Actuality = new WeekActuality();
                        DefaultRouteToHome.IsToHome = true;
                        DefaultRouteToHome.IsMon = true;
                        DefaultRouteToHome.IsTue = true;
                        DefaultRouteToHome.IsWed = true;
                        DefaultRouteToHome.IsThu = true;
                        DefaultRouteToHome.IsFri = true;
                        DefaultRouteToHome.IsSat = false;
                        DefaultRouteToHome.IsSun = false;

                        /////////////////////////////////
                        var DefaultRouteToWork = new Direction();
                        DefaultRouteToWork.Actuality = new WeekActuality();
                        DefaultRouteToWork.PathID = deftoWork.PathID;
                        DefaultRouteToWork.DirectionName = deftoWork.PathName;
                        DefaultRouteToWork.IsToHome = true;
                        DefaultRouteToWork.IsMon = true;
                        DefaultRouteToWork.IsTue = true;
                        DefaultRouteToWork.IsWed = true;
                        DefaultRouteToWork.IsThu = true;
                        DefaultRouteToWork.IsFri = true;
                        DefaultRouteToWork.IsSat = false;
                        DefaultRouteToWork.IsSun = false;
                        /////////////////////////////////
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