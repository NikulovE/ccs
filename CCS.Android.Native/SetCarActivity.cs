using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections;
using System.Collections.Generic;
using Android;
using static Android.Resource;
using System.Threading.Tasks;
using System;

namespace CCS.Android.Native
{
    [Activity(Label = "My Car")]
    public class SetCarActivity : Activity
    {

        private List<KeyValuePair<string, int>> BrandsStack = new List<KeyValuePair<string, int>>();
        private List<string> BrandNames = new List<string>();
        private int BrandIndex;

        private List<KeyValuePair<string, int>> ModelsStack = new List<KeyValuePair<string, int>>();
        private List<string> ModelNames = new List<string>();
        private int ModelIndex;

        private List<KeyValuePair<string, int>> ColorsStack = new List<KeyValuePair<string, int>>();
        private List<string> ColorNames = new List<string>();
        private int ColorIndex;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Car);
            LoadCar();
            
            InitializeUI();
        }
       

        private async void LoadCar()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            await Shared.ViewModel.Car.LoadCar(progressBar, output);
        }

        private async void InitializeUI()
        {
            await LoadBrands();
            await LoadModels();
            await LoadColors();

            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var brandselector= FindViewById<Spinner>(Resource.Id.Brand);
            brandselector.SetSelection(BrandIndex);        

            var modelselector = FindViewById<Spinner>(Resource.Id.Model);
            modelselector.SetSelection(ModelIndex);

            var colorselector = FindViewById<Spinner>(Resource.Id.Color);
            colorselector.SetSelection(ColorIndex);

            var govnum = FindViewById<TextView>(Resource.Id.PlaneNum);
            govnum.Text = Shared.ModelView.UserCar.Default.GovNumber;
            govnum.AfterTextChanged += async (sender, e) =>
            {
                Shared.ModelView.UserCar.Default.GovNumber = govnum.Text;
                await Shared.ViewModel.Car.UpdateCar(progressBar, output);
            };

            brandselector.ItemSelected += async (sender, e) =>
            {
                var u = (Spinner)sender;
                if (Shared.ModelView.UserCar.Default.BrandID != BrandsStack[e.Position].Value) {                    
                    Shared.ModelView.UserCar.Default.BrandID = BrandsStack[e.Position].Value;
                    Shared.ModelView.UserCar.Default.Brand = BrandsStack[e.Position].Key;
                    Shared.ModelView.UserCar.Default.ModelID = 0;
                    Shared.ModelView.UserCar.Default.Model = "Other";                    
                    await Shared.ViewModel.Car.UpdateCar(progressBar, output);
                    await LoadModels();
                    modelselector.SetSelection(ModelIndex);
                }

            };
            modelselector.ItemSelected += async (sender, e) =>
            {
                var u = (Spinner)sender;
                if (Shared.ModelView.UserCar.Default.ModelID != ModelsStack[e.Position].Value)
                {
                    Shared.ModelView.UserCar.Default.ModelID = ModelsStack[e.Position].Value;
                    Shared.ModelView.UserCar.Default.Model = ModelsStack[e.Position].Key;
                    await Shared.ViewModel.Car.UpdateCar(progressBar, output);
                }
            };
            colorselector.ItemSelected += async (sender, e) =>
            {
                var u = (Spinner)sender;
                if (Shared.ModelView.UserCar.Default.ColorID != ColorsStack[e.Position].Value)
                {
                    Shared.ModelView.UserCar.Default.ColorID = ColorsStack[e.Position].Value;
                    Shared.ModelView.UserCar.Default.Color = ColorsStack[e.Position].Key;
                    await Shared.ViewModel.Car.UpdateCar(progressBar, output);
                }
            };


        }

        async Task<bool> LoadBrands() {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var brands = await Shared.ViewModel.Car.GetBrands(progressBar,output);
            foreach (var Brand in brands)
            {
                var CarBrand = Brand.Split(':');
                var nextbrand = new KeyValuePair<string, int>(CarBrand[0], int.Parse(CarBrand[1]));
                BrandsStack.Add(nextbrand);
            }
            
            foreach (var item in BrandsStack)
            {
                if (Shared.ModelView.UserCar.Default.Brand == item.Key)
                {
                    BrandIndex = BrandsStack.IndexOf(item);
                }

                BrandNames.Add(item.Key);
            }
            var brandselector = FindViewById<Spinner>(Resource.Id.Brand);
            var adapter = new ArrayAdapter<string>(this, Layout.SimpleSpinnerItem, BrandNames);
            brandselector.Adapter = adapter;
            return true;         

        }
        async Task<bool> LoadModels()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var models = await Shared.ViewModel.Car.GetModel(Shared.ModelView.UserCar.Default.BrandID, progressBar, output);
            ModelsStack.Clear();
            foreach (var model in models)
            {
                var CarModel = model.Split(':');
                var nextmodel = new KeyValuePair<string, int>(CarModel[0], int.Parse(CarModel[1]));
                ModelsStack.Add(nextmodel);
            }
            ModelNames.Clear();
            foreach (var item in ModelsStack)
            {
                if (Shared.ModelView.UserCar.Default.Model == item.Key)
                {
                    ModelIndex = ModelsStack.IndexOf(item);
                }

                ModelNames.Add(item.Key);
            }
            var modelselector = FindViewById<Spinner>(Resource.Id.Model);
            var adp = new ArrayAdapter<string>(this, Layout.SimpleSpinnerItem, ModelNames);
            modelselector.Adapter = adp;
            return true;

        }

        async Task<bool> LoadColors()
        {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var colors = await Shared.ViewModel.Car.GetColors(progressBar, output);
            foreach (var color in colors)
            {
                var CarColor = color.Split(':');
                var nextcolor = new KeyValuePair<string, int>(CarColor[0], int.Parse(CarColor[1]));
                ColorsStack.Add(nextcolor);
            }

            foreach (var item in ColorsStack)
            {
                if (Shared.ModelView.UserCar.Default.Color == item.Key)
                {
                    ColorIndex = ColorsStack.IndexOf(item);
                }
                ColorNames.Add(item.Key);
            }
            var colorselector = FindViewById<Spinner>(Resource.Id.Color);
            var adpt = new ArrayAdapter<string>(this, Layout.SimpleSpinnerItem, ColorNames);
            colorselector.Adapter = adpt;
            return true;

        }

    }
}