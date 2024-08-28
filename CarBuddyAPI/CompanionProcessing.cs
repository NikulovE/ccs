using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class CompanionProcessing
    {
        public bool isDriver;
        public int UserOfficeID;
        public int FriendUID;

        public List<OnMapPoint> PointsArray = new List<OnMapPoint>();


        public bool FindCompanionsHomePoints()
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                var UserSession = dbo.Sessions.First(req => req.ID == App.SessionID);
                isDriver = UserSession.User.isDriver.Value;
                UserOfficeID = UserSession.User.OfficeID;
            }
            catch (Exception)
            {
                return false;
            }
            try
            {
                var Query = dbo.Users.Where(req => req.isDriver == !isDriver && req.OfficeID == UserOfficeID && req.OrgEmployee.isVisibile.Value == true).Select(reqx => reqx.Home);
                //var 
                foreach (var point in Query)
                {
                    var newpoint = new OnMapPoint();
                    newpoint.Latitude = point.latitude;
                    newpoint.Longtitude = point.longtitude;
                    newpoint.UID = point.UID;
                    newpoint.IsHome = true;
                    PointsArray.Add(newpoint);
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

        public bool FindCompanionsRoutePoints(DateTime date, Boolean Way)
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                var UserSession = dbo.Sessions.Single(req => req.ID == App.SessionID);
                isDriver = UserSession.User.isDriver.Value;
                UserOfficeID = UserSession.User.OfficeID;
            }
            catch (Exception)
            {
                return false;
            }
            if (isDriver == true) return false;
            try
            {
                var Query = dbo.RoutePoints.AsQueryable();
                var DayOfWeek = date.DayOfWeek;
                if (DayOfWeek == DayOfWeek.Monday)
                {
                    if (Way)
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).MonToHome;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.MonToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualMon == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                    else
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).MonToWork;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.MonToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualMon == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                }
                if (DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (Way)
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).TueToHome;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.TueToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualTue == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                    else
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).TueToWork;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.TueToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualTue == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                }
                if (DayOfWeek == DayOfWeek.Thursday)
                {
                    if (Way)
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).ThuToHome;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.ThuToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualThu == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                    else
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).ThuToWork;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.ThuToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualThu == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                }
                if (DayOfWeek == DayOfWeek.Wednesday)
                {
                    if (Way)
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).WedToHome;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.WedToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualWed == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                    else
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).WedToWork;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.WedToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualWed == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                }
                if (DayOfWeek == DayOfWeek.Friday)
                {
                    if (Way)
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).FriToHome;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.FriToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualFri == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                    else
                    {
                        var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).FriToWork;
                        Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.FriToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualFri == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                    }
                }
                if (DayOfWeek == DayOfWeek.Saturday)
                {
                        if (Way)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SatToHome;
                            Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.SatToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualSat == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                        }

                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SatToWork;
                            Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.SatToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualSat == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                        }
                }
                if (DayOfWeek == DayOfWeek.Sunday)
                {
                    if (Way)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SunToHome;
                            Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.SunToHome == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualSun == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SatToWork;
                            Query = dbo.Users.Where(req => req.UID != App.UID && req.OfficeID == UserOfficeID && req.Schedule.SunToWork == userTime && req.OrgEmployee.isVisibile.Value == true && req.RoutePoint.isActualSun == true && req.RoutePoint.way.Value == Way).Select(reqx => reqx.RoutePoint);
                        }    
                }
                //var 
                foreach (var point in Query)
                {
                    var newpoint = new OnMapPoint();
                    newpoint.Latitude = point.latitude;
                    newpoint.Longtitude = point.longtitude;
                    newpoint.UID = point.UID;
                    newpoint.PointID = point.RoutePointId;
                    PointsArray.Add(newpoint);
                }
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }
        }

        public bool isFriend() {
            var dbo = new CarBuddyDataContext();
            try
            {

                UserOfficeID = dbo.Sessions.First(req => req.ID == App.SessionID).User.OfficeID;
                return dbo.Users.Any(req => req.OfficeID == UserOfficeID && req.OrgEmployee.isVisibile == true && req.UID == FriendUID);
            }
            catch
            {
                return false;
            }
        }

        public UserCompanion GetUserInfo() {
            var Friend = new UserCompanion();

            var dbo = new CarBuddyDataContext();
            var FriendProfile = dbo.Users.First(req => req.UID == FriendUID);
            Friend.Email = FriendProfile.Registration.Mail;
            Friend.FirstName = FriendProfile.FirstName;
            Friend.LastName = FriendProfile.LastName;
            Friend.Phone = FriendProfile.Phone;
            Friend.Rating = FriendProfile.Rating.Value;
            Friend.UID = FriendUID;
            Friend.Payment = FriendProfile.Payment.Value;
            if (FriendProfile.isDriver.Value) {
                var FriendCar = dbo.Cars.First(req => req.UID == FriendUID);
                Friend.Brand = FriendCar.CarBrandS.Brand;
                Friend.GovNumber = FriendCar.CarGovNumber;
                Friend.Model = FriendCar.CarModelS.Model;
                Friend.Comfort = FriendCar.CarComfort;
                Friend.Places = FriendCar.CarCapacity.Value;

            }
            return Friend;
        }
    }
}