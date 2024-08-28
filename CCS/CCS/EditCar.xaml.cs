using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
namespace CCS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCar : ContentPage
	{
        List<String> Brands = new List<string>();
        List<String> Models = new List<string>();
        public EditCar ()
		{
			InitializeComponent ();
            CarGrid.BindingContext = Shared.ModelView.UserCar.Default;
            LoadCar();

        }
        async void LoadCar() {
            await Shared.ViewModel.Car.LoadCar();
        }

        private async void FinalSelectCarBrand(object sender, EventArgs e)
        {

            try
            {
                int SelectedBrandId = int.Parse(Brands.First(req => req.Split(':')[0] == CarBrandSelector.SelectedItem.ToString()).ToString().Split(':')[1]);
                if (Shared.ModelView.UserCar.Default.Brand != CarBrandSelector.SelectedItem.ToString())
                {
                    Shared.ModelView.UserCar.Default.ModelID = 0;
                    Shared.ModelView.UserCar.Default.Model = "Other";
                    Shared.ModelView.UserCar.Default.BrandID = SelectedBrandId;
                    Shared.ModelView.UserCar.Default.Brand = CarBrandSelector.SelectedItem.ToString();
                    Models = await Shared.View.Car.ShowCarModels(CarModelSelector, SelectedBrandId);
                    await Shared.ViewModel.Car.UpdateCar();
                }
            }
            catch (Exception) { }
        }

        private async void UpdateCarProfile(object sender, EventArgs e)
        {
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void SelectCarModel(object sender, EventArgs e)
        {
            try
            {
                int SelectModelID = int.Parse(Models.First(req => req.Split(':')[0] == CarModelSelector.SelectedItem.ToString()).ToString().Split(':')[1]);
                Shared.ModelView.UserCar.Default.ModelID = SelectModelID;
                Shared.ModelView.UserCar.Default.Model = CarModelSelector.SelectedItem.ToString();
                await Shared.ViewModel.Car.UpdateCar();
            }
            catch (Exception) { }
        }

        private async void DecreasePassangerPlaces(object sender, EventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places--;
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void ExpandPassangerPlaces(object sender, EventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places++;
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            Brands=await Shared.View.Car.ShowCarBrands(CarBrandSelector);
        }
    }
}