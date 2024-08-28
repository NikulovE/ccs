using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class GroupProcessing
    {
        public List<UserGroup> UserGroups = new List<UserGroup>();

        public bool LoadGroups()
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                var GroupsArray = dbo.GroupMembers.Where(req => req.UID == App.UID).Select(req => req.Group.Name+ ";" + req.isVisibile + ";" + req.Group.GroupID).ToList();

                foreach (var team in GroupsArray)
                {
                    var temp = new UserGroup();
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
            var dbo = new CarBuddyDataContext();
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