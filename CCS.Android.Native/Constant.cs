using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using WindowsAzure.Messaging;
using System.Collections.Generic;

namespace CCS.Android.Native
{

    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://commutecarsharing.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=3A3MrIVF46nQ3lQs80Kz6mJRvEgqQeioKiOrsrikhQw=";
        public const string NotificationHubName = "CCS";
    }

}


