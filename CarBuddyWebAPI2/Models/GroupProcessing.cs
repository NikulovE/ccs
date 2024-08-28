using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class GroupProcessing
    {
        public List<UserOrganization> UserGroups = new List<UserOrganization>();

        public bool LoadGroups()
        {
            var dbo = new AppDbDataContext();
            try
            {
                var GroupsArray = dbo.GroupMembers.Where(req => req.UID == App.UID).Select(req => req.Group.Name+ ";" + req.isVisibile + ";" + req.Group.GroupID).ToList();

                foreach (var team in GroupsArray)
                {
                    var temp = new UserOrganization();
                    temp.Name = team.Split(';')[0];
                    temp.IsVisible = Boolean.Parse(team.Split(';')[1]);
                    temp.TeamID = int.Parse(team.Split(';')[2]);
                    temp.IsOrganization = false;
                    UserGroups.Add(temp);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }
        }

        public static bool ChangeVisibility(int GroupID, bool isVisible)
        {
            var dbo = new AppDbDataContext();
            try
            {

                dbo.GroupMembers.Single(req => req.UID == App.UID && req.GroupID == GroupID).isVisibile = isVisible;

                try
                {
                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }
        }
    }
}