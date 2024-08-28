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
	public partial class RoleSelector : ContentPage
	{
		public RoleSelector ()
		{
			InitializeComponent ();
            initAsync();


        }

        async void initAsync()
        {
            if (!App.isProfileLoaded)
            {
                await Shared.ViewModel.UserProfile.Load();
                App.isProfileLoaded = true;
            }
            Shared.ModelView.UIBinding.Default.OutPut = "";
        }

        private async void SelectedDriverModePassenger(object sender, EventArgs e)
        {
            Shared.ModelView.UserProfile.Default.IsDriver = false;
            if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
            {
                Shared.View.General.ShowNextRegistrationGrid(SelectDriverModeGrid, ShowMainGrid);
            }
        }

        private void ShowMainGrid()
        {
            App.Current.MainPage = new MainPage();
        }

        private async void SelectedDriverModeDriver(object sender, EventArgs e)
        {
            Shared.ModelView.UserProfile.Default.IsDriver = true;
            if (await Shared.ViewModel.UserProfile.ChangeDriverMode())
            {
                Shared.View.General.ShowNextRegistrationGrid(SelectDriverModeGrid, ShowMainGrid);
            }
        }
    }
}