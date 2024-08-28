using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers.Registration
{
    public class ConfirmationController : Controller
    {
        // GET: Confirmation
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ProfileConfirmation newuser)
        {
            if (ModelState.IsValid)
            {

                var regkey = Session["UniCarBuddyRegKey"].ToString();
                var AesEngine = new Shared.Crypting.AES();
                var OpenedKey = "";
                try
                {
                    OpenedKey = AesEngine.Decrypt(regkey, newuser.Password);
                }
                catch (Exception)
                {
                    ViewBag.Error = Properties.Resources.x50003;
                    OpenedKey = "";
                }
                if (OpenedKey != "")
                {
                    var RegData = OpenedKey.Split(',');
                    var UID = int.Parse(RegData[0]);
                    var RSAPub = RegData[1];
                    try
                    {
                        var RSAEngine = new Shared.Crypting.RSA();
                        var Answer = RSAEngine.Encrypt(newuser.Password, RSAPub);
                        var Response = Requests.ConfirmRegistration(UID, Answer).Result;

                        if (Response.Item1 == true)
                        {

                            HttpContext.Response.Cookies["MasterAESKey"].Value = Shared.Crypting.GeneratePassword(32);
                            HttpContext.Response.Cookies["ServerRSAKey"].Value = RSAPub;
                            HttpContext.Response.Cookies["UID"].Value = UID.ToString();
                            var ServerSign = AesEngine.Decrypt(Response.Item2, newuser.Password);
                            HttpContext.Response.Cookies["SessionKey"].Value = ServerSign.Split(',')[1];
                            HttpContext.Response.Cookies["SessionID"].Value = ServerSign.Split(',')[0];
                            HttpContext.Response.Cookies["ProfileVersion"].Value = 2.ToString();
                            return RedirectToAction("Index","App");
                        }
                        else
                        {
                            ViewBag.Error = ConvertMessages.Message(Response.Item2);
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = Properties.Resources.x50004;
                    }
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}