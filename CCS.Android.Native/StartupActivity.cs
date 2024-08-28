using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CCS.Android.Native
{
    [Activity(Label = "Commute Car Sharing", MainLauncher = true, Icon = "@mipmap/icon")]
    public class StartupActivity : Activity
    {
        public static bool isProfileLoaded = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            //Shared.Actions.showConfirmationGrid = ShowConfirmationGrid;
            Shared.Actions.showFillingProfileGrid = ShowFillingProfileGrid;
            Shared.Actions.showRegistrationGrid = ShowRegistrationGrid;
            switch (Shared.Model.LocalStorage.ProfileVersion)
            {
                case 0:
                    ShowRegistrationGrid();
                    break;
                case 1:
                    ShowConfirmationGrid();
                    break;
                case 2:
                    ShowFillingProfileGrid();
                    break;
                case 3:
                    ShowSelectDriverModeGrid();
                    break;
                case 247:
                    ShowSelectDriverModeGrid();
                    break;
                default:
                    ShowMainGrid();
                    break;
            }
        }

        private void ShowMainGrid()
        {
            StartActivity(typeof(MainActivity));
            this.Finish();
        }

        private async void ShowSelectDriverModeGrid()
        {
            SetContentView(Resource.Layout.SelectRole);
            var progressbar = FindViewById<ProgressBar>(Resource.Id.SelectRoleProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OutSelectRole);
            if (!isProfileLoaded)
            {
                await Shared.ViewModel.UserProfile.Load(progressbar, output);
                isProfileLoaded = true;
            }
            output.Text = "";
            var tobewalker = FindViewById<Button>(Resource.Id.SelectWalker);
            tobewalker.Click += async (ev, ar) =>
            {
                Shared.ModelView.UserProfile.Default.IsDriver = false;
                if (await Shared.ViewModel.UserProfile.Update(progressbar, output))
                {
                    ShowMainGrid();
                }
                else
                {
                    output.Text = Shared.ConvertMessages.Message("x50009");
                }
            };
            var tobedriver = FindViewById<Button>(Resource.Id.SelectDriver);
            tobedriver.Click += async (ev, ar) =>
            {
                Shared.ModelView.UserProfile.Default.IsDriver = true;
                if (await Shared.ViewModel.UserProfile.Update(progressbar, output))
                {
                    ShowMainGrid(); 
                }
                else
                {
                    output.Text = Shared.ConvertMessages.Message("x50009");
                }
            };
        }

        private async void ShowFillingProfileGrid()
        {
            SetContentView(Resource.Layout.FillingProfile);
            var progressbar = FindViewById<ProgressBar>(Resource.Id.FillingProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OutFilling);
            await Shared.ViewModel.UserProfile.Load(progressbar, output);
            isProfileLoaded = true;
            FindViewById<EditText>(Resource.Id.FirstName).Text = Shared.ModelView.UserProfile.Default.FirstName;
            FindViewById<EditText>(Resource.Id.LastName).Text = Shared.ModelView.UserProfile.Default.LastName;
            FindViewById<EditText>(Resource.Id.PhoneBox).Text = Shared.ModelView.UserProfile.Default.Phone;
            var button = FindViewById<Button>(Resource.Id.ContinueFillingProfile);
            button.Click += async (ev, ar) =>
            {
                Shared.ModelView.UserProfile.Default.FirstName = FindViewById<EditText>(Resource.Id.FirstName).Text;
                Shared.ModelView.UserProfile.Default.LastName = FindViewById<EditText>(Resource.Id.LastName).Text;
                Shared.ModelView.UserProfile.Default.Phone = FindViewById<EditText>(Resource.Id.PhoneBox).Text;
                if (await Shared.ViewModel.UserProfile.Update(progressbar, output))
                {
                    ShowSelectDriverModeGrid();
                }
                else
                {
                    output.Text = Shared.ConvertMessages.Message("x50009");
                }
            };

        }

        private void ShowConfirmationGrid()
        {
            SetContentView(Resource.Layout.Confirmation);            
            var button = FindViewById<Button>(Resource.Id.ConfirmPassword);
            button.Click += (ev, ar) =>
            {
                var progressbar = FindViewById<ProgressBar>(Resource.Id.ConfirmationProgressBar);
                var output = FindViewById<TextView>(Resource.Id.OutConfirmation);
                Shared.ViewModel.Registration.Confirm(button, FindViewById<EditText>(Resource.Id.PasswordFromMail), progressbar, output);

            };
            var backbutton = FindViewById<Button>(Resource.Id.BackToRegistartionGrid);
            backbutton.Click += (ev, ar) =>
            {
                ShowRegistrationGrid();
            };
        }


        void ShowRegistrationGrid()
        {
            StartActivity(typeof(RegistrationActivity));
            this.Finish();

        }
    }
}