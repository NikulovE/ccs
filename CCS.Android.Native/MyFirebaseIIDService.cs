using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using WindowsAzure.Messaging;
using System.Collections.Generic;

namespace CCS.Android.Native
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService {
        const string TAG = "MyFirebaseIIDService";
        NotificationHub hub;

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            //Shared.Actions.RegisterInAzureHub = SendRegistrationToServer;
        //Log.Debug(TAG, "FCM token: " + refreshedToken);
           SendRegistrationToServer(refreshedToken);
        }


        public void SendRegistrationToServer(string refreshedToken)
        {
            //var refreshedToken = FirebaseInstanceId.Instance.Token;
            // Register with Notification Hubs
            hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);
            var tags = new List<string>() { Shared.Model.LocalStorage.UID.ToString() };
            try
            {
                hub.Register(refreshedToken, tags.ToArray());
            }
            catch (Exception e) {

            }

            //Log.Debug(TAG, $"Successful registration of ID {regID}");
        }
    }
}