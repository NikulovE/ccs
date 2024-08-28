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
    [Activity(Label = "Your profile")]
    public class ProfileActivity : Activity
    {
        bool needupdate = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profile);
            InitializeUI();
        }
        protected override void OnUserLeaveHint()
        {
            base.OnUserLeaveHint();
            UpdateProfile();
        }
        

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            UpdateProfile();
        }

       // override v

        async void UpdateProfile() {
            if (needupdate)
            {
                var output = FindViewById<TextView>(Resource.Id.SystemOut);
                var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
                await Shared.ViewModel.UserProfile.Update(progressBar, output);
                needupdate = false;
            };
        }

        void InitializeUI() {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var Fname = FindViewById<EditText>(Resource.Id.FirstName);
            Fname.Text = Shared.ModelView.UserProfile.Default.FirstName;
            Fname.AfterTextChanged += async (ev, ar) => {
                Shared.ModelView.UserProfile.Default.FirstName = Fname.Text;
                needupdate = true;
            };

            var Lname = FindViewById<EditText>(Resource.Id.LastName);
            Lname.Text = Shared.ModelView.UserProfile.Default.LastName;
            Lname.AfterTextChanged += async (ev, ar) => {
                Shared.ModelView.UserProfile.Default.LastName = Lname.Text;
                needupdate = true;
            };

            var Phone = FindViewById<EditText>(Resource.Id.PhoneBox);
            Phone.Text = Shared.ModelView.UserProfile.Default.Phone;
            Phone.AfterTextChanged += async (ev, ar) => {
                Shared.ModelView.UserProfile.Default.Phone = Phone.Text;
                needupdate = true;
            };

            var SetHome = FindViewById<Button>(Resource.Id.SetHome);
            SetHome.Click += (ev, ar) =>
            {
                StartActivity(typeof(SetHomeActivity));
            };
            var RoleSwitcher=FindViewById<ToggleButton>(Resource.Id.roleswitcher);
            RoleSwitcher.Checked = Shared.ModelView.UserProfile.Default.IsDriver;
            RoleSwitcher.CheckedChange += async (ev, ar) =>
            {                
                Shared.ModelView.UserProfile.Default.IsDriver = RoleSwitcher.Checked;
                await Shared.ViewModel.UserProfile.ChangeDriverMode(progressBar, output);
                var MyCar = FindViewById<Button>(Resource.Id.SetCar);
                if (Shared.ModelView.UserProfile.Default.IsDriver == true)
                {
                    MyCar.Visibility = ViewStates.Visible;
                }
                else {
                    MyCar.Visibility = ViewStates.Gone;
                }
            };

            var CarCar = FindViewById<Button>(Resource.Id.SetCar);
            CarCar.Click += (ev, ar) =>
            {
                StartActivity(typeof(SetCarActivity));
            };
            if (Shared.ModelView.UserProfile.Default.IsDriver == true)
            {
                CarCar.Visibility = ViewStates.Visible;
            }
            else
            {
                CarCar.Visibility = ViewStates.Gone;
            }


        }
    }
}