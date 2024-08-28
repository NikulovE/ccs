using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CCS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrganizationJoin : ContentPage
	{
		public OrganizationJoin ()
		{
			InitializeComponent ();
            OrgJointGrid.BindingContext = Shared.ModelView.UIBinding.Default;
        }

        private async void CheckEmail(object sender, EventArgs e)
        {
            if (await Shared.ViewModel.Organization.StartJoin(InputedMail.Text.Replace(" ", "")))
            {
                StartJoin.IsVisible = false;
                SendKey.IsVisible = false;
                CompleteJoin.IsVisible = true;
                ConfirmKey.IsVisible = true;
            }
        }

        private async void Join(object sender, EventArgs e)
        {
            if (await Shared.ViewModel.Organization.CompleteJoiner(InputedMail.Text.Split('@')[1], InputedKey.Text.Replace(" ", "")))
            {
                await Shared.ViewModel.Organization.LoadOrganizations();
                CompleteJoin.IsVisible = false;
                ConfirmKey.IsVisible = false;
                Success.IsVisible = true;
                ContinueS.IsVisible = true;
            };
        }

        private void Continue(object sender, EventArgs e)
        {
            if (Shared.ModelView.UserProfile.Default.OfficeID == -1)
            {
                App.Current.MainPage = new CCS.SelectOffice();
                return;
            }
            else
            { App.Current.MainPage = new MainPage(); }
        }


    }
}