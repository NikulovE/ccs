using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class CompanionProcessing
    {
        public bool isDriver;
        public int UserOfficeID;
        public int FriendUID;

        public List<OnMapPoint> PointsArray = new List<OnMapPoint>();


        public bool FindCompanionsHomePoints(DateTime date, Boolean IsToHome)
        {
            var dbo = new AppDbDataContext();
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
                var Query = dbo.Homes.AsQueryable();
                //var Query = dbo.Users.Where(req => req.isDriver == !isDriver && req.OfficeID == UserOfficeID).Select(reqx => reqx.Home);
                try
                {
                    
                    var DayOfWeek = date.DayOfWeek;
                    if (DayOfWeek == DayOfWeek.Monday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).MonToHome;                           
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.MonToHome > date.TimeOfDay &&*/ req.User.Schedule.isMon == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).MonToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.MonToWork > date.TimeOfDay &&*/ req.User.Schedule.isMon == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Tuesday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).TueToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.TueToHome > date.TimeOfDay &&*/ req.User.Schedule.isTue == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).TueToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.TueToWork > date.TimeOfDay &&*/ req.User.Schedule.isTue == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Thursday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).ThuToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.ThuToHome > date.TimeOfDay &&*/ req.User.Schedule.isThu == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).ThuToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.ThuToWork > date.TimeOfDay &&*/ req.User.Schedule.isThu == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Wednesday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).WedToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.WedToHome > date.TimeOfDay &&*/ req.User.Schedule.isWed == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).WedToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.WedToWork > date.TimeOfDay &&*/ req.User.Schedule.isWed == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Friday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).FriToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.FriToHome > date.TimeOfDay &&*/ req.User.Schedule.isFri == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).FriToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.FriToWork > date.TimeOfDay &&*/ req.User.Schedule.isFri == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SatToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SatToHome > date.TimeOfDay &&*/ req.User.Schedule.isSat == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SatToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SatToWork > date.TimeOfDay &&*/ req.User.Schedule.isSat == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                    if (DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (IsToHome)
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SunToHome;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SunToHome > date.TimeOfDay &&*/ req.User.Schedule.isSun == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);

                        }
                        else
                        {
                            var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID).SunToWork;
                            Query = dbo.Homes.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SunToWork > date.TimeOfDay &&*/ req.User.Schedule.isSun == true && req.UID != App.UID && req.User.Schedule.IsEnabled == true);
                        }
                    }
                }
                catch { }


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

        public bool FindCompanionsRoutePoints(DateTime date, Boolean IsToHome)
        {
            var dbo = new AppDbDataContext();
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
            if (isDriver == true) return false;
            try
            {
                var Query = dbo.Paths.AsQueryable();
                var DayOfWeek = date.DayOfWeek;
                if (DayOfWeek == DayOfWeek.Monday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled==true).MonToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.MonToHome > date.TimeOfDay &&*/ req.User.Schedule.isMon==true  && req.IsMon==true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled==true); 

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).MonToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.MonToWork > date.TimeOfDay && */ req.User.Schedule.isMon == true && req.IsMon == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).TueToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.TueToHome > date.TimeOfDay && */ req.User.Schedule.isTue == true && req.IsTue == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).TueToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.TueToWork > date.TimeOfDay && */ req.User.Schedule.isTue == true && req.IsTue == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Thursday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).ThuToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.ThuToHome > date.TimeOfDay && */ req.User.Schedule.isThu == true && req.IsThu == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).ThuToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.ThuToWork > date.TimeOfDay && */ req.User.Schedule.isThu == true && req.IsThu == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Wednesday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).WedToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.WedToHome > date.TimeOfDay && */ req.User.Schedule.isWed == true && req.IsWed == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).WedToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.WedToWork > date.TimeOfDay && */ req.User.Schedule.isWed == true && req.IsWed == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Friday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).FriToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.FriToHome > date.TimeOfDay && */ req.User.Schedule.isFri == true && req.IsFri == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).FriToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.FriToWork > date.TimeOfDay && */ req.User.Schedule.isFri == true && req.IsFri == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Saturday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).SatToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SatToHome > date.TimeOfDay && */ req.User.Schedule.isSat == true && req.IsSat == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).SatToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SatToWork > date.TimeOfDay && */ req.User.Schedule.isSat == true && req.IsSat == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                if (DayOfWeek == DayOfWeek.Sunday)
                {
                    if (IsToHome)
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).SunToHome;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SunToHome > date.TimeOfDay && */ req.User.Schedule.isSun == true && req.IsSun == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);

                    }
                    else
                    {
                        //var userTime = dbo.Schedules.First(reqs => reqs.UID == App.UID && reqs.IsEnabled == true).SunToWork;
                        Query = dbo.Paths.Where(req => req.User.isDriver != isDriver && req.User.OfficeID == UserOfficeID && /*req.User.Schedule.SunToWork < date.TimeOfDay && */ req.User.Schedule.isSun == true && req.IsSun == true && req.UID != App.UID && req.way == IsToHome && req.User.Schedule.IsEnabled == true);
                    }
                }
                //var 
                foreach (var path in Query)
                {
                    var points = dbo.RoutePoints.Where(req => req.PathID == path.PathID);
                    foreach (var point in points)
                    {
                        var newpoint = new OnMapPoint();
                        newpoint.Latitude = point.latitude;
                        newpoint.Longtitude = point.longtitude;
                        newpoint.UID = path.UID;
                        newpoint.PointID = point.RoutePointId;
                        PointsArray.Add(newpoint);
                    }
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
            var dbo = new AppDbDataContext();
            try
            {

                UserOfficeID = dbo.Sessions.First(req => req.ID == App.SessionID).User.OfficeID;
                return dbo.Users.Any(req => req.OfficeID == UserOfficeID && req.UID == FriendUID);
            }
            catch
            {
                return false;
            }
        }

        public UserCompanion GetUserInfo() {
            var Friend = new UserCompanion();
            
            var dbo = new AppDbDataContext();
            var FriendProfile = dbo.Users.First(req => req.UID == FriendUID);
            try
            {
                Friend.Email = FriendProfile.Registration.Mail;
            }
            catch { }
            try
            {
                Friend.FirstName = FriendProfile.FirstName;
            }
            catch { }
            try
            {
                Friend.LastName = FriendProfile.LastName;
            }
            catch { }
            try
            {
                Friend.Phone = FriendProfile.Phone;
            }
            catch { }
            try
            {
                Friend.Rating = FriendProfile.Rating.Value;
            }
            catch { }
            try
            {
                Friend.UID = FriendUID;
            }
            catch { }
            var DayOfWeek = DateTime.Now.DayOfWeek;
            try
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).MonToHome.Ticks;
            }
            catch {
                ScheduleProcessing.CreateDefaultSchedule(FriendUID);
            }
            if (DayOfWeek == DayOfWeek.Monday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).MonToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).MonToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Tuesday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).TueToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).TueToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Thursday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).ThuToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).ThuToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Wednesday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).WedToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).WedToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Friday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).FriToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).FriToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Saturday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).FriToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).FriToWork.Ticks;
            }
            if (DayOfWeek == DayOfWeek.Sunday)
            {
                Friend.ToHome = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).SunToHome.Ticks;
                Friend.ToWork = dbo.Schedules.First(reqs => reqs.UID == FriendUID && reqs.IsEnabled == true).SunToWork.Ticks;
            }
            Friend.Payment = FriendProfile.Payment.Value;
            if (FriendProfile.isDriver.Value) {
                try
                {
                    try
                    {
                        var FriendCar = dbo.Cars.First(req => req.UID == FriendUID);
                        Friend.Brand = FriendCar.CarBrandS.Brand;
                        Friend.GovNumber = FriendCar.CarGovNumber;
                        Friend.Model = FriendCar.CarModelS.Model;
                        Friend.Comfort = FriendCar.CarComfort;
                        Friend.Places = FriendCar.CarCapacity.Value;
                    }
                    catch {
                        CarProcessing.CreateCar(FriendUID);
                        var FriendCar = dbo.Cars.First(req => req.UID == FriendUID);
                        Friend.Brand = FriendCar.CarBrandS.Brand;
                        Friend.GovNumber = FriendCar.CarGovNumber;
                        Friend.Model = FriendCar.CarModelS.Model;
                        Friend.Comfort = FriendCar.CarComfort;
                        Friend.Places = FriendCar.CarCapacity.Value;
                    }
                   
                }
                catch { }

            }
            return Friend;
        }
    }
}