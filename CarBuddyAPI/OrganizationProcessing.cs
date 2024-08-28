using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;

namespace CarBuddyAPI
{
    public class OrganizationProcessing
    {
        public List<UserGroup> UserOrganizations = new List<UserGroup>();
        public List<OfficeOnMap> UserOffices = new List<OfficeOnMap>();

        public static bool isOrgAlreadyCreated(String Domain)
        {
            var dbo = new CarBuddyDataContext();
            if (dbo.Organizations.Any(req => req.Domain == Domain))
            {
                return true;
            }
            else return false;
        }

        public static bool isOrgMailbox(String Domain)
        {
            var dbo = new CarBuddyDataContext();
            if (dbo.Organizations.Any(req => req.Domain == Domain && req.GroupCompany.GroupCompanyID!=1))
            {
                return true;
            }
            else return false;
        }

        //public static bool isInOrgInternalNetwork(String Domain)
        //{
        //    Ping pingSender = new Ping();
        //    switch (Domain)
        //    {
        //        case "icl-services.com":
        //            if (pingSender.Send("russia.local").Status == IPStatus.Success) return true;
        //            else return false;
        //        default: return false;
        //    }
        //}

        public static bool isOfficeAlreadyCreated(int OrgID, String Name)
        {
            var dbo = new CarBuddyDataContext();
            if (dbo.Offices.Any(req => req.OrgID == OrgID && req.Name == Name))
            {
                return true;
            }
            else return false;
        }

