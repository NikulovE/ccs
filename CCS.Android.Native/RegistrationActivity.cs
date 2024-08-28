using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Shared;
using WindowsAzure.Messaging;

namespace CCS.Android.Native
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);      
            Shared.Actions.showConfirmationGrid = ShowConfirmationGrid;
            Shared.Actions.showFillingProfileGrid = ShowFillingProfileGrid;
            Shared.Actions.showRegistrationGrid = ShowRegistrationGrid;
            ShowRegistrationGrid();
        }



        private void ShowRegistrationGrid()
        {
            SetContentView(Resource.Layout.Registration);
            var button = FindViewById<Button>(Resource.Id.SendKey);
            button.Click += (ev, ar) =>
            {
                var progressbar = FindViewById<ProgressBar>(Resource.Id.RegistrationProgressBar);
                var output = FindViewById<TextView>(Resource.Id.OutRegistration);
                Shared.ViewModel.Registration.CheckEmail(FindViewById<EditText>(Resource.Id.InputedMail), button, progressbar, output);
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

        private async void ShowFillingProfileGrid()
        {
            SetContentView(Resource.Layout.FillingProfile);
            
            var progressbar = FindViewById<ProgressBar>(Resource.Id.FillingProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OutFilling);
            await Shared.ViewModel.UserProfile.Load(progressbar, output);
            StartupActivity.isProfileLoaded = true;
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

        

        private async void ShowSelectDriverModeGrid()
        {
            SetContentView(Resource.Layout.SelectRole);
            var progressbar = FindViewById<ProgressBar>(Resource.Id.SelectRoleProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OutSelectRole);
            if (!StartupActivity.isProfileLoaded)
            {
                await Shared.ViewModel.UserProfile.Load(progressbar, output);
                StartupActivity.isProfileLoaded = true;
            }
            output.Text = "";
            var tobewalker = FindViewById<Button>(Resource.Id.SelectWalker);
            tobewalker.Click += async (ev, ar) =>
            {
                Shared.ModelView.UserProfile.Default.IsDriver = false;
                if (await Shared.ViewModel.UserProfile.Update(progressbar, output))
                {
                    try
                    {
                        StartActivity(typeof(MainActivity));
                        this.Finish();
                    }
                    catch { }
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
                    try
                    {                        
                        StartActivity(typeof(MainActivity));
                        this.Finish();
                    }
                    catch { }
                }
                else
                {
                    output.Text = Shared.ConvertMessages.Message("x50009");
                }
            };
        }

    }
}