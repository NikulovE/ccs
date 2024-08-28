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
    [Activity(Label = "Join organization")]
    public class OrganizationJoinActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OrganizationJoin);
            InitializeUI();

        }

        void InitializeUI() {
            var startOrgJoin = FindViewById<Button>(Resource.Id.SendKey);
            startOrgJoin.Click += CheckEmail;

            var JoinOrg = FindViewById<Button>(Resource.Id.DoJoin);
            JoinOrg.Click += Join;

            var Continue = FindViewById<Button>(Resource.Id.OrgContinue);
            Continue.Click += ContinuContinuee_Click;

            var RegisterNEwOne = FindViewById<Button>(Resource.Id.RegOrganiztion);
            RegisterNEwOne.Click += RegisterNEwOne_Click;
        }

        private void RegisterNEwOne_Click(object sender, EventArgs e)
        {
            var output = FindViewById<TextView>(Resource.Id.OrgOutPut).Text = "not implemented";
        }

        private void ContinuContinuee_Click(object sender, EventArgs e)
        {
            if (Shared.ModelView.UserProfile.Default.OfficeID == -1)
            {
                StartActivity(typeof(SelectOfficeActivity));
                return;
            }
            else
            { StartActivity(typeof(MainActivity));
                this.Finish();
            }
            
        }

        private async void Join(object sender, EventArgs e)
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.OrgJoinProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OrgOutPut);
            if (await Shared.ViewModel.Organization.CompleteJoiner((FindViewById<EditText>(Resource.Id.OrgJoinInputedMail)).Text.Split('@')[1], (FindViewById<EditText>(Resource.Id.OrgJoinInputedKey)).Text.Replace(" ", ""), progressbar,output))
            {
                await Shared.ViewModel.Organization.LoadOrganizations(progressbar, output);

                FindViewById<LinearLayout>(Resource.Id.CompleteOrgJoin).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.DoJoin).Visibility = ViewStates.Gone;
                FindViewById<TextView>(Resource.Id.Joined).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.OrgContinue).Visibility = ViewStates.Visible;
            };
        }
        
        private async void CheckEmail(object sender, EventArgs e)
        {
            var progressbar = FindViewById<ProgressBar>(Resource.Id.OrgJoinProgressBar);
            var output = FindViewById<TextView>(Resource.Id.OrgOutPut);

            if (await Shared.ViewModel.Organization.StartJoin((FindViewById<EditText>(Resource.Id.OrgJoinInputedMail)).Text.Replace(" ", ""), progressbar, output))
            {
                FindViewById<LinearLayout>(Resource.Id.StartOrgJoin).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.SendKey).Visibility = ViewStates.Gone;
                FindViewById<LinearLayout>(Resource.Id.CompleteOrgJoin).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.DoJoin).Visibility = ViewStates.Visible;
                FindViewById<TextView>(Resource.Id.WorkBoxWorry).Visibility = ViewStates.Gone;

            }
            else {
                FindViewById<Button>(Resource.Id.RegOrganiztion).Visibility = ViewStates.Visible;
            }
        }        
    }
}