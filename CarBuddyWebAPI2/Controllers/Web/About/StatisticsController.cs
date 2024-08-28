using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Web.About
{
    public class StatisticsController : ApiController
    {
        public Statistics Get()
        {
            var ActualStatisctics = new Statistics
            {
                Users = TotalUsers(),
                Drivers = TotalDrivers(),
                Passengers = TotalPassengers(),
                Organizations = TotalRegisteredOrganizations()
            };
            return ActualStatisctics;
        }

        public class Statistics
        {
            public int Users { get; set; }
            public int Drivers { get; set; }
            public int Passengers { get; set; }
            public int Organizations { get; set; }
        }

        public static int TotalUsers()
        {
            try
            {
                var dbo = new AppDbDataContext();
                return dbo.Users.Count();
            }
            catch
            {
                return 0;
            }
        }

        public static int TotalDrivers()
        {
            try
            {
                var dbo = new AppDbDataContext();
                return dbo.Users.Where(req => req.isDriver == true).Count();
            }
            catch
            {
                return 0;
            }
        }

        public static int TotalPassengers()
        {
            try
            {
                var dbo = new AppDbDataContext();
                return dbo.Users.Where(req => req.isDriver == false).Count();
            }
            catch
            {
                return 0;
            }
        }

        public static int TotalRegisteredOrganizations()
        {
            try
            {
                var dbo = new AppDbDataContext();
                return dbo.Organizations.Count();
            }
            catch
            {
                return 0;
            }
        }

    }
}
