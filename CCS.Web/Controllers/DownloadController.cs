using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using CCS.Web.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http.Headers;

namespace CCS_Web.Controllers
{
    [Route("api/[controller]")]
    public class DownloadController : Controller
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> Download(string filename, string subpath)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), ("Files" + '\\' + subpath), filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".mp4", "video/mp4"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".apk", "application/vnd.android.package-archive"},
                {".application","application/x-ms-application" },
                {".msi","application/x-msi" },
                {".crt","application/x-x509-ca-cert" },
                {".csv", "text/csv"}
            };
        }





    }
}
