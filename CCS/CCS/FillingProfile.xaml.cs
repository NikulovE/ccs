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
	public partial class FillingProfile : ContentPage
	{
		public FillingProfile ()
		{
			InitializeComponent ();
            NickName.BindingContext = Shared.ModelView.UserProfile.Default;
            out1.BindingContext = Shared.ModelView.UIBinding.Default;
            Shared.ModelView.UIBinding.Default.OutPut = "";
            LoadProfileAsync();
        }

        async void LoadProfileAsync()
        {
            if (await Shared.ViewModel.UserProfile.Load()) {
                App.isProfileLoaded = true;
            }
        }

        private async void CompleteFillingProfile(object sender, EventArgs e)
        {
            if (FNameBox.Text == "")
            {

                Shared.ModelView.UIBinding.Default.OutPut += Shared.ConvertMessages.Message("x50005");
                return;
            }
            if (LNameBox.Text == "")
            {
                Shared.ModelView.UIBinding.Default.OutPut += Shared.ConvertMessages.Message("x50006");
                return;
            }
            if (PhoneBox.Text != "")
            {
                try
                {
                    long.Parse(PhoneBox.Text);
                }
                catch (Exception)
                {
                    Shared.ModelView.UIBinding.Default.OutPut = Shared.ConvertMessages.Message("x50008");
                    return;
                }
            }
            if (FNameBox.Text != "" && LNameBox.Text != "")
            {

                if (await Shared.ViewModel.UserProfile.Update())
                {
                    Shared.View.General.ShowNextRegistrationGrid(FillingProfileGrid, ShowSelectDriverMode);
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = Shared.ConvertMessages.Message("x50009");
                }
            }
        }

        private void ShowSelectDriverMode()
        {
            App.Current.MainPage = new RoleSelector();
        }

        private void CheckPhoneNumber(object sender, TextChangedEventArgs e)
        {

        }

    }
}