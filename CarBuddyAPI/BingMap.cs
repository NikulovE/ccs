using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class BingMap
    {
        private static string BingMapsAPIKey = "AkJ2whJytZnJi4vvYHWvHwwkOufOiw7gWLfqWWo0WrmbHWXPS49PuW8PGCjNiVmh";
        public static String BingMapsAPI(int SessionID) {

            var dbo = new CarBuddyDataContext();
            var AESEngine = new Shared.Crypting.AES();
            var UserSessionKey = dbo.Sessions.Single(req => req.ID == SessionID).CryptKey;
            return AESEngine.Encrypt(BingMapsAPIKey, UserSessionKey);
        }
    }
}