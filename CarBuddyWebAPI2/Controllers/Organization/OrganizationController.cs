using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Organization
{
    public class OrganizationController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<UserOrganization>> LoadOrganizations(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new List<UserOrganization>();

                var organizations = new OrganizationProcessing();
                var groups = new GroupProcessing();

                organizations.UserOrganizations = result;
                groups.UserGroups = result;
                if (organizations.LoadOrganizations() && groups.LoadGroups()) return new Tuple<bool, List<UserOrganization>>(true, result);
                else
                {
                    return new Tuple<bool, List<UserOrganization>>(false, new List<UserOrganization>());
                }
            }
            else
            {
                return new Tuple<bool, List<UserOrganization>>(false, new List<UserOrganization>()); //wrong sign
            }
        }


        [HttpPost]
        public Tuple<bool, string> ChangeVisibility(int SessionID, string Sign, [FromBody]Tuple<bool,bool,int> Organization )
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (Organization.Item2)
                {

                    if (OrganizationProcessing.ChangeVisibility(Organization.Item3, Organization.Item1))
                    {
                        return new Tuple<bool, string>(true, "");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "x14004");
                    }
                }
                else
                {
                    if (GroupProcessing.ChangeVisibility(Organization.Item3, Organization.Item1))
                    {
                        return new Tuple<bool, string>(true, "");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "x14004");
                    }
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
