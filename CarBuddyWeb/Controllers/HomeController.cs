using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                if (HttpContext.Request.Cookies["SessionKey"].Value != null) ViewBag.LoginedUser = true;
            }
            catch (Exception) { }
            return View();
        }


    }
}