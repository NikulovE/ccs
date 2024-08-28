using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class ScheduleProcessing
    {
        public List<WeeklySchedule> UserSchedules = new List<WeeklySchedule>();
        public WeeklySchedule UserSingleSchedule = new WeeklySchedule();
        public bool LoadSchedule()
        {
            var dbo = new AppDbDataContext();
            try
            {
                var Schedules =new List<Schedule>();
                try
                {
                    Schedules = dbo.Schedules.Where(req => req.UID == App.UID).ToList();
                }
                catch (Exception) {
                    CreateDefaultSchedule(App.UID);
                    Schedules = dbo.Schedules.Where(req => req.UID == App.UID).ToList();
                }
                if (Schedules.Count == 0)
                {
                    CreateDefaultSchedule(App.UID);
                    Schedules = dbo.Schedules.Where(req => req.UID == App.UID).ToList();
                }

                if (Schedules.Count == 1)
                {
                    dbo.Schedules.First(req => req.UID == App.UID).IsEnabled = true;
                    dbo.SubmitChanges();
                }
                foreach (var schedule in Schedules)
                {
                    var UserSchedule = new WeeklySchedule();
                    UserSchedule.ScheduleID = schedule.ScheduleID;
                    UserSchedule.IsEnabled = schedule.IsEnabled;

                    UserSchedule.Monday.ToHome = schedule.MonToHome.Ticks;
                    UserSchedule.Monday.ToWork = schedule.MonToWork.Ticks;
                    UserSchedule.Monday.IsEnabled = schedule.isMon;

                    UserSchedule.Tuesday.ToHome = schedule.TueToHome.Ticks;
                    UserSchedule.Tuesday.ToWork = schedule.TueToWork.Ticks;
                    UserSchedule.Tuesday.IsEnabled = schedule.isTue;

                    UserSchedule.Wednesday.ToHome = schedule.WedToHome.Ticks;
                    UserSchedule.Wednesday.ToWork = schedule.WedToWork.Ticks;
                    UserSchedule.Wednesday.IsEnabled = schedule.isWed;

                    UserSchedule.Thursday.ToHome = schedule.ThuToHome.Ticks;
                    UserSchedule.Thursday.ToWork = schedule.ThuToWork.Ticks;
                    UserSchedule.Thursday.IsEnabled = schedule.isThu;

                    UserSchedule.Friday.ToHome = schedule.FriToHome.Ticks;
                    UserSchedule.Friday.ToWork = schedule.FriToWork.Ticks;
                    UserSchedule.Friday.IsEnabled = schedule.isFri;

                    UserSchedule.Saturday.ToHome = schedule.SatToHome.Ticks;
                    UserSchedule.Saturday.ToWork = schedule.SatToWork.Ticks;
                    UserSchedule.Saturday.IsEnabled = schedule.isSat;

                    UserSchedule.Sunday.ToHome = schedule.SunToHome.Ticks;
                    UserSchedule.Sunday.ToWork = schedule.SunToWork.Ticks;
                    UserSchedule.Sunday.IsEnabled = schedule.isSun;
                    UserSchedules.Add(UserSchedule);
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

        internal bool DeleteSchedule(int scheduleID)
        {
            try
            {
                var dbo = new AppDbDataContext();
                try
                {
                    if (dbo.Schedules.Where(req => req.UID == App.UID).Count() == 1) {
                        return false;
                    }
                }
                catch { }
                var Schedule = dbo.Schedules.First(req => req.UID == App.UID && req.ScheduleID == scheduleID);
                dbo.Schedules.DeleteOnSubmit(Schedule);
                dbo.SubmitChanges();
                return true;
            }
            catch {

                return false;
            }
        }

        public static void CreateDefaultSchedule(int UID) {
            var dbo = new AppDbDataContext();
            var UserDefaultSchedule = new Schedule();
            UserDefaultSchedule.UID = UID;

            UserDefaultSchedule.IsEnabled = true;

            UserDefaultSchedule.isMon = true;
            UserDefaultSchedule.isTue = true;
            UserDefaultSchedule.isWed = true;
            UserDefaultSchedule.isThu = true;
            UserDefaultSchedule.isFri = true;
            UserDefaultSchedule.isSat = false;
            UserDefaultSchedule.isSun = false;

            UserDefaultSchedule.MonToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.MonToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.TueToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.TueToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.WedToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.WedToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.ThuToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.ThuToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.FriToWork = new TimeSpan(8, 0, 0);
            UserDefaultSchedule.FriToHome = new TimeSpan(17, 0, 0);

            UserDefaultSchedule.SatToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.SatToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.SunToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.SunToWork = new TimeSpan(8, 0, 0);
            try
            {
                dbo.Schedules.InsertOnSubmit(UserDefaultSchedule);
                dbo.SubmitChanges();
            }
            catch (Exception) { }
            finally {
                dbo.Connection.Close();
            }
        }

        public bool AddSchedule()
        {
            var dbo = new AppDbDataContext();
            var UserDefaultSchedule = new Schedule();
            UserDefaultSchedule.UID = App.UID;

            UserDefaultSchedule.IsEnabled = false;

            UserDefaultSchedule.isMon = true;
            UserDefaultSchedule.isTue = true;
            UserDefaultSchedule.isWed = true;
            UserDefaultSchedule.isThu = true;
            UserDefaultSchedule.isFri = true;
            UserDefaultSchedule.isSat = false;
            UserDefaultSchedule.isSun = false;

            UserDefaultSchedule.MonToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.MonToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.TueToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.TueToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.WedToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.WedToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.ThuToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.ThuToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.FriToWork = new TimeSpan(8, 0, 0);
            UserDefaultSchedule.FriToHome = new TimeSpan(17, 0, 0);

            UserDefaultSchedule.SatToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.SatToWork = new TimeSpan(8, 0, 0);

            UserDefaultSchedule.SunToHome = new TimeSpan(17, 0, 0);
            UserDefaultSchedule.SunToWork = new TimeSpan(8, 0, 0);
            try
            {
                dbo.Schedules.InsertOnSubmit(UserDefaultSchedule);
                dbo.SubmitChanges();
                UserSingleSchedule.IsEnabled = false;
                UserSingleSchedule.ScheduleID = UserDefaultSchedule.ScheduleID;
                UserSingleSchedule.Monday = new DaySchedule { IsEnabled = true, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Tuesday = new DaySchedule { IsEnabled = true, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Wednesday = new DaySchedule { IsEnabled = true, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Thursday = new DaySchedule { IsEnabled = true, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Friday = new DaySchedule { IsEnabled = true, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Saturday = new DaySchedule { IsEnabled = false, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                UserSingleSchedule.Sunday = new DaySchedule { IsEnabled = false, ToHome = new TimeSpan(17, 0, 0).Ticks, ToWork = new TimeSpan(8, 0, 0).Ticks };
                return true;
            }
            catch (Exception) {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }
        }

        public bool UpdateSchedule()
        {
            var dbo = new AppDbDataContext();
            try
            {
                var Schedule = dbo.Schedules.First(req => req.UID == App.UID && req.ScheduleID== UserSingleSchedule.ScheduleID);
                if (UserSingleSchedule.IsEnabled == true) {
                    foreach (var sch in dbo.Schedules.Where(req => req.UID == App.UID && req.ScheduleID != UserSingleSchedule.ScheduleID)) {
                        sch.IsEnabled = false;
                    }
                }
                Schedule.IsEnabled = UserSingleSchedule.IsEnabled;
                
                Schedule.isMon = UserSingleSchedule.Monday.IsEnabled;
                Schedule.isTue = UserSingleSchedule.Tuesday.IsEnabled;
                Schedule.isWed = UserSingleSchedule.Wednesday.IsEnabled;
                Schedule.isThu = UserSingleSchedule.Thursday.IsEnabled;
                Schedule.isFri = UserSingleSchedule.Friday.IsEnabled;
                Schedule.isSat = UserSingleSchedule.Saturday.IsEnabled;
                Schedule.isSun = UserSingleSchedule.Sunday.IsEnabled;

                //Monday
                Schedule.MonToHome = new TimeSpan(UserSingleSchedule.Monday.ToHome);
                Schedule.MonToWork = new TimeSpan(UserSingleSchedule.Monday.ToWork);

                //Tuesday
                Schedule.TueToHome = new TimeSpan(UserSingleSchedule.Tuesday.ToHome);
                Schedule.TueToWork = new TimeSpan(UserSingleSchedule.Tuesday.ToWork);

                //Wednesday
                Schedule.WedToHome = new TimeSpan(UserSingleSchedule.Wednesday.ToHome);
                Schedule.WedToWork = new TimeSpan(UserSingleSchedule.Wednesday.ToWork);

                //Thursday
                Schedule.ThuToHome = new TimeSpan(UserSingleSchedule.Thursday.ToHome);
                Schedule.ThuToWork = new TimeSpan(UserSingleSchedule.Thursday.ToWork);

                //Friday
                Schedule.FriToHome = new TimeSpan(UserSingleSchedule.Friday.ToHome);
                Schedule.FriToWork = new TimeSpan(UserSingleSchedule.Friday.ToWork);

                //Saturday
                Schedule.SatToHome = new TimeSpan(UserSingleSchedule.Saturday.ToHome);
                Schedule.SatToWork = new TimeSpan(UserSingleSchedule.Saturday.ToWork);

                //Sunday
                Schedule.SunToHome = new TimeSpan(UserSingleSchedule.Sunday.ToHome);
                Schedule.SunToWork = new TimeSpan(UserSingleSchedule.Sunday.ToWork);                
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

        int ConvertMinToIndex(int Minutes) {
            int Index = 0;
            switch (Minutes)
            {
                case 0:
                    Index = 0;
                    break;
                case 10:
                    Index = 1;
                    break;
                case 20:
                    Index = 2;
                    break;
                case 30:
                    Index = 3;
                    break;
                case 40:
                    Index = 4;
                    break;
                case 50:
                    Index = 5;
                    break;
            }
            return Index;
        }

        int ConvertIndexToMin(int Index)
        {
            int Minutes = 0;
            switch (Index)
            {
                case 1:
                    Minutes = 0;
                    break;
                case 2:
                    Minutes = 10;
                    break;
                case 3:
                    Minutes = 20;
                    break;
                case 4:
                    Minutes = 30;
                    break;
                case 5:
                    Minutes = 40;
                    break;
                case 6:
                    Minutes = 50;
                    break;
            }
            return Minutes;
        }
    }
}