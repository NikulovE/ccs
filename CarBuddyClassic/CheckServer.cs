using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Media;
using System.Net.NetworkInformation;

namespace CCS.Classic
{
    class CheckServer
    {

        //public static bool CheckAvailability() {
        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create("http://api.unicarbuddy.ru/CarBuddyService.svc");
        //        request.Method = "GET";
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var success = response.StatusCode == HttpStatusCode.OK;
        //        return success;
        //    }
        //    catch (Exception) {
        //        return false;
        //    }

        //}


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

        public static async Task<bool> isInOrgInternalNetwork(String Domain)
        {
            Ping pingSender = new Ping();
            switch (Domain)
            {
                case "icl-services.com":
                    try
                    {
                        var pingstate = await pingSender.SendPingAsync("localserverru.testrussia.local");
                        if (pingstate.Status == IPStatus.Success) return true;
                        else return false;
                    }
                    catch (Exception) {
                        return false;
                    }
                default: return false;
            }
        }
    }
}
