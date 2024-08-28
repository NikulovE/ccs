using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CCS.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditCar : Page
    {
        bool carloaded = false;
        public EditCar()
        {
            this.InitializeComponent();
            
            LoadCar();

            
        }

        private void LoadCar()
        {


            BrandX.Content = Shared.ModelView.UserCar.Default.Brand;

            Shared.View.Car.ShowCarModels(CarModelSelector, Shared.ModelView.UserCar.Default.BrandID);


        }

        private void StartSelectCarBrand(object sender, object e)
        {
            Shared.View.Car.ShowCarBrands(CarBrandSelector);
            CarBrandSelector.SelectionChanged += ResetCarModel;
        }

        private void FinalSelectCarBrand(object sender, object e)
        {
            CarBrandSelector.SelectionChanged -= ResetCarModel;
        }

        private async void ResetCarModel(object sender, SelectionChangedEventArgs e)
        {
            var selectedBrand = ((ComboBoxItem)CarBrandSelector.SelectedItem).Content.ToString();
            if (Shared.ModelView.UserCar.Default.Brand != selectedBrand)
            {
                CarModelSelector.SelectionChanged -= SelectCarModel;
                int BrandID = (int.Parse(((ComboBoxItem)CarBrandSelector.SelectedItem).Tag.ToString()));
                Shared.ModelView.UserCar.Default.ModelID = 0;
                Shared.ModelView.UserCar.Default.BrandID = BrandID;
                Shared.ModelView.UserCar.Default.Brand = ((ComboBoxItem)CarBrandSelector.SelectedItem).Content.ToString();
                Shared.View.Car.ShowCarModels(CarModelSelector, BrandID);
                await Shared.ViewModel.Car.UpdateCar();
                CarModelSelector.SelectionChanged += SelectCarModel;
            }
        }

        private async void SelectCarModel(object sender, SelectionChangedEventArgs e)
        {
            if (carloaded)
            {
                try
                {
                    var Model = ((ComboBox)sender).SelectedItem as ComboBoxItem;
                    int ModelID = (int.Parse((Model).Tag.ToString()));
                    Shared.ModelView.UserCar.Default.ModelID = ModelID;
                    await Shared.ViewModel.Car.UpdateCar();
                }
                catch { }

            }
        }

        private async void ExpandPassangerPlaces(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places++;
            await Shared.ViewModel.Car.UpdateCar();
        }

        private async void DecreasePassangerPlaces(object sender, RoutedEventArgs e)
        {
            Shared.ModelView.UserCar.Default.Places--;
            await Shared.ViewModel.Car.UpdateCar();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            CarInfo.DataContext = Shared.ModelView.UserCar.Default;

            carloaded = true;
            GovNumber.TextChanged += NumberChanged;
           




        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
           if(numberChanged) await Shared.ViewModel.Car.UpdateCar();
        }

        bool numberChanged = false;
        private void NumberChanged(object sender, TextChangedEventArgs e)
        {
           numberChanged = true;
        }


    }
}
