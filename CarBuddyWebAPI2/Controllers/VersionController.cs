using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    public class VersionController : ApiController
    {
        public String Get()
        {
            return "18.4.19.0";
        }
    }
}
