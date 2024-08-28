using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using System.Globalization;

namespace WebAPI2.Hubs
{
    public class ChatHub : Hub
    {

        public void SendInviteToUser(int ToUserUID)
        {
            Clients.Group(ToUserUID.ToString(CultureInfo.InvariantCulture)).AddedTrip();
        }

        public void UpdateTripToUser(int ToUserUID)
        {
            Clients.Group(ToUserUID.ToString(CultureInfo.InvariantCulture)).UpdateTrip();
        }

        public void SendMessageToUser(int ToUserUID)
        {
            Clients.Group(ToUserUID.ToString(CultureInfo.InvariantCulture)).AddedMessage();
        }

        public void Register(int UserUID)
        {
            Groups.Add(Context.ConnectionId, UserUID.ToString(CultureInfo.InvariantCulture));

        }
    }
}