        public static int isCanBeMemberOfOrganization(String MailDomain)
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                int OrgID = dbo.Organizations.Single(req => req.Domain == MailDomain && req.isApproved == true).OrgID;
                return OrgID;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                dbo.Connection.Close();
            }
        }

        public static bool isMemberOfOrganization(int OrgID)
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                if (dbo.OrgEmployees.Any(req => req.OrgID == OrgID && req.Session.ID== App.SessionID))
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally {
                dbo.Connection.Close();
            }
        }

        public static bool isAlreadyMember(String Domain)
        {
            //if (Domain == "") Domain = UserEmail.Split('@')[1];
            var dbo = new CarBuddyDataContext();
            if (dbo.OrgEmployees.Any(req => req.UID == App.UID && req.Organization.isApproved == true && req.Organization.Domain == Domain))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public class Registration {


            public String UserEmail;
            public String Domain;
            public String CorporateDomain;
            public String CompanyName;

            public string Password;

            public bool GeneratePreRegistrationEntry()
            {
                Password = Shared.Crypting.GeneratePassword(32);
                try
                {
                    var dbo = new CarBuddyDataContext();

                    if (dbo.OrgRegistrations.Any(req => req.Domain == CorporateDomain))
                    {
                        var alreadyRegisteredCompany = dbo.OrgRegistrations.First(req => req.Domain == CorporateDomain);

                        alreadyRegisteredCompany.Password = Password;
                        alreadyRegisteredCompany.TimeStamp = DateTime.Now;
                        dbo.SubmitChanges();
                    }
                    else
                    {
                        var newCompanyRegistration = new CarBuddyAPI.OrgRegistration();
                        newCompanyRegistration.Password = Password;
                        newCompanyRegistration.TimeStamp = DateTime.Now;
                        newCompanyRegistration.Name = CompanyName;
                        newCompanyRegistration.Domain = CorporateDomain;
                        try
                        {
                            dbo.OrgRegistrations.InsertOnSubmit(newCompanyRegistration);
                            dbo.SubmitChanges();
                            return true;
                        }
                        catch (Exception)
                        {

                            return false;
                        }
                    }
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }

        }

        public class Confirmation
        {
            public String UserEmail;
            public String Domain;
            public String TeamName;
            public String EncPassword;
            public int SessionID;
            private String Password;

            public bool CreateCompany()
            {
                try
                {
                    var dbo = new CarBuddyDataContext();

                    try
                    {
                        var UserPassword = dbo.Sessions.Single(req => req.ID == SessionID).CryptKey;
                        var AESEngine = new Shared.Crypting.AES();
                        Password = AESEngine.Decrypt(EncPassword, UserPassword);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    if (dbo.OrgRegistrations.Any(req => req.Domain == Domain))
                    {
                        var companyRegistration = dbo.OrgRegistrations.Single(req => req.Domain == Domain);
                        var passwordOnRegistration = companyRegistration.Password;

                        if (passwordOnRegistration == Password)
                        {
                            var newCompany = new CarBuddyAPI.Organization();
                            newCompany.Name = companyRegistration.Name;
                            newCompany.Domain = Domain;
                            newCompany.isApproved = true;
                            try
                            {
                                dbo.Organizations.InsertOnSubmit(newCompany);
                                dbo.SubmitChanges();
                                return true;
                            }



                            catch (Exception)
                            {
                                return false;
                            }
                            finally
                            {

                                var newEmploee = new OrgEmployee();
                                newEmploee.isVisibile = true;
                                newEmploee.OrgID = newCompany.OrgID;
                                newEmploee.UID = dbo.Sessions.Single(req => req.ID == SessionID).UID;
                                dbo.OrgEmployees.InsertOnSubmit(newEmploee);
                                

                                dbo.OrgRegistrations.DeleteOnSubmit(companyRegistration);
                                dbo.SubmitChanges();
                                dbo.Connection.Close();
                                try
                                {
                                    Shared.SendingMail.SendEmail("nikulov@live.ru", companyRegistration.Name, "New Org");
                                }
                                catch (Exception) { }
                            }
                        }
                        else return false;


                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }

        }

        public class Join
        {
            public String UserEmail;
            public String Domain="";
            public String Password;
            int OrgID;


            public bool GenerateJoinerRegistrationEntry()
            {
                Password = Shared.Crypting.GeneratePassword(32);
                var dbo = new CarBuddyDataContext();
                try
                {

                    OrgID = dbo.Organizations.Single(req => req.Domain == Domain && req.isApproved == true).OrgID;

                    if (dbo.OrgJoiners.Any(req => req.UID == App.UID && req.OrgID == OrgID))
                    {
                        var AlreadyTriedToJoin = dbo.OrgJoiners.Single(req => req.UID == App.UID && req.OrgID == OrgID);
                        AlreadyTriedToJoin.Password = Password;
                        AlreadyTriedToJoin.TimeStamp = DateTime.Now;
                    }
                    else
                    {
                        var NewJoiner = new OrgJoiner();
                        NewJoiner.Password = Password;
                        NewJoiner.UID = App.UID;
                        NewJoiner.OrgID = OrgID;
                        NewJoiner.TimeStamp = DateTime.Now;
                        dbo.OrgJoiners.InsertOnSubmit(NewJoiner);
                    }


                    dbo.SubmitChanges();
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
                finally {
                    dbo.Connection.Close();
                }
            }

            public bool CompleteJoinToOrganization(String EncPassword)
            {
                try
                {
                    var dbo = new CarBuddyDataContext();

                    try
                    {
                        var UserPassword = dbo.Sessions.First(req => req.ID == App.SessionID).CryptKey;
                        var AESEngine = new Shared.Crypting.AES();

                        Password = AESEngine.Decrypt(EncPassword, UserPassword);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    try {
                        OrgID = dbo.Organizations.First(req => req.Domain == Domain && req.isApproved == true).OrgID;
                    }
                    catch (Exception) {
                        return false;
                    }


                    if (dbo.OrgJoiners.Any(req => req.UID == App.UID))
                    {
                        var newJoiner = dbo.OrgJoiners.First(req => req.OrgID == OrgID && req.UID == App.UID);
                        var passwordOnRegistration = newJoiner.Password;

                        if (passwordOnRegistration == Password)
                        {
                            var joiner = new OrgEmployee();
                            joiner.isVisibile = true;
                            joiner.OrgID = OrgID;
                            joiner.UID = App.UID;
                            try
                            {
                                dbo.OrgEmployees.InsertOnSubmit(joiner);
                                dbo.SubmitChanges();
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                            finally
                            {
                                dbo.OrgJoiners.DeleteOnSubmit(newJoiner);
                                dbo.SubmitChanges();
                                dbo.Connection.Close();
                            }
                        }
                        else return false;


                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static void AddUserToExistingOrganization(String Domain) {
                try
                {
                    var dbo = new CarBuddyDataContext();
                    var OrgID = dbo.Organizations.First(req => req.Domain == Domain && req.isApproved == true).OrgID;
                    if(!dbo.OrgEmployees.Any(req => req.UID == App.UID)){
                        var joiner = new OrgEmployee();
                        joiner.isVisibile = true;
                        joiner.OrgID = OrgID;
                        joiner.UID = App.UID;
                        dbo.OrgEmployees.InsertOnSubmit(joiner);
                        dbo.SubmitChanges();
                    }
                }
                catch (Exception) { }
            }

        }

        public class Leave {
            public static bool LeaveOrganization(int OrgID) {
                try {
                    var dbo = new CarBuddyDataContext();
                    try
                    {
                        var Emploee = dbo.OrgEmployees.First(req => req.OrgID == OrgID && req.Session.ID == App.SessionID);
                        dbo.OrgEmployees.DeleteOnSubmit(Emploee);
                        dbo.SubmitChanges();
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
                catch (Exception) {
                    return false;
                }
            }
        }

        public bool LoadOrganizations() {
            var dbo = new CarBuddyDataContext();
            try
            {

                var CompaniesArray = dbo.OrgEmployees.Where(req => req.UID == App.UID && req.Organization.isApproved == true).Select(req => req.Organization.Name + ";" + req.isVisibile + ";" + req.Organization.OrgID).ToList();

                foreach (var team in CompaniesArray)
                {
                    var temp = new UserGroup();
                    temp.Name = team.Split(';')[0];
                    temp.IsVisible = Boolean.Parse(team.Split(';')[1]);
                    temp.TeamID = int.Parse(team.Split(';')[2]);
                    temp.IsOrganization = true;
                    UserOrganizations.Add(temp);
                }

                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally {
                dbo.Connection.Close();
            }
        }

        public bool LoadOffices(double longtitude, double latitude)
        {
            var dbo = new CarBuddyDataContext();

            if ((int)longtitude ==4 && (int)latitude == 5) {
                try
                {
                    var UserPrevLocation = dbo.CurrentPositions.First(req => req.UID == App.UID);
                    longtitude = UserPrevLocation.longitude.Value;
                    latitude = UserPrevLocation.latitude.Value;
                }
                catch (Exception) { }
            }


            try
            {
                var leftLat = latitude - 0.5;
                var rightLat = latitude + 0.5;
                var topLong = longtitude + 0.5;
                var bottomLong = longtitude - 0.5;

                var Offices = dbo.Offices.Where(req => req.Organization.isApproved == true && (req.GroupCompanyByGCID.OrgEmployee.UID == App.UID || req.OrgEmployee.UID == App.UID) && req.latitude < rightLat && req.latitude > leftLat && req.longitude > bottomLong && req.longitude < topLong).Distinct();
                foreach (var org in Offices)
                {
                    try
                    {
                        var office = new OfficeOnMap();
                        office.Name = org.Name;
                        office.Longtitude = org.longitude.Value;
                        office.Latitude = org.latitude.Value;
                        office.OrganizationID = org.OrgID;
                        office.ID = org.OfficeID;
                        UserOffices.Add(office);
                    }
                    catch (Exception) { }
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

        public static bool ChangeVisibility(int OrgID, bool isVisible) {
            var dbo = new CarBuddyDataContext();
            try
            {
                dbo.OrgEmployees.Single(req => req.UID == App.UID && req.OrgID == OrgID).isVisibile = isVisible;

                try
                {
                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception) {
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

        public static bool CreateOffice(int OrgID, String Name, double lon, double lat)
        {
            if (Name == "") return false;
            var dbo = new CarBuddyDataContext();
            try
            {
                if (dbo.Offices.Any(req => req.OrgID == OrgID && req.Name == Name)) return false;
                else {
                    var newoffice = new Office();
                    newoffice.OrgID = OrgID;
                    newoffice.longitude = lon;
                    newoffice.latitude = lat;
                    newoffice.Name = Name;
                    try {
                        newoffice.GroupCompanyID = dbo.GroupCompanies.First(req => req.OrgID == OrgID).GroupCompanyID;
                    }
                    catch { }
                    dbo.Offices.InsertOnSubmit(newoffice);
                }
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

//public bool GeneratePreRegistrationEntry()
//{
//    Password = Shared.Crypting.GeneratePassword(32);
//    UID = Shared.Crypting.GeneratePassword(9, true);
//    try
//    {
//        var dbo = new CarBuddyDataContext();

//        if (dbo.Registrations.Any(req => req.Mail == UserEmail))
//        {
//            var alreadyRegisteredUser = dbo.Registrations.Single(req => req.Mail == UserEmail);

//            alreadyRegisteredUser.Password = Password;
//            alreadyRegisteredUser.Errors++;
//            alreadyRegisteredUser.TimeStamp = DateTime.Now;

//            var RSAEng = new Shared.Crypting.RSA();
//            KeyPair = RSAEng.CreateKeyPair();

//            alreadyRegisteredUser.RSAprivate = KeyPair.Item1;
//            alreadyRegisteredUser.RSApublic = KeyPair.Item2;

//            dbo.SubmitChanges();
//            if (alreadyRegisteredUser.Errors > 15) return false;
//        }
//        else
//        {
//            var newUserRegistration = new CarBuddyAPI.Registration();
//            newUserRegistration.Password = Password;
//            newUserRegistration.Errors = 0;
//            newUserRegistration.TimeStamp = DateTime.Now;
//            newUserRegistration.UID = int.Parse(UID);
//            newUserRegistration.Mail = UserEmail;


//            var RSAEng = new Crypting.RSA();
//            KeyPair = RSAEng.CreateKeyPair();

//            newUserRegistration.RSAprivate = KeyPair.Item1;
//            newUserRegistration.RSApublic = KeyPair.Item2;

//            try
//            {
//                dbo.Registrations.InsertOnSubmit(newUserRegistration);
//                dbo.SubmitChanges();
//            }
//            catch
//            {

//                UID = Shared.Crypting.GeneratePassword(9, true);
//                newUserRegistration.UID = int.Parse(UID);
//                dbo.Registrations.InsertOnSubmit(newUserRegistration);
//                dbo.SubmitChanges();
//            }

//        }
//        return true;
//    }
//    catch (Exception)
//    {
//        return false;
//    }
//}

//public String GeneratePreRegistrationAESKey()
//{
//    var aesEngine = new Crypting.AES();
//    var AESKey = aesEngine.Encrypt(UID + ',' + KeyPair.Item2, Password);
//    return AESKey;

//}

//public bool RefreshPassword()
//{
//    try
//    {
//        var dbo = new CarBuddyDataContext();
//        var UserRegEntry = dbo.Registrations.Single(req => req.Mail == UserEmail);
//        Password = Shared.Crypting.GeneratePassword(32);
//        UserRegEntry.Password = Password;

//        KeyPair = new Tuple<string, string>("", UserRegEntry.RSApublic);
//        UID = UserRegEntry.UID.ToString();
//        dbo.SubmitChanges();
//        return true;
//    }
//    catch (Exception)
//    {
//        return false;
//    }

//}

//public bool SendKeyToEmail()
//{
//    if (Shared.SendingMail.SendEmail(UserEmail, "Please, use: " + Password + " to continue registration", "Your password for " + Shared.General.AppName)) return true;
//    else return false;
//}