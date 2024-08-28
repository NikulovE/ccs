using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;
using System.Web;

namespace WebAPI2.Controllers.Car
{
    public class CarController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<String>> GetCarBrands()
        {
            return new Tuple<bool, List<string>>(true, CarProcessing.GetCurrentAvailableCarBrands());
        }

        [HttpGet]
        public Tuple<bool, List<string>> GetCarModels(int CarBrandID)
        {
            return new Tuple<bool, List<string>>(true, CarProcessing.GetCurrentAvailableCaModels(CarBrandID));
        }

        [HttpGet]
        public Tuple<bool, UserCar> LoadUserCar(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CarProcessing();
                if (car.LoadUserCar()) return new Tuple<bool, UserCar>(true, car.UserCar);
                else return new Tuple<bool, UserCar>(false, new UserCar());

            }
            else
            {
                return new Tuple<bool, UserCar>(false, new UserCar { ComfortOptions = "WrongSign" });
            }
        }
        [HttpPost]
        public Tuple<bool, string> UpdateCar(int SessionID, string Sign, [FromBody]UserCar UserCar)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CarProcessing { UserCar = UserCar };
                if (car.ChangeUserCar()) return new Tuple<bool, string>(true, "x19000");
                else return new Tuple<bool, string>(true, "x19001");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }

    public class CarColorsController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<string>> GetCarColors()
        {
            return new Tuple<bool, List<string>>(true, CarProcessing.GetCurrentAvailableColors());
        }

    }
}
