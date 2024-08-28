using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Groups
{
    public class GroupController : ApiController
    {


        [HttpGet]
        public Tuple<bool, List<UserOrganization>> LoadGroups(int SessionID, string Sign)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Tuple<bool, string> GenerateInviteToGroup(int SessionID, string Sign, int GroupID)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Tuple<bool, string> CreateGroup(int SessionID, string Sign, string Name)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Tuple<bool, string> JoinGroup(int SessionID, string Sign, int GroupID, string Answer)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Tuple<bool, string> LeaveGroup(int SessionID, string Sign, int GroupID, string Answer)
        {
            throw new NotImplementedException();
        }
    }
}
