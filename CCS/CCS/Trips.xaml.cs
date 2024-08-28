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
	public partial class Trips : ContentPage
	{


		public Trips ()
		{
			InitializeComponent ();
            Shared.Actions.refreshTrips = loadtripsAsync;
            Shared.Actions.refreshTrips();
            Shared.Actions.showTripOffers = RefreshTrips;
            Shared.Actions.showTripOffers();
            

        }

        async void loadtripsAsync()
        {
            await Shared.ViewModel.Trips.LoadTrips();
        }

        private void RefreshTrips() {
            TripsList.ItemsSource = Shared.ModelView.UIBinding.Default.TripOffers;
        }

        private async void AcceptTrip(object sender, EventArgs e)
        {
            var but = sender as Button;
            var OfferID = int.Parse(but.ClassId);
            await Shared.ViewModel.Trips.AcceptTrip(OfferID);
        }

        private async void RejectTrip(object sender, EventArgs e)
        {
            var but = sender as Button;
            var OfferID = int.Parse(but.ClassId);
            await Shared.ViewModel.Trips.RejectTrip(OfferID);
        }
    }


}