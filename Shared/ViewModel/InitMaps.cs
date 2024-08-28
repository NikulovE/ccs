

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


#if NETFX_CORE
#if WINDOWS_UWP
#else
    using Bing.Maps;
#endif
#else

#if WPF
using Microsoft.Maps.MapControl.WPF;
#endif
#endif

namespace Shared.ViewModel
{
#if WINDOWS_UWP
#else
#if XAMARIN

#else
    class InitMaps
    {
        public static async Task<bool> InitBingMapsEngine(Map MainMap)
        {
            View.General.inLoading();
            var apiflow = await Model.Requests.RetrievBingMapsKey();
            View.General.outLoading();
            if (apiflow.Item1 == true)
            {
                var AESEngine = new Shared.Crypting.AES();
#if NETFX_CORE
                MainMap.Credentials = AESEngine.Decrypt(apiflow.Item2, Model.LocalStorage.SessionKey);
#else

#if WPF
                //var bingKey = new ApplicationIdCredentialsProvider(AESEngine.Decrypt(apiflow.Item2, Model.LocalStorage.SessionKey));
                //var bingKey = new ApplicationIdCredentialsProvider("AkJ2whJytZnJi4vvYHWvHwwkOufOiw7gWLfqWWo0WrmbHWXPS49PuW8PGCjNiVmh");
                //MainMap.CredentialsProvider = bingKey;
#endif



#endif
                return true;
            }
            else
            {
                ModelView.UIBinding.Default.OutPut = ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                return false;
            }
        }
    }
#endif
#endif
}