using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Organization.Membershipping
{
    public class OrganizationMemberController : ApiController
    {
        [HttpPut]
        public Tuple<bool, string> StartJoinToOrganization(int SessionID, string Sign, [FromBody]string email)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (!General.isEmailFormatValid(email))
                {
                    return new Tuple<bool, String>(false, "x10005"); //email not valid
                }
                else
                {
                    var Domain = email.Split('@')[1];
                    var newJoiner = new OrganizationProcessing.Join { UserEmail = email, Domain = Domain };

                    if (OrganizationProcessing.isOrgAlreadyCreated(Domain))
                    {
                        if (OrganizationProcessing.isAlreadyMember(Domain))
                        {
                            return new Tuple<bool, String>(false, "x14006"); //already member
                        }
                        else
                        {
                            if (newJoiner.GenerateJoinerRegistrationEntry())
                            {
                                if (General.SendKeyToEmail(email, newJoiner.Password))
                                {
                                    return new Tuple<bool, String>(true, "x14001"); //email sent success
                                }
                                else
                                {
                                    return new Tuple<bool, String>(false, "x10001"); //email sent not success
                                }
                            }
                            else
                            {
                                if (App.ErrorCode == "0") return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                                else return new Tuple<bool, String>(false, App.ErrorCode);
                            }
                        }
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x14005"); //comp is not already created
                    }

                }

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        [HttpPost]
        public Tuple<bool, String> ConfirmJoinOrganization(int SessionID, string Sign, string domain, [FromBody]string encyptedConfirmationKey)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var newJoiner = new OrganizationProcessing.Join { Domain = domain };
                if (OrganizationProcessing.isOrgAlreadyCreated(domain))
                {
                    if (OrganizationProcessing.isAlreadyMember(domain))
                    {
                        return new Tuple<bool, String>(false, "x14006"); //already member
                    }
                    else
                    {
                        if (newJoiner.CompleteJoinToOrganization(encyptedConfirmationKey))
                        {
                            return new Tuple<bool, String>(true, "x14008"); //email sent success
                        }
                        else
                        {
                            return new Tuple<bool, String>(false, "x50004"); //can`t generate joiner flow
                        }
                    }
                }
                else
                {
                    return new Tuple<bool, String>(false, "x14005"); //comp is not already created
                }

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }


    }

    public class OrganizationEController : ApiController
    {
        [HttpPost]
        public Tuple<bool, string> LeaveOrganization(int SessionID, string Sign, [FromBody]int OrgID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (OrganizationProcessing.isMemberOfOrganization(OrgID))
                {
                    if (OrganizationProcessing.Leave.LeaveOrganization(OrgID)) return new Tuple<bool, String>(true, "x20001");
                    else
                    {
                        return new Tuple<bool, String>(false, "x20002");
                    }
                }
                else
                {
                    return new Tuple<bool, String>(false, "x20003");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
