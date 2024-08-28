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
	public partial class Messages : ContentPage
	{
		public Messages ()
		{
			InitializeComponent ();
            Loadmessages();           

        }

        async void Loadmessages() {
            await Shared.ViewModel.Messages.Load();
            MessagesStack.ItemsSource = Shared.ModelView.UIBinding.Default.Messages;
        }

        private async void SendMessage(object sender, EventArgs e)
        {
            var but = sender as Button;
            int WithUID = int.Parse(but.ClassId);
            var message = Shared.ModelView.UIBinding.Default.Messages.First(req => req.WithUID == WithUID).NewMessage;
            await Shared.ViewModel.Messages.Send(WithUID, message);

            await Navigation.PushModalAsync(new Messages(),false);
            
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(false);
        }
    }
}