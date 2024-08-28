using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPI2.Controllers
{

    public class Notification
    {
        //public int Id { get; set; }
        //public string Payload { get; set; }
        //public bool Read { get; set; }
    }

    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        //private List<Notification> notifications = new List<Notification>();

        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://commutecarsharing.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=NAZmhxw/x0LGe30BgV74e/iLvF5MS6TWHe+JhRvIts8=", "CCS");
        }

        //public Notification CreateNotification(/*string payload*/)
        //{
        //    var notification = new Notification();
        //    //{
        //    //    Id = notifications.Count,
        //    //    Payload = payload,
        //    //    Read = false
        //    //};

        //   // notifications.Add(notification);

        //    return notification;
        //}

        //public Notification ReadNotification(int id)
        //{
        //    return notifications.ElementAt(id);
        //}
    }

    public class NotificationsController //: ApiController
    {
        //public NotificationsController()
        //{            
        //    Notifications.Instance.CreateNotification("This is a secure notification!");
        //}

        //// GET api/notifications/id
        //public Notification Get(int id)
        //{
        //    return Notifications.Instance.ReadNotification(id);
        //}

        public static async Task<bool> SendMessage(int UID, String message)
        {
            try
            {
                //var message = "Hi";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //var secureNotificationInTheBackend = Notifications.Instance.CreateNotification(message);
                var usernameTag = UID.ToString();
                //Notifications.Instance.CreateNotification();// "This is a secure notification!");
                // windows
                //var rawNotificationToBeSent = new Microsoft.Azure.NotificationHubs.WindowsNotification(secureNotificationInTheBackend.Id.ToString(), new Dictionary<string, string> {{"X-WNS-Type", "wns/raw"}});
                //await Notifications.Instance.Hub.SendNotificationAsync(rawNotificationToBeSent, usernameTag);

                var toast = @"<toast><visual><binding template=""ToastGeneric""><text id=""1"">" + message+ "</text></binding></visual></toast>";
                await Notifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast, usernameTag);
                // apns
                //await Notifications.Instance.Hub.SendAppleNativeNotificationAsync("{\"aps\": {\"alert\": \"" + message + "\"}}", usernameTag);

                // gcm
                await Notifications.Instance.Hub.SendGcmNativeNotificationAsync("{\"data\": {\"message\": \"" + message + "\"}}", usernameTag);

                return true;
            }
            catch (Exception e) {
                return false;
            }
            //return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

  
}
