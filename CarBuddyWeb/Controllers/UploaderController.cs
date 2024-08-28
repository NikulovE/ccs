using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class UploaderController : Controller
    {
        // GET: Uploader
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)

        {

            if (postedFile != null)

            {

                string path = Server.MapPath("~/Content/Uploads/");

                if (!Directory.Exists(path))

                {

                    Directory.CreateDirectory(path);

                }



                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));

                ViewBag.Message = "File uploaded successfully.";

            }



            return View();

        }
    }
}