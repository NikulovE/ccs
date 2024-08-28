using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ModelView;
using Shared.Model;
using System.Collections.ObjectModel;

namespace Shared.ViewModel
{
    class Car
    {
        public static async Task<bool> UpdateCar()
        {
            View.General.inLoading();
            var query = await Model.Requests.UpdateCar(ModelView.UserCar.Default);
            View.General.outLoading();
            if (query.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static async Task<bool> LoadCar()
        {

            View.General.inLoading();
            var profile = await Model.Requests.LoadCar();
            View.General.outLoading();
            if (profile.Item1 == true)
            {

                ModelView.UserCar.Default.Brand = profile.Item2.Brand;
                ModelView.UserCar.Default.BrandID = profile.Item2.BrandID;
                ModelView.UserCar.Default.Color = profile.Item2.Color;
                ModelView.UserCar.Default.ColorID = profile.Item2.ColorID;
                ModelView.UserCar.Default.Model = profile.Item2.Model;
                ModelView.UserCar.Default.ModelID = profile.Item2.ModelID;
                ModelView.UserCar.Default.GovNumber = profile.Item2.GovNumber;
                ModelView.UserCar.Default.Places = profile.Item2.Places;
                //ModelView.UserCar.Default = profile.Item2;

                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<List<String>> GetColors()
        {
            View.General.inLoading();
            var query = await Shared.Model.Requests.GetCarColors();
            View.General.outLoading();
            if (query.Item1 == true)
            {
                return query.Item2;
            }
            else
            {
                return new List<string>();
            }
        }

        public static async Task<List<String>> GetBrands()
        {
            View.General.inLoading();
            var query = await Shared.Model.Requests.GetCarBrands();
            View.General.outLoading();
            if (query.Item1 == true)
            {
                return query.Item2;
            }
            else
            {
                return new List<string>();
            }
        }

        public static async Task<List<String>> GetModel(int BrandID)
        {
            View.General.inLoading();
            var query = await Model.Requests.GetCarModels(BrandID);
            View.General.outLoading();
            if (query.Item1 == true)
            {
                return query.Item2;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
