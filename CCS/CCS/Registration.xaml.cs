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
	public partial class Registration : ContentPage
	{
		public Registration ()
		{
			InitializeComponent ();
            
            Shared.Model.LocalStorage.Reset();
            Shared.Model.LocalStorage.ProfileVersion = 0;
            RegistrationGrid.BindingContext = Shared.ModelView.UIBinding.Default;

        }

        private void CheckEmail(object sender, EventArgs e)
        {
            Shared.ModelView.UIBinding.Default.OutPut = "";
            Shared.ViewModel.Registration.CheckEmail(InputedMail, SendKey, RegistrationGrid, ShowConfirmationGrid);
        }

        private void ShowConfirmationGrid()
        {
            Navigation.PushModalAsync(new Confirmation());
        }
    }
}