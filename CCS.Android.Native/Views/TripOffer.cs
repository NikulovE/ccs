using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Shared.View
{
    class TripOffer
    {
        //public static List<Marker> Trips = new List<Marker>();

        public async static void ShowTripsOnMap(GoogleMap Maplayer)
        {
            //Maplayer.c
            foreach (var trip in Shared.ModelView.UIBinding.Default.TripOffers)
            {
                MarkerOptions PointOnMap = new MarkerOptions();
                PointOnMap.SetPosition(new LatLng(trip.Latitude, trip.Longtitude));
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(CCS.Android.Native.Resource.Drawable.trip);
                PointOnMap.SetIcon(icon);
                var infoline = "";
                try
                {
                    var companionInfo = await ViewModel.Companions.GetCompanionInfo(trip.CompanionID);
                    infoline = ConvertMessages.Message("Phone") + ":" + companionInfo.Phone + " " + PaymentConverter(companionInfo.Payment) + " " + new DateTime(trip.StartTime).ToString() + " " + companionInfo.Brand + " " + companionInfo.Model + " " + companionInfo.GovNumber;

                }
                catch { }
                PointOnMap.SetTitle(trip.Companion);
                PointOnMap.InvokeZIndex(9000);
                PointOnMap.SetSnippet(infoline);
                try
                {
                   Maplayer.AddMarker(PointOnMap);
                    //Trips.Add(Point);
                }
                catch { }
                

            }
        }

        public static String PaymentConverter(int Payment)
        {
            switch (Payment)
            {
                case 0:
                    return ConvertMessages.Message("Pay");
                case 1:
                    return ConvertMessages.Message("FreePay");
                case 2:
                    return ConvertMessages.Message("NotDecided");
                default:
                    return "";
            };

        }

    }
}
