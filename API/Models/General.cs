using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    class General
    {
        public static string CreateSign()
        {
            var AESEngine = new Shared.Crypting.AES();
            //#if ANDROID
#if DEBUG
            var skey = LocalStorage.SessionKey;
            var Id = LocalStorage.SessionID.ToString();
            var ticks = (LocalStorage.SystemTime).ToString();
#endif
            //#endif
            return AESEngine.Encrypt((LocalStorage.SystemTime).ToString() + ',' + LocalStorage.SessionID.ToString(), LocalStorage.SessionKey);
        }

        public static string EncryptString(String OpenString)
        {
            var AESEngine = new Shared.Crypting.AES();
            return AESEngine.Encrypt(OpenString, LocalStorage.SessionKey);

        }

        public static List<Model.UserCompanion> CompanionsCache = new List<Model.UserCompanion>();
    }
}
