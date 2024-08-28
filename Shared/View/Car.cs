using System.Collections.Generic;


#if NETFX_CORE
using Windows.UI.Xaml.Controls;
#else
#if WPF
using System.Windows.Controls;
#endif
#endif
#if XAMARIN
using Xamarin.Forms;
using System.Threading.Tasks;
#endif


namespace Shared.View
{
    class Car
    {
#if XAMARIN
        public static async Task<List<string>> ShowCarBrands(Picker Box) {
            Box.Items.Clear();
            var Brands = await ViewModel.Car.GetBrands();
            foreach (var Brand in Brands)
            {
                var CarBrand = Brand.Split(':');
                Box.Items.Add(CarBrand[0]);
            }
            return Brands;
        }

        public static async Task<List<string>> ShowCarModels(Picker Box, int BrandID)
        {
            Box.Items.Clear();
            var Models = await ViewModel.Car.GetModel(BrandID);
            foreach (var Model in Models)
            {
                var CarModel = Model.Split(':');
                Box.Items.Add(CarModel[0]);
            }
            return Models;

        }
#else
        public static async void ShowCarBrands(ComboBox Box)
        {
            if (Box.Items.Count == 1)
            {
                Box.Items.Clear();
                foreach (var Brand in await ViewModel.Car.GetBrands())
                {
                    var CarBrand = Brand.Split(':');
                    var NextBrand = new ComboBoxItem();
                    if (ModelView.UserCar.Default.Brand == CarBrand[0]) NextBrand.IsSelected = true;
                    NextBrand.Content = CarBrand[0];
                    NextBrand.Tag = CarBrand[1];
                    Box.Items.Add(NextBrand);
                }
            }

        }

        public static async void ShowCarColors(ComboBox Box, int ColorID=500)
        {
            //if (Box.Items.Count == 1)
            //{
                Box.Items.Clear();
                foreach (var Brand in await ViewModel.Car.GetColors())
                {
                    var CarColor = Brand.Split(':');
                    var NextBrand = new ComboBoxItem();
                    if (ModelView.UserCar.Default.Color == CarColor[0]) NextBrand.IsSelected = true;
                    NextBrand.Content = CarColor[0];
                    NextBrand.Tag = CarColor[1];
                    Box.Items.Add(NextBrand);
                }
            //}

        }

        public static async void ShowCarModels(ComboBox Box, int BrandID)
        {
            var CarModels = new List<ComboBoxItem>();
            int selectedindex = 0;
            foreach (var Model in await ViewModel.Car.GetModel(BrandID))
            {
                var CarModel = Model.Split(':');
                var NextModel = new ComboBoxItem();                
                NextModel.Content = CarModel[0];
                NextModel.Tag = CarModel[1];
                CarModels.Add(NextModel);
                if (ModelView.UserCar.Default.ModelID == int.Parse(CarModel[1]))
                {
                    selectedindex = CarModels.IndexOf(NextModel);
                }
            }
            try
            {
                Box.ItemsSource = CarModels;
                Box.SelectedIndex = selectedindex;
            }
            catch { }
        }
#endif

    }
}
