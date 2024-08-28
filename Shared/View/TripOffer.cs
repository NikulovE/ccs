using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;



#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Windows.UI.Xaml;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif
#else

#if WPF
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows.Media;
#endif

using System.Windows;

#if XAMARIN
using Xamarin.Forms;
using Xamarin.Forms.Maps;
#endif
#endif


namespace Shared.View
{
    class TripOffer
    {
#if XAMARIN

        public static void ShowTripPointsOnMap(Map Map) {
            Map.Pins.Clear();
            try
            {
                foreach (var trip in Shared.ModelView.UIBinding.Default.TripOffers)
                {
                    try
                    {
                        var Pos = new Position(trip.Latitude, trip.Longtitude);
                        var TripPoint = new Pin { Position = Pos };
                        TripPoint.Label = trip.Companion;
                        var culturex = CultureInfo.CurrentCulture;
                        var s = new DateTime(trip.StartTime);
                        TripPoint.Address = trip.IsToHome ? "to home at " + s.ToString(culturex) : "to work at " + s.ToString(culturex);
                        Map.Pins.Add(TripPoint);
                    }
                    catch (Exception) { }
                }
            }
            catch { }
        }

        public static void FillTrips(ListView TripsStack)
        {
        }
#else

        public static StackPanel Trip(Model.TripOffer tripoffer)
        {
            var TripStack = new StackPanel { Orientation = Orientation.Horizontal };
            var Companion = new TextBlock { Text = tripoffer.Companion, VerticalAlignment = VerticalAlignment.Center, FontSize = 18, Margin=new Thickness(5) };

            var isConfirmed = new Button { Content = tripoffer.Confirmed ? "\uE0E7" : "\uE897", FontFamily = Shared.View.General.SetSegoeMDLFont(), IsEnabled = tripoffer.IsCanBeAccepted, Visibility = tripoffer.IsCanBeAccepted == true ? Visibility.Collapsed : Visibility.Visible , Margin = new Thickness(5) };

            var ConfirmedSym = new Button { Content = "\uE0E7", FontFamily = View.General.SetSegoeMDLFont(), Visibility = tripoffer.IsCanBeAccepted == true ? Visibility.Visible : Visibility.Collapsed, Margin = new Thickness(5) };
            var UnConfirmedSym = new Button { Content = "\uE10A", FontFamily = View.General.SetSegoeMDLFont(), Margin = new Thickness(5) };

            var isToHome = new TextBlock { Text = tripoffer.IsToHome ? "\uE821 \uEA62 \uE80F" : "\uE80F \uEA62 \uE821", FontFamily = View.General.SetSegoeMDLFont(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16 };
            var StartTime = new TextBlock { Text =(new DateTime(tripoffer.StartTime)).Hour < 12 ? " \uED39" : " \uEC46", FontFamily = View.General.SetSegoeMDLFont(), VerticalAlignment =VerticalAlignment.Center, FontSize = 16 };



            TripStack.Children.Add(Companion);
            TripStack.Children.Add(isConfirmed);
            TripStack.Children.Add(ConfirmedSym);
            TripStack.Children.Add(UnConfirmedSym);
            TripStack.Children.Add(isToHome);
            TripStack.Children.Add(StartTime);

            ConfirmedSym.Click += async (ev, ar) =>
            {
                await Shared.ViewModel.Trips.AcceptTrip(tripoffer.OfferID);
            };
            UnConfirmedSym.Click += async (ev, ar) =>
            {
                await Shared.ViewModel.Trips.RejectTrip(tripoffer.OfferID);
            };
            TripStack.Margin = new Thickness(6);
            return TripStack;
        }

        public static void FillTrips(ItemsControl TripsStack)
        {
            TripsStack.Items.Clear();
            foreach (var tripoffer in Shared.ModelView.UIBinding.Default.TripOffers)
            {
                TripsStack.Items.Add(Trip(tripoffer));
            }

        }
#if WINDOWS_UWP
        public static void ShowTripPointsOnMap(MapControl Map)
#else
        public static void ShowTripPointsOnMap(MapLayer Map)
#endif

        {
            Map.Children.Clear();
            foreach (var trip in Shared.ModelView.UIBinding.Default.TripOffers)
            {
                var TripPoint = View.MapsSymbols.TripPoint(trip);
#if WINDOWS_UWP
                MapControl.SetLocation(TripPoint, new Model.Location { Latitude = trip.Latitude, Longitude = trip.Longtitude });
#else
                MapLayer.SetPosition(TripPoint, new Model.Location { Latitude = trip.Latitude, Longitude = trip.Longtitude });
#endif

                Map.Children.Add(TripPoint);
            }
        }
        //public FrameworkElement OfferLine(CarBuddyServiceAPI.TripOffer Offer) {
        //    var line = new StackPanel { Orientation = Orientation.Horizontal };
        //    var Name = new TextBlock { Text = ModelView.UserProfile.Default.IsDriver ? Offer.Driver : Offer.Passenger };
        //    line.Children.Add(Name);
        //    return line;
        //}
#endif
        }
}
