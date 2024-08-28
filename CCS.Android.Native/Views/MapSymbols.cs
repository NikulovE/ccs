using Android;
using Android.Content.Res;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace Shared.View
{
    class MapsSymbols
    {
        public static async void ShowCompanions(GoogleMap Maplayer, List<Model.OnMapPoint> Points)
        {

            
                foreach (var point in Points)
                {
                    MarkerOptions PointOnMap = new MarkerOptions();
                    PointOnMap.SetPosition(new LatLng(point.Latitude, point.Longtitude));

                    if (point.IsHome)
                    {
                        PointOnMap.SetTitle("Click to send invite");
                        PointOnMap.SetSnippet(await CompanionTooltipAsync(point.UID));

                        //PointOnMap.InvokeZIndex(Convert.ToSingle(point.UID));
                        //Poi
                        BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(CCS.Android.Native.Resource.Drawable.r_p_home);
                        PointOnMap.SetIcon(icon);
                        //Maplayer.AddMarker(PointOnMap);
                        
                        //PointOnMap.Clicked += async (ev, ar) =>
                        //{
                        //    await ViewModel.Trips.Send(point.UID, 0, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome);
                        //};

                        //PointOnMap.Label = "Home. Click to send invite";
                        //PointOnMap.Address = await CompanionTooltipAsync(point.UID);

                        //Maplayer.Pins.Add(PointOnMap);
                    }
                    else
                    {
                        PointOnMap.SetTitle("Click to send invite");
                        PointOnMap.SetSnippet(await CompanionTooltipAsync(point.UID));
                       
                        //PointOnMap.InvokeZIndex(Convert.ToSingle(point.UID));
                        //PointOnMap.InvokeZIndex(Converter(point.PointID, point.UID));
                        BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(CCS.Android.Native.Resource.Drawable.r_p);
                        PointOnMap.SetIcon(icon);
                        
                        //PointOnMap.Clicked += async (ev, ar) =>
                        //{
                        //    await ViewModel.Trips.Send(point.UID, point.PointID, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome);
                        //};
                        //PointOnMap.Label = "Click to send invite";
                        //PointOnMap.Address = await CompanionTooltipAsync(point.UID);
                        //Maplayer.Pins.Add(PointOnMap);
                    }
                    Marker marker = Maplayer.AddMarker(PointOnMap);
                    if (point.IsHome) marker.Tag = point.UID.ToString() + ";true;0";
                    else marker.Tag = point.UID.ToString() + ";false;" + point.PointID.ToString();
                    //marker.
                }
                //Maplayer.InfoWindowClick += (ev, ar) =>
                //{
                //    var UID = Convert.ToInt32(ar.Marker.ZIndex);
                //    //var u=int. ar.Marker.ZIndex;
                //};
            
        }

        public static float Converter(int a, int b) {
            try
            {
                int left = b;
                int right = a;
                decimal Both;
                bool ConverstionSucceed = decimal.TryParse((left + "." + right), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Both);
                if (ConverstionSucceed) return float.Parse(Both.ToString());
                else return 0;
                //return Both;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        static async System.Threading.Tasks.Task<string> CompanionTooltipAsync(int UID)
        {
            var user = await ViewModel.Companions.GetCompanionInfo(UID);
            var ToolTipPanel = "";

            var Name = user.FirstName + " " + user.LastName + " ";
            if (Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome) Name += Shared.ConvertMessages.Message("startat") + user.ToHomeHlp;
            else {
                Name += Shared.ConvertMessages.Message("startat") + user.ToWorkHlp;
            }
            ToolTipPanel += Name;

            if (!Shared.ModelView.UserProfile.Default.IsDriver)
            {
                var CarInfo = user.Brand + " " + user.Model + " | " + ConvertMessages.Message("Places") + ":" + user.Places.ToString()+' ';
                ToolTipPanel+=CarInfo;
            }
            var Payable = PaymentConverter(user.Payment);
            ToolTipPanel+=Payable;
            return ToolTipPanel;

        }
        
        static String PaymentConverter(int Payment)
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

