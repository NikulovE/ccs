using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers.Registration
{
    public class LoginController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ProfileRegistration newuser)
        {
            if (ModelState.IsValid)
            {

                var req=Requests.StartRegistration(newuser.Email).Result;
                if (req.Item1 == true)
                {
                    Session["UniCarBuddyRegKey"] = req.Item2;
                    return View("ConfirmRegistartion", newuser);
                }
                else
                {
                    if (req.Item2 == "x10003")
                    {
                        var response = Models.Requests.RestoreRegistration(newuser.Email).Result;
                        if (response.Item1 == true)
                        {
                            Session["UniCarBuddyRegKey"] = response.Item2;
                            return RedirectToAction("Index", "Confirmation");
                        }
                        else
                        {
                            @ViewBag.Error = ConvertMessages.Message(response.Item2);
                        }
                    }
                    else {
                        @ViewBag.Error = ConvertMessages.Message(req.Item2);
                    }
                    return View();

                }
                //HttpContext.Response.Cookies["id"].Value = "ca-4353w";
                //Session["name"] = req.Item2;
                //ViewBag.Greeting = Session["name"].ToString();
                //ViewBag.Hs = HttpContext.Request.Cookies["id"].Value;
                //return View("ConfirmRegistartion", newuser);
            }
            else
            {

                return View();
            }
        }
        
        
    }
}