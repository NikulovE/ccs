using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Organization.Office
{
    public class OfficeController : ApiController
    {
        [HttpPost]
        public Tuple<bool, List<OfficeOnMap>> LoadOffices(int SessionID, string Sign, [FromBody]Tuple<double,double> gps)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new OrganizationProcessing();
                if (result.LoadOffices(gps.Item1, gps.Item2)) return new Tuple<bool, List<OfficeOnMap>>(true, result.UserOffices);
                else
                {
                    return new Tuple<bool, List<OfficeOnMap>>(false, new List<OfficeOnMap>());
                }
            }
            else
            {
                return new Tuple<bool, List<OfficeOnMap>>(false, new List<OfficeOnMap>()); //wrong sign
            }
        }

        [HttpGet]
        public Tuple<bool, OfficeOnMap> LoadUserOffice(int SessionID, string Sign) {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new OrganizationProcessing();
                if (result.LoadUserOffice()) return new Tuple<bool, OfficeOnMap>(true, result.UserOffice);
                else
                {
                    return new Tuple<bool, OfficeOnMap>(false, new OfficeOnMap());
                }
            }
            else
            {
                return new Tuple<bool, OfficeOnMap>(false, new OfficeOnMap()); //wrong sign
            }
        }

        [HttpPut]
        public Tuple<bool, string> CreateOffice(int SessionID, string Sign, [FromBody]Tuple<int,string,Double,double> OfficeParam)// int TeamID, string Name, double longtitude, double latitude)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (OrganizationProcessing.isMemberOfOrganization(OfficeParam.Item1))
                {
                    if (!OrganizationProcessing.isOfficeAlreadyCreated(OfficeParam.Item1, OfficeParam.Item2))
                    {
                        if (OrganizationProcessing.CreateOffice(OfficeParam.Item1, OfficeParam.Item2, OfficeParam.Item3, OfficeParam.Item4))
                        {
                            return new Tuple<bool, String>(true, "x14010"); //created
                        }
                        else
                        {
                            return new Tuple<bool, String>(false, "x50000"); //something wrong
                        }
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x14009"); //office is already created
                    }
                }
                else
                {
                    return new Tuple<bool, String>(false, "x14011"); //you are not a member
                }



            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }


    }

    public class OfficeComplaintController : ApiController
    {

        [HttpPost]
        public Tuple<bool, string> NewComplaint(int SessionID, string Sign, int SysCode, [FromBody]int UniqID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewComplaint = new ComplaintsProcessing();
                if (!NewComplaint.isAlreadyComplained(UniqID, SysCode))
                {
                    if (SysCode == 1)
                    {
                        if (NewComplaint.WrongOffice(UniqID)) return new Tuple<bool, String>(true, "x18001"); //added
                        else return new Tuple<bool, String>(false, "x12001"); //wrong sign

                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x18003"); //wrong syscode
                    }
                }
                else
                {
                    return new Tuple<bool, String>(false, "x18002"); //wrong sign
                }


            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
