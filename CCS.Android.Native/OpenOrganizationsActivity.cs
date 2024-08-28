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
    [Activity(Label = "Organizations")]
    public class OpenOrganizationsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Organizations);
            Shared.Actions.refreshOffices = LoadOffices;
            LoadOffices();
            var Join = FindViewById<Button>(Resource.Id.Join);
            Join.Click += (ev, ar) => {
                StartActivity(typeof(OrganizationJoinActivity));
            };
        }
        async void LoadOrganization()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.Organization.LoadOrganizations(progressBar, output);

            var OrganizationsScroll = FindViewById<ScrollView>(Resource.Id.Organizations);
            OrganizationsScroll.RemoveAllViews();
            var orgstack = new LinearLayout(this);
            var imgViewParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
            orgstack.LayoutParameters = imgViewParams;
            foreach (var organization in Shared.ModelView.UserOrganizations.Default.OrganizationsList)
            {
                var SingleOrgStack = new GridLayout(this);
                SingleOrgStack.ColumnCount = 2;
                GridLayout.LayoutParams param2 = new GridLayout.LayoutParams();
                param2.ColumnSpec = GridLayout.InvokeSpec(0);
                param2.SetMargins(0, 5, 0, 10);
                param2.SetGravity(GravityFlags.Fill);
                SingleOrgStack.LayoutParameters = imgViewParams;

                var NewMessage = new TextView(this);
                //NewMessage.Gravity = GravityFlags.Fill;
                NewMessage.LayoutParameters = param2;
                NewMessage.Text = organization.Name;
                NewMessage.TextSize = 24;
                //NewMessage.SetTextAppearance(Resource.Styleable);


                var fallBut = new LinearLayout(this);
                fallBut.Orientation = Orientation.Horizontal;
                GridLayout.LayoutParams param = new GridLayout.LayoutParams();
                param.ColumnSpec = GridLayout.InvokeSpec(1);
                fallBut.LayoutParameters = param;





                var AddOffice = new Button(this);
                AddOffice.Text = "add office";
                AddOffice.Click += (ev, ar) =>
                {
                    Shared.ModelView.UserOrganizations.Default.SelectedOrganization = organization.TeamID;
                    StartActivity(typeof(AddOfficeActivity));
                };

                var LeaveOrganization = new Button(this);
                LeaveOrganization.Text = "leave";
                LeaveOrganization.Click += (ev, ar) =>
                {
                    Shared.ModelView.UserOrganizations.Default.SelectedOrganization = organization.TeamID;
                    Shared.ViewModel.Organization.LeaveOrganization(progressBar, output);
                };
//                if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count == 1) {
//#if DEBUG
//#else
//                    LeaveOrganization.Enabled = false;
//#endif
//                }

                fallBut.AddView(AddOffice);
                fallBut.AddView(LeaveOrganization);

                SingleOrgStack.AddView(NewMessage);
                SingleOrgStack.AddView(fallBut);
                orgstack.AddView(SingleOrgStack);
            }
            OrganizationsScroll.AddView(orgstack);
        }

        async void LoadOffices()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.Organization.LoadOffices(progressBar, output, true);
            var CurrentOf = FindViewById<Button>(Resource.Id.currentoffice);
            CurrentOf.Text = Shared.ModelView.UserOrganizations.Default.CurrentOffice.Name;
            CurrentOf.Click += (ev, ar) =>
            {
                StartActivity(typeof(SelectOfficeActivity));
            };
            LoadOrganization();


        }
    }
}