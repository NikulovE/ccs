using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
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
            var dbo = new CarBuddyDataContext();
            return dbo.CarBrands.Select(req => req.Brand + ':' + req.Id).ToList();
        }

        public static List<String> GetCurrentAvailableCaModels(int CarBrandID)
        {
            var dbo = new CarBuddyDataContext();
            var result=dbo.CarModels.Where(req => req.CarBrandID == CarBrandID).Select(req => req.Model + ':' + req.Id).ToList();
            if(CarBrandID!=0)  result.Add("Other:0");
            return result;
        }

        public bool LoadUserCar()
        {
            try
            {
                var dbo = new CarBuddyDataContext();
                var userCar = dbo.Cars.First(req => req.UID==App.UID);
                UserCar.Brand = userCar.CarBrandS.Brand;
                UserCar.BrandID = userCar.CarBrandID;
                UserCar.Model = userCar.CarModelS.Model;
                UserCar.ModelID = userCar.CarModelID;
                UserCar.Places = userCar.CarCapacity.Value;
                UserCar.ComfortOptions = userCar.CarComfort;
                UserCar.GovNumber = userCar.CarGovNumber;
                return true;
            }
            catch (Exception) {
                return false;
            }

        }

        public static bool isAlreadHasCar() {
            var dbo = new CarBuddyDataContext();
            if (dbo.Cars.Any(req =>req.Session.ID == App.SessionID)) return true;
            else return false;

        }

        public static bool CreateCar() {
            try
            {
                var dbo = new CarBuddyDataContext();
                var newcar = new Car();
                newcar.UID = App.UID;
                newcar.CarBrandID = 0;
                newcar.CarModelID = 0;
                newcar.CarCapacity = 3;
                newcar.CarGovNumber = "";
                newcar.CarComfort = "0000000";
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
                var dbo = new CarBuddyDataContext();
                var userCar = dbo.Cars.First(req => req.UID == App.UID);
                userCar.CarBrandID = UserCar.BrandID;
                userCar.CarModelID = UserCar.ModelID;
                userCar.CarComfort = UserCar.ComfortOptions;
                userCar.CarGovNumber = UserCar.GovNumber;
                userCar.CarCapacity = (byte)UserCar.Places;
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
//        var dbo = new CarBuddyDataContext();
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