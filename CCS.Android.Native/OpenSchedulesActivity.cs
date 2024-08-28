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
    [Activity(Label = "Schedules")]
    public class OpenSchedulesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Schedules);
            Shared.Actions.refreshSchedules = Rewrite;
            LoadSchedules();
        }

        async void LoadSchedules() {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.UserSchedule.Load(progressBar, output, FindViewById<ScrollView>(Resource.Id.Schedules), this);
        }

        void Rewrite() {
            Shared.ViewModel.UserSchedule.Rewrite(FindViewById<ScrollView>(Resource.Id.Schedules), this);
        }
    }
}