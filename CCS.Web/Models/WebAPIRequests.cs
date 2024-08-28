using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CCS.Web.Models
{
    public class WebAPIRequests
    {
        public static async Task<String> Request(String controller) {
            using (var client = new HttpClient())
            {
                try
                {
#if DEBUG
                    client.BaseAddress = new Uri("http://localhost:59524");
#else
                    client.BaseAddress = new Uri("http://api.commutecarsharing.ru");
#endif
                    var response = await client.GetAsync(controller);
                    return await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    return "Error";
                }
            }
        }
    }
}
