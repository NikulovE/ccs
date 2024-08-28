using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CCS.Android.Native
{
    [Activity(Label = "Trips")]
    public class TripsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Trips);
            Shared.Actions.refreshTrips = LoadTrips;
            LoadTrips();
        }

        private void LoadTrips()
        {
            RunOnUiThread(async () =>
            {
                var output = FindViewById<TextView>(Resource.Id.SystemOut);
                var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
                if (await Shared.ViewModel.Trips.LoadTrips(progressBar, output))
                {
                    ShowTrips();
                }
            });
        }

        void ShowTrips() {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var AllTrips = FindViewById<ScrollView>(Resource.Id.Trips);
            AllTrips.RemoveAllViews();
            var imgViewParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
            var TripsStack = new LinearLayout(this);
            foreach (var trip in Shared.ModelView.UIBinding.Default.TripOffers)
            {                
                TripsStack.LayoutParameters = imgViewParams;
                TripsStack.Orientation = Orientation.Vertical;

                var ConverationWIth = trip.Companion;                

                var SingleTrip = new GridLayout(this);
                SingleTrip.ColumnCount = 2;
                //SendMessageStack.LayoutParameters = imgViewParams;

                GridLayout.LayoutParams param2 = new GridLayout.LayoutParams();
                param2.ColumnSpec = GridLayout.InvokeSpec(0);
                param2.SetGravity(GravityFlags.Fill);
                param2.SetMargins(0, 12, 0, 0);
                //param2.or
                var Companion = new TextView(this);
                
                Companion.TextSize = 18;
                
                //NewMessage.Gravity = GravityFlags.Fill;
                Companion.LayoutParameters = param2;
                Companion.TextAlignment = TextAlignment.Center;
                Companion.Text = trip.Companion;




                var AproveRejectStack = new LinearLayout(this);
                AproveRejectStack.Orientation = Orientation.Horizontal;


                var Approve = new Button(this);
                Approve.LayoutParameters=new LinearLayout.LayoutParams(100, 100);
                Approve.Text = "v";
                if (!trip.IsCanBeAccepted)
                {
                    Approve.Enabled = false;
                    if (!trip.Confirmed) Approve.Text = "?";
                }
                Approve.Click += async (ev, ar) => {
                    await Shared.ViewModel.Trips.AcceptTrip(progressBar, output, trip.OfferID);
                };
                var Reject = new Button(this);
                Reject.Text = "x";
                Reject.Click += async (ev, ar) => {
                    await Shared.ViewModel.Trips.RejectTrip(progressBar, output, trip.OfferID);
                };
                Reject.LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                GridLayout.LayoutParams param = new GridLayout.LayoutParams();
                param.ColumnSpec = GridLayout.InvokeSpec(1);
                AproveRejectStack.LayoutParameters = param;

                AproveRejectStack.AddView(Approve);
                AproveRejectStack.AddView(Reject);
                SingleTrip.AddView(Companion);
                SingleTrip.AddView(AproveRejectStack);
                

                TripsStack.AddView(SingleTrip);
            }
            AllTrips.AddView(TripsStack);
        }
    }
}