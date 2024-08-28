using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using CCS.Web.Models;

namespace CCS_Web.Controllers
{
    [Route("api/[controller]")]
    public class AboutController : Controller
    {
        [HttpGet("[action]")]
        public async Task<String> Statistics()
        {
            return await WebAPIRequests.Request("/api/Statistics");
        }


    }
}
