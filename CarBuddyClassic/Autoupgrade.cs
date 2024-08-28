using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CCS.Classic
{
    public class Autoupgrade
    {
        //const int CurrentVersion = 201801260;

        async static void UpgradeVersion()
        {
            try
            {
                var filename = System.AppDomain.CurrentDomain.BaseDirectory + "CCS.classic.msi";
                using (var httpClient = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (var request = new HttpRequestMessage(HttpMethod.Get, "https://commutecarsharing.ru/api/Download/Download?filename=CCS.classic.msi&subpath=WPF"))
                    {

                        using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            await contentStream.CopyToAsync(stream);
                            try
                            {
                                System.Diagnostics.Process.Start(filename, "/qn /norestart");
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch {

            }

        }

        public static async void CheckLatestVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            var CurrentVersion = new Version(version);
            var WebVersion = new Version(await Shared.Model.Requests.CheckLatestVersion());
            if (WebVersion > CurrentVersion) UpgradeVersion();
        }


    }
}
