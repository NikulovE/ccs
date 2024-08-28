using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Confirmation : ContentPage
	{
		public Confirmation ()
		{
			InitializeComponent ();
            Shared.ModelView.UIBinding.Default.OutPut = "";
            ConfirmationGrid.BindingContext = Shared.ModelView.UIBinding.Default;
        }

        private void ConfirmRegistration(object sender, EventArgs e)
        {
            Shared.ViewModel.Registration.Confirm(ConfirmPassword, PasswordFromMail, ConfirmationGrid, ShowFilliningProfileGrid);
        }

        private void ShowFilliningProfileGrid()
        {
            try
            {
                Navigation.PopToRootAsync();
            }
            catch (Exception) { }
            App.Current.MainPage = new CCS.FillingProfile();
        }

        private void BackToRegistration(object sender, EventArgs e)
        {
            Shared.View.General.ShowPreviousRegistrationGrid(ConfirmationGrid, BackToRegistration);
        }

        private void BackToRegistration()
        {
            App.Current.MainPage = new CCS.Registration();
        }

    }
}