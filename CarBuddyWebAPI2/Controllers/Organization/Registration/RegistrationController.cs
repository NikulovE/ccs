using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Organization
{
    public class OrgRegistrationController : ApiController
    {
        [HttpPut]
        public Tuple<bool, string> StartOrganizationRegistration(int SessionID, string Sign, [FromBody]string email, string name)
        {

            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var newOrg = new OrganizationProcessing.Registration();
                newOrg.UserEmail = email.ToLower();
                if (General.isEmailFormatValid(email))
                {
                    if (!OrganizationProcessing.isOrgAlreadyCreated(email.Split('@')[1]))
                    {
                        var organization = new OrganizationProcessing.Registration();
                        organization.UserEmail = email;
                        organization.CompanyName = name;
                        organization.CorporateDomain = email.Split('@')[1];
                        if (organization.GeneratePreRegistrationEntry())
                        {
                            if (General.SendKeyToEmail(email, organization.Password)) return new Tuple<bool, String>(true, "x14001"); //in approval
                            else return new Tuple<bool, String>(false, "x10001"); //can`t send
                        }
                        else return new Tuple<bool, String>(false, "x14002"); //can`t generate preinfo
                    }
                    else return new Tuple<bool, String>(false, "x14000"); //comp is already created

                }
                else return new Tuple<bool, String>(false, "x10005"); //email not valid
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
        [HttpPost]
        public Tuple<bool, string> ConfirmOrganizationRegistration(int SessionID, string Sign,  string domain, [FromBody]string encyptedConfirmationKey)
        {

            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var newTeam = new OrganizationProcessing.Confirmation();
                newTeam.Domain = domain.ToLower();
                if (!OrganizationProcessing.isOrgAlreadyCreated(domain))
                {
                    var company = new OrganizationProcessing.Confirmation();
                    company.Domain = domain;
                    company.SessionID = SessionID;
                    company.EncPassword = encyptedConfirmationKey;
                    if (company.CreateCompany())
                    {
                        return new Tuple<bool, String>(true, "x14003"); //registration in approval
                    }
                    else return new Tuple<bool, String>(false, "x14002"); //can`t generate preinfo
                }
                else return new Tuple<bool, String>(false, "x14000"); //comp is already created
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

    }
}
