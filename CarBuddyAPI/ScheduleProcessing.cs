using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class ScheduleProcessing
    {
        public WeeklySchedule UserSchedule = new WeeklySchedule();

        public bool LoadSchedule()
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                var Schedule =new Schedule();
                try
                {
                    Schedule = dbo.Schedules.First(req => req.UID == App.UID);
                }
                catch (Exception) {
                    CreateDefaultSchedule();
                    Schedule = dbo.Schedules.First(req => req.UID == App.UID);
                }


                UserSchedule.Monday.ToHome = Schedule.MonToHome;
                UserSchedule.Monday.ToWork = Schedule.MonToWork;
                UserSchedule.Monday.IsEnabled = Schedule.isMon;

                UserSchedule.Tuesday.ToHome = Schedule.TueToHome;
                UserSchedule.Tuesday.ToWork = Schedule.TueToWork;
                UserSchedule.Tuesday.IsEnabled = Schedule.isTue;

                UserSchedule.Wednesday.ToHome = Schedule.WedToHome;
                UserSchedule.Wednesday.ToWork = Schedule.WedToWork;
                UserSchedule.Wednesday.IsEnabled = Schedule.isWed;

                UserSchedule.Thursday.ToHome = Schedule.ThuToHome;
                UserSchedule.Thursday.ToWork = Schedule.ThuToWork;
                UserSchedule.Thursday.IsEnabled = Schedule.isThu;

                UserSchedule.Friday.ToHome = Schedule.FriToHome;
                UserSchedule.Friday.ToWork = Schedule.FriToWork;
                UserSchedule.Friday.IsEnabled = Schedule.isFri;

                UserSchedule.Saturday.ToHome = Schedule.SatToHome;
                UserSchedule.Saturday.ToWork = Schedule.SatToWork;
                UserSchedule.Saturday.IsEnabled = Schedule.isSat;

                UserSchedule.Sunday.ToHome = Schedule.SunToHome;
                UserSchedule.Sunday.ToWork = Schedule.SunToWork;
                UserSchedule.Sunday.IsEnabled = Schedule.isSun;

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


        public static void CreateDefaultSchedule() {
            var dbo = new CarBuddyDataContext();
            var UserDefaultSchedule = new Schedule();
            UserDefaultSchedule.UID = App.UID;


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

        public bool UpdateSchedule()
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                var Schedule = dbo.Schedules.First(req => req.UID == App.UID);

                Schedule.isMon = UserSchedule.Monday.IsEnabled;
                Schedule.isTue = UserSchedule.Tuesday.IsEnabled;
                Schedule.isWed = UserSchedule.Wednesday.IsEnabled;
                Schedule.isThu = UserSchedule.Thursday.IsEnabled;
                Schedule.isFri = UserSchedule.Friday.IsEnabled;
                Schedule.isSat = UserSchedule.Saturday.IsEnabled;
                Schedule.isSun = UserSchedule.Sunday.IsEnabled;

                //Monday
                Schedule.MonToHome = UserSchedule.Monday.ToHome;
                Schedule.MonToWork = UserSchedule.Monday.ToWork;

                //Tuesday
                Schedule.TueToHome = UserSchedule.Tuesday.ToHome;
                Schedule.TueToWork = UserSchedule.Tuesday.ToWork;

                //Wednesday
                Schedule.WedToHome = UserSchedule.Wednesday.ToHome;
                Schedule.WedToWork = UserSchedule.Wednesday.ToWork;

                //Thursday
                Schedule.ThuToHome = UserSchedule.Thursday.ToHome;
                Schedule.ThuToWork = UserSchedule.Thursday.ToWork;

                //Friday
                Schedule.FriToHome = UserSchedule.Friday.ToHome;
                Schedule.FriToWork = UserSchedule.Friday.ToWork;

                //Saturday
                Schedule.SatToHome = UserSchedule.Saturday.ToHome;
                Schedule.SatToWork = UserSchedule.Saturday.ToWork;

                //Sunday
                Schedule.SunToHome = UserSchedule.Sunday.ToHome;
                Schedule.SunToWork = UserSchedule.Sunday.ToWork;

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