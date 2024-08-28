using AppWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;


namespace AppWeb.Controllers
{
    public class AboutController : Controller
    {

       

        // GET: Statistics



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            return View();

        }

        public ActionResult PrivacyPolicy()
        {
            return View();

        }
        public ActionResult RegisterOrganization()
        {
            return View();

        }

        public ActionResult CreateOffice()
        {
            return View();

        }

        



        public ActionResult Statistics()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good morning" : "Good day";
            //ViewBag.Users = TotalUsers();
            //ViewBag.Drivers = TotalDrivers();
            //ViewBag.Passengers = TotalPassengers();
            //ViewBag.Organizations = TotalRegisteredOrganizations();
            return View();
        }

        public ActionResult HowTo()
        {
            return View();
        }

    }
}