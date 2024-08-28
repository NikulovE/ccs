using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Schedule
{
    public class ScheduleController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<WeeklySchedule>> LoadSchedule(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing();
                if (schedule.LoadSchedule())
                {
                    try
                    {
                        return new Tuple<bool, List<WeeklySchedule>>(true, schedule.UserSchedules);
                    }
                    catch
                    {
                        return new Tuple<bool, List<WeeklySchedule>>(false, new List<WeeklySchedule>());
                    }
                }
                else return new Tuple<bool, List<WeeklySchedule>>(false, new List<WeeklySchedule>());

            }
            else
            {
                return new Tuple<bool, List<WeeklySchedule>>(false, new List<WeeklySchedule>());

            }
        }
        [HttpPost]
        public Tuple<bool, string> UpdateSchedule(int SessionID, string Sign, [FromBody]WeeklySchedule UserSchedule)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing { UserSingleSchedule = UserSchedule };
                if (schedule.UpdateSchedule())
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x21001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x21002");
                    }
                }
                else return new Tuple<bool, String>(false, "x21002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        [HttpDelete]
        public Tuple<bool, string> RemoveSchedule(int SessionID, string Sign, int ScheduleID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing();
                if (schedule.DeleteSchedule(ScheduleID))
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x21001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x21002");
                    }
                }
                else return new Tuple<bool, String>(false, "x21002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }


        [HttpPut]
        public Tuple<bool, WeeklySchedule> AddSchedule(int SessionID, string Sign, [FromBody]String foo)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing();
                if (schedule.AddSchedule())
                {
                    try
                    {
                        return new Tuple<bool, WeeklySchedule>(true, schedule.UserSingleSchedule);
                    }
                    catch
                    {
                        return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
                    }
                }
                else return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());

            }
            else
            {
                return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
            }
        }
    }
}
