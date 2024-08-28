using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class CarProcessing
    {
        public int UID;
        public String UserCarBrand;
        public String UserCarModel;
        //private int UserCarBrandID;
        //private int UserCarModelID;
        public UserCar UserCar = new UserCar();

        public static List<String> GetCurrentAvailableCarBrands() {
            var dbo = new AppDbDataContext();
            return dbo.CarBrands.Select(req => req.Brand + ':' + req.Id).ToList();
        }

        public static List<String> GetCurrentAvailableColors()
        {
            var dbo = new AppDbDataContext();
            return dbo.Colors.Select(req => req.ColorName + ':' + req.ColorID).ToList();
        }

        public static List<String> GetCurrentAvailableCaModels(int CarBrandID)
        {
            var dbo = new AppDbDataContext();
            var result=dbo.CarModels.Where(req => req.CarBrandID == CarBrandID).Select(req => req.Model + ':' + req.Id).ToList();
            if(CarBrandID!=0)  result.Add("Other:0");
            return result;
        }

        public bool LoadUserCar()
        {
            try
            {
                var dbo = new AppDbDataContext();
                var userCar = dbo.Cars.First(req => req.UID==App.UID);
                UserCar.Brand = userCar.CarBrandS.Brand;
                UserCar.BrandID = userCar.CarBrandID;
                UserCar.Model = userCar.CarModelS.Model;
                UserCar.ModelID = userCar.CarModelID;
                UserCar.Places = userCar.CarCapacity.Value;
                UserCar.ComfortOptions = userCar.CarComfort;
                UserCar.GovNumber = userCar.CarGovNumber;
                UserCar.ColorID = userCar.ColorID;
                UserCar.Color = userCar.Color.ColorName;
                return true;
            }
            catch (Exception) {
                if (CreateCar(App.UID)) {
                    try
                    {
                        var dbo = new AppDbDataContext();
                        var userCar = dbo.Cars.First(req => req.UID == App.UID);
                        UserCar.Brand = userCar.CarBrandS.Brand;
                        UserCar.BrandID = userCar.CarBrandID;
                        UserCar.Model = userCar.CarModelS.Model;
                        UserCar.ModelID = userCar.CarModelID;
                        UserCar.Places = userCar.CarCapacity.Value;
                        UserCar.ComfortOptions = userCar.CarComfort;
                        UserCar.GovNumber = userCar.CarGovNumber;
                        UserCar.ColorID = userCar.ColorID;
                        UserCar.Color = userCar.Color.ColorName;
                        return true;
                    }
                    catch {
                        return false;
                    }
                }
                else return false;
            }

        }

        public static bool isAlreadHasCar() {
            var dbo = new AppDbDataContext();
            if (dbo.Cars.Any(req =>req.UID == App.UID)) return true;
            else return false;

        }

        public static bool CreateCar(int UID) {
            try
            {
                var dbo = new AppDbDataContext();
                var newcar = new Car();
                newcar.UID = UID;
                newcar.CarBrandID = 0;
                newcar.CarModelID = 0;
                newcar.CarCapacity = 3;
                newcar.CarGovNumber = "";
                newcar.CarComfort = "0000000";
                newcar.ColorID = 0;
                dbo.Cars.InsertOnSubmit(newcar);
                dbo.SubmitChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool ChangeUserCar()
        {
            try
            {
                var dbo = new AppDbDataContext();
                var userCar = dbo.Cars.First(req => req.UID == App.UID);
                userCar.CarBrandID = UserCar.BrandID;
                userCar.CarModelID = UserCar.ModelID;
                userCar.CarComfort = UserCar.ComfortOptions;
                userCar.CarGovNumber = UserCar.GovNumber;
                userCar.CarCapacity = (byte)UserCar.Places;
                userCar.ColorID = UserCar.ColorID;
                dbo.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

//public bool ChangeUserProfile(UserProfile uProfile)
//{
//    try
//    {
//        var dbo = new AppDbDataContext();
//        user = dbo.Sessions.First(req => req.ID == SessionID).User;

//        if (uProfile.FirstName != "") user.FirstName = uProfile.FirstName;
//        if (uProfile.LastName != "") user.LastName = uProfile.LastName;
//        user.version++;
//        userProfile.Version = user.version.HasValue ? user.version.Value : 3;
//        if (uProfile.Phone != "") user.Phone = uProfile.Phone;
//        user.Payment = uProfile.Payment;
//        user.OfficeID = uProfile.OfficeID;
//        user.isDriver = uProfile.IsDriver;
//        dbo.SubmitChanges();
//        return true;
//    }
//    catch (Exception)
//    {
//        return false;
//    }

//}