using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class DownloadsController : Controller
    {
        // GET: Download
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CA()
        {
            string path = @"~/Download/CA/RootCA.crt";
            string content = "application/x-x509-ca-cert";
            return new FilePathResult(path, content);

        }

        public ActionResult Classic()
        {
            string path = @"~/Download/WPF/CCS.Classic.msi";
            string content = "application/application/x-msi";
            return new FilePathResult(path, content);

        }

        public ActionResult AndroidApk()
        {
            string path = @"~/Download/Android/com.ccs.android-Signed.apk";
            string content = "application/vnd.android.package-archive";
            return new FilePathResult(path, content);

        }
    }
}