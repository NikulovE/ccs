using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class ComplaintsProcessing
    {
        public bool isAlreadyComplained(int UniqId, int SysCode) {
            try
            {
                var dbo = new AppDbDataContext();
                if (dbo.Complaints.Any(req => req.FromUID == App.UID && req.SysCode == SysCode && req.UniqID == UniqId)) return true;
                else return  false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool WrongOffice(int OfficeID)
        {
            var dbo = new AppDbDataContext();
            try
            {
                var NewComplaint = new Complaint();
                NewComplaint.SysCode = 1;
                NewComplaint.UniqID = OfficeID;
                NewComplaint.FromUID = App.UID;
                dbo.Complaints.InsertOnSubmit(NewComplaint);
                dbo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;

            }
            finally {
                try
                {
                    var totalCompplaints = dbo.Complaints.Where(req => req.UniqID == OfficeID && req.SysCode == 1).Count();
                    var userwithoffice = dbo.Users.Where(req => req.OfficeID == OfficeID).Count();
                    int quorum = userwithoffice / 3;
                    if (totalCompplaints > quorum)
                    {
                        var WrongOffice = dbo.Offices.First(req => req.OfficeID == OfficeID);
                        dbo.Offices.DeleteOnSubmit(WrongOffice);
                        dbo.SubmitChanges();
                        var users = dbo.Users.Where(req => req.OfficeID == OfficeID);
                        foreach (var usr in users)
                        {
                            usr.OfficeID = -1;
                        }
                        dbo.SubmitChanges();
                    }
                }
                catch { }
            }
        }
    }
}