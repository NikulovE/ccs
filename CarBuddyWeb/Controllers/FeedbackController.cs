using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;
namespace AppWeb.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Support
        public ActionResult Index()
        {
            //try
            //{
            //    if (HttpContext.Request.Cookies["SessionKey"].Value != null) ViewBag.LoginedUser = true;
            //}
            //catch (Exception) { }
            ////var AppFeedbacks = Models.Requests.LoadFeebacks();
            //ViewBag.Feedbacks = AppFeedbacks.Reverse();

            //ViewBag.PreviousUserFeedbacks = "";
            //ViewBag.PreviousUserStars = 1;
            //try
            //{
            //    int UserUID = int.Parse(HttpContext.Request.Cookies["UID"].Value);
            //    if (AppFeedbacks.Any(req => req.UID == UserUID))
            //    {
            //        var previoususerfeeback = AppFeedbacks.First(req => req.UID == UserUID);
            //        ViewBag.PreviousUserFeedbacks = previoususerfeeback.FeedbackText;
            //        ViewBag.PreviousUserStars = previoususerfeeback.Stars;
            //    }
            //}
            //catch (Exception) { }
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1 star", Value = "1" });
            items.Add(new SelectListItem { Text = "2 stars", Value = "2" });
            items.Add(new SelectListItem { Text = "3 stars", Value = "3" });
            items.Add(new SelectListItem { Text = "4 stars", Value = "4" });
            items.Add(new SelectListItem { Text = "5 stars", Value = "5" });

            var model = new UserFeedback
            {
                StarsSelector = items
            };




            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserFeedback userFeedback)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int SessionID = int.Parse(HttpContext.Request.Cookies["SessionID"].Value);
                    String SessionKey = HttpContext.Request.Cookies["SessionKey"].Value;
                    var AESEngine = new Shared.Crypting.AES();
                    var Sign = AESEngine.Encrypt((DateTime.UtcNow.Ticks).ToString() + ',' + SessionID.ToString(), SessionKey);
                    //var req = Requests.SaveFeedBack(SessionID,Sign,userFeedback.SelectedStarsCounter,userFeedback.FeedbackText);
                }
                catch (Exception) {
                }
            }
            try
            {
                if (HttpContext.Request.Cookies["SessionKey"].Value != null) ViewBag.LoginedUser = true;
            }
            catch (Exception) { }
            //var AppFeedbacks = Models.Requests.LoadFeebacks();
            //ViewBag.Feedbacks = AppFeedbacks.Reverse();

            //int UserUID = int.Parse(HttpContext.Request.Cookies["UID"].Value);
            //ViewBag.PreviousUserFeedbacks = "";
            //ViewBag.PreviousUserStars = 1;
            //if (AppFeedbacks.Any(req => req.UID == UserUID))
            //{
            //    var previoususerfeeback = AppFeedbacks.First(req => req.UID == UserUID);
            //    ViewBag.PreviousUserFeedbacks = previoususerfeeback.FeedbackText;
            //    ViewBag.PreviousUserStars = previoususerfeeback.Stars;

            //}


            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1 star", Value = "1" });
            items.Add(new SelectListItem { Text = "2 stars", Value = "2" });
            items.Add(new SelectListItem { Text = "3 stars", Value = "3" });
            items.Add(new SelectListItem { Text = "4 stars", Value = "4" });
            items.Add(new SelectListItem { Text = "5 stars", Value = "5" });

            var model = new UserFeedback
            {
                StarsSelector = items
            };
            return View(model);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}