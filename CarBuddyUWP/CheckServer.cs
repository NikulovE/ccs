using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CarBuddyUWP
{
    class CheckServer
    {
        public static async Task<bool> CheckAvailability()
        {
            try
            {
                var request = WebRequest.Create("http://api.unicarbuddy.ru/CarBuddyService.svc");
                request.Method = "GET";
                var response = await request.GetResponseAsync();
                if (response.ContentLength > 0) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


        //public static void CheckRussia() {
        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create("http://10.10.111.187/CarBuddyService.svc");
        //        request.Method = "GET";
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var success = response.StatusCode == HttpStatusCode.OK;
        //        if (success)
        //        {
        //            ServerName = "10.10.111.187";
        //            App.APIEndPointAddress = new System.ServiceModel.EndpointAddress("http://10.10.111.187/CarBuddyService.svc");
        //        }
        //    }
        //    catch (Exception) { }

        //}

    }
}
