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
	public partial class EditProfile : ContentPage
	{
		public EditProfile ()
		{
			InitializeComponent ();
            LoadProfile();
            
            // DriverReward.SelectedIndex = Shared.ModelView.UserProfile.Default.Payment
            RoleToggler.Toggled += SwitchRole;

        }

        private async void LoadProfile() {
            await Shared.ViewModel.UserProfile.Load();
            ProfileGrid.BindingContext = Shared.ModelView.UserProfile.Default;
        }
        private void CheckOrganizations(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Organizations());
        }

        private void SetHome(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SetHome());
        }

        private async void SwitchRole(object sender, ToggledEventArgs e)
        {
            if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
            {

            }
        }

        private void BuildRoutes(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BuildRoutes());
        }

        private async void SaveProfile(object sender, EventArgs e)
        {
            await Shared.ViewModel.UserProfile.Update();
        }

        private void CheckCarDetails(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new EditCar());
        }

        private void CheckSchedule(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Schedule());
        }
    }
}