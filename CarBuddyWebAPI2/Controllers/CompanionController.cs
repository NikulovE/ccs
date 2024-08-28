using WebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    public class CompanionController : ApiController
    {
        [HttpGet]
        public Tuple<bool, UserCompanion> GetUserInfo(int SessionID, string Sign, int SysId)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CompanionProcessing { FriendUID = SysId };
                if (car.isFriend())
                {
                    try
                    {
                        return new Tuple<bool, UserCompanion>(true, car.GetUserInfo());
                    }
                    catch
                    {
                        return new Tuple<bool, UserCompanion>(false, new UserCompanion { Brand="can not get user info"});
                    }
                }
                else return new Tuple<bool, UserCompanion>(false, new UserCompanion {Brand = "not friends"});

            }
            else
            {
                return new Tuple<bool, UserCompanion>(false, new UserCompanion { Brand="Wrong sign"});
            }
        }
    }
}
