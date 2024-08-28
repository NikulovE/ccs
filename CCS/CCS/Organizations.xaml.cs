using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Organizations : ContentPage
	{

        public Organizations ()
		{
			InitializeComponent ();
            OrganizationGrid.BindingContext = Shared.ModelView.UserOrganizations.Default;
            LoadUserOrgs();


        }

        async void LoadUserOrgs() {
            await Shared.ViewModel.Organization.LoadOffices();
            await Shared.ViewModel.Organization.LoadOrganizations();
            OfficeList.SelectedIndex = Shared.ModelView.UIBinding.Default.SelectedOffice;
            
        }

        private void OfficeList_SelectedIndexChanged(object sender, EventArgs e)
        {

            Shared.ModelView.UserProfile.Default.OfficeID = Shared.ModelView.UserOrganizations.Default.officelst.First(req => req.Name == OfficeList.SelectedItem.ToString()).ID;
            Shared.ModelView.UIBinding.Default.SelectedOffice = OfficeList.SelectedIndex;
            SaveAccount();
            OfficeList.SelectedIndexChanged -= OfficeList_SelectedIndexChanged;
        }

        private void StartSelectOffice(object sender, FocusEventArgs e)
        {
            OfficeList.SelectedIndexChanged += OfficeList_SelectedIndexChanged;
        }

        private async void SaveAccount() {
            await Shared.ViewModel.UserProfile.Update();
        }

        private void JoinOrganization(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new OrganizationJoin());
        }
    }
}