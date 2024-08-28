using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Shared.Model;

namespace CCS.Android.Native
{
    class GeoIP
    {
        public static async void GetGeoIP()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://icanhazip.com");
                var IP = await response.Content.ReadAsStringAsync();
                response = await client.GetAsync("http://ip-api.com/json/" + IP);
                string json = null;
                var GeoLocationResult = new GeoIPResponse();
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    GeoLocationResult = JsonEngine.Deserialize<GeoIPResponse>(json);
                }
                if (Shared.ModelView.UIBinding.Default.CurrentUserPosition.Latitude == 0 && Shared.ModelView.UIBinding.Default.CurrentUserPosition.Longitude == 0)
                {
                    if (IP.StartsWith("176.59."))
                    {
                        GeoLocationResult.lat = 51.67200f;
                        GeoLocationResult.lon = 39.18430f;
                    }
                    if (IP.StartsWith("85.26."))
                    {
                        GeoLocationResult.lat = 55.7852f;
                        GeoLocationResult.lon = 49.1693f;
                    }
                    if (IP.StartsWith("66.102."))
                    {
                        GeoLocationResult.lat = 55.7852f;
                        GeoLocationResult.lon = 49.1693f;
                    }
                    Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Location(GeoLocationResult.lat, GeoLocationResult.lon);
                }
                Shared.Actions.InitializeGPS();
            }
            catch { }
            //;.DownloadString("http://icanhazip.com");
            //try
            //{
            //    var request = WebRequest.Create("http://ip-api.com/json/" + externalip.Split('\\')[0]);
            //    var response = await request.GetResponseAsync();

            //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Shared.GeoPosition.jsonItem));

            //    var GeoIP = (Shared.GeoPosition.jsonItem)ser.ReadObject(response.GetResponseStream());
            //    Shared.ModelView.UIBinding.Default.CurrentUserPosition = new Shared.Model.Location(GeoIP.lat, GeoIP.lon);
            //    Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(GeoIP.lat, GeoIP.lon);
            //    await Shared.ViewModel.UserLocation.UpdateLocation();
            //}
            //catch { }
        }
    }

    public class GeoIPResponse
    {
        public string _as { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string isp { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string org { get; set; }
        public string query { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string status { get; set; }
        public string timezone { get; set; }
        public string zip { get; set; }
    }

}