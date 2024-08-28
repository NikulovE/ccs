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
	public partial class SearchCompanions : ContentPage
	{
		public SearchCompanions ()
		{
			InitializeComponent ();
            Shared.Actions.initializeMap = initmap;
            Shared.Actions.initializeMap();
        }

        private void CheckWizard()
        {
            if (Shared.ModelView.UIBinding.Default.HomeLocation.Latitude == 0 && Shared.ModelView.UIBinding.Default.HomeLocation.Longitude == 0) { }
            else
            {
                if (Shared.ModelView.UserOrganizations.Default.OrganizationsList.Count == 0)
                {
                    App.Current.MainPage = new CCS.OrganizationJoin();
                    return;
                }
                if (Shared.ModelView.UserProfile.Default.OfficeID == -1)
                {
                    App.Current.MainPage = new CCS.SelectOffice();
                    return;
                }
            }
        }
        private void initmap()
        {
            if (Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude != 0 && Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude != 0)
            {
                SearchMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Shared.ModelView.UIBinding.Default.CurrentCenter.Latitude, Shared.ModelView.UIBinding.Default.CurrentCenter.Longitude), Distance.FromMiles(1)));
            }

        }

        private void SearchToHome(object sender, EventArgs e)
        {
            SearchMap.Pins.Clear();
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = true;
            Shared.ViewModel.Companions.FindCompanions(SearchMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }

        private void SearchToWork(object sender, EventArgs e)
        {
            SearchMap.Pins.Clear();
            Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome = false;
            Shared.ViewModel.Companions.FindCompanions(SearchMap, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome, DateTime.Today);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            CheckWizard();
        }
    }
}