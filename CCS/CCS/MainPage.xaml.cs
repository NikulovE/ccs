using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace CCS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
            //Shared.Actions.showTripPointonMap = ShowTripPoint;
            //Shared.Actions.showTripOffers = ShowTripOffers;
            //Shared.Actions.initializeMap = initmap;
            //Shared.Actions.refreshTrips = LoadTrips;
            //Shared.Actions.loadHome = LoadHome;
            //Shared.Actions.initializeMap();
            //Shared.Actions.refreshTrips();
        }

        private async void LoadTrips() {
            await Shared.ViewModel.Trips.LoadTrips();
            Shared.Actions.showTripPointonMap();


        }
        private async void LoadHome()
        {
            await Shared.ViewModel.UserHome.LoadHome();
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0)
            {
                App.Current.MainPage = new CCS.SetHome();
                return;
            };
        }
        public void ShowTripPoint()
        {
            Shared.View.TripOffer.ShowTripPointsOnMap(Map);
        }

        public void ShowTripOffers()
        {
           // Shared.View.TripOffer.FillTrips(Trips);

        }

        private void EditProfile(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new EditProfile());
        }

        public async void LoadUserOffice()
        {
            //OfficesOnMap.Visibility = Visibility.Collapsed;
            var SelectedOffice = new Xamarin.Forms.Maps.Map();
            await Shared.ViewModel.Organization.LoadOffices(true);
        }

        private void initmap()
        {
            if (Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude != 0 && Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude != 0)
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude), Distance.FromMiles(1)));
            }

        }

        private void StartFindCompaions(object sender, EventArgs e)
        {

            try
            {
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(Map.VisibleRegion.Center.Latitude, Map.VisibleRegion.Center.Longitude);
            }
            catch (Exception) { }
            Navigation.PushModalAsync(new SearchCompanions());
        }

        private void CheckTrips(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new Trips());
        }


        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Shared.Actions.refreshTrips();
            Shared.Actions.initializeMap();

        }

        private void CheckMessages(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Messages());
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            try
            {
                Shared.ModelView.UIBinding.Default.CurrentCenter = new Shared.Model.Location(Map.VisibleRegion.Center.Latitude, Map.VisibleRegion.Center.Longitude);
            }
            catch (Exception) { }
        }
    }
}
