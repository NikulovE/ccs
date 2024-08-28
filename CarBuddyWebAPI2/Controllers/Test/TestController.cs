using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Test
{
    public class TestController : ApiController
    {
        // GET: api/Test
        public Resp Get()
        {
            return new Resp { yes = "OK" };
        }

        public class Resp
        {
            public string yes { get; set; }
        }

        //// GET: api/Test/5
        //public string Get(string SessionKey)
        //{
        //    if (ProfileProcessing.SetCookies("SHIT")) return "True";
        //    else return "false";
        //}

        //// POST: api/Test
        //public void Post([FromBody]string value)
        //{
        //    ProfileProcessing.SetCookies("SHIT");
        //    //return "HI";
        //}

        //// PUT: api/Test/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Test/5
        //public void Delete(int id)
        //{
        //}
    }
}
