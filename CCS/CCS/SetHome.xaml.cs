using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace CCS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetHome : ContentPage
	{
        public SetHome()
		{
			InitializeComponent ();
            initmap();

        }

        private void initmap() {

            GlobalMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude), Distance.FromMiles(1)));
            MoveHome();

        }



        private void MoveHome() {
            try
            {
                if (Shared.ModelView.UIBinding.Default.HomeLocation.Longitude != 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Latitude != 0) {
                    Continue.IsVisible = true;
                }
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(GlobalMap.VisibleRegion.Center.Latitude, GlobalMap.VisibleRegion.Center.Longitude);
                Shared.ModelView.UIBinding.Default.HomeLocation = Shared.ModelView.UIBinding.Default.CurrentCenter;
            }
            catch (Exception) {
            }
            GlobalMap.Pins.Clear();
            
            var Home = new Pin
            {
                Position = new Position(Shared.ModelView.UIBinding.Default.HomeLocation.Latitude, Shared.ModelView.UIBinding.Default.HomeLocation.Longitude),
                Label = "My home"
            };
            
            GlobalMap.Pins.Add(Home);
            
        }

        private void MoveToHomeToCenter(object sender, EventArgs e)
        {
            MoveHome();
        }

        private async void SetHomeOnMap(object sender, EventArgs e)
        {
            if (await Shared.ViewModel.UserHome.SetHome() == true) {
                Continue.IsVisible = true;
            }
        }

        private void BackToMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}