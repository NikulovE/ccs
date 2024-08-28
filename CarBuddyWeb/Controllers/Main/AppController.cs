using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers.Main
{
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLocations()
        {
            var locations = new List<Models.Locations>()
            {
                new Models.Locations(12.505353,55.335292),
                 new Models.Locations(13.505353,55.485292),
                  new Models.Locations(13.655353,55.665292)
            };
            return Json(locations, JsonRequestBehavior.AllowGet);
        }

    }
}