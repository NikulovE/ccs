using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;



namespace CarBuddyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CarBuddyService : ICarBuddyService
    {
        public Tuple<bool,String>  StartRegistration(string email)
        {
            var newuser = new ProfileProcessing.Registration();
            newuser.UserEmail = email.ToLower();

            //return UserRegistration.CheckEmailAlreadyPresent(useremail);
            if (General.isEmailFormatValid(email))
            {
                if(OrganizationProcessing.isOrgMailbox(email.Split('@')[1])) return new Tuple<bool, String>(false, "x10007"); //email already tried to register
                if (newuser.isEmailAlreadyPresent())
                {
                    if (newuser.GeneratePreRegistrationEntry())
                    {
                        if (General.SendKeyToEmail(email,newuser.Password)) return new Tuple<bool, String>(true, newuser.GeneratePreRegistrationAESKey());
                        else return new Tuple<bool, String>(false, "x10001"); //can`t send
                    }
                    else return new Tuple<bool, String>(false, "x10002"); //can`t generate preinfo
                }
                else return new Tuple<bool, String>(false, "x10003"); //email already tried to register

            }
            else return new Tuple<bool, String>(false, "x10005"); //email not valid

        }
        public Tuple<bool, String> ConfirmRegistration(int UID, String Answer)
        {
            var userConfirmation = new ProfileProcessing.Confirmation { Answer = Answer, UID=UID };

            if (userConfirmation.FindUserPassword())
            {
                if (userConfirmation.isAlreadyCreated())
                {
                    if (userConfirmation.CheckAnswer())
                    {
                        var SessionKey = userConfirmation.ConfirmRegistration(true);
                        switch (SessionKey) {
                            case "error0":  return new Tuple<bool, String>(false, "x11006");
                            case "error1":  return new Tuple<bool, String>(false, "x11007");
                            case "error2": return new Tuple<bool, String>(false, "x11008");
                            default: return new Tuple<bool, String>(true, SessionKey);

                        }
                        //if (SessionKey != "error0" && ) return new Tuple<bool, String>(true, SessionKey);
                        //else return new Tuple<bool, String>(false, "x11002");
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x11003");
                    }
                }
                else
                {
                    if (userConfirmation.CheckAnswer())
                    {
                        var SessionKey = userConfirmation.ConfirmRegistration();
                        if (SessionKey != "error") return new Tuple<bool, String>(true, SessionKey);
                        else return new Tuple<bool, String>(false, "x11002");
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x11003");
                    }
                }
            }
            else return new Tuple<bool, String>(false, "x11004");
        }
        public Tuple<bool, String> RestoreRegistration(string email) {
            var alreadyRegisteredUser = new ProfileProcessing.Registration { UserEmail = email };
            if (alreadyRegisteredUser.RefreshPassword())
            {
                General.SendKeyToEmail(email, alreadyRegisteredUser.Password);
                return new Tuple<bool, String>(true, alreadyRegisteredUser.GeneratePreRegistrationAESKey());
            }
            else return new Tuple<bool, String>(false, "x10006"); //can`t generate preinfo

        }

        public Tuple<bool, String> RestoreSessionKey(int UID, String ClientRequest)
        {
            var userConfirmation = new ProfileProcessing.Confirmation { Answer = ClientRequest, UID = UID };
            if (userConfirmation.CheckPreSign())
            {
                var newSessionPair = ProfileProcessing.SessionKey.CreateSessionKey(UID);
                if (newSessionPair.Item2 != "error")
                {
                    return new Tuple<bool, String>(true, ProfileProcessing.SessionKey.EncryptedSession(userConfirmation.ClientRSA, newSessionPair.Item1, newSessionPair.Item2));
                }
                else
                {
                    return new Tuple<bool, String>(false, "x23004");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x23005");
            }
        }
        public Tuple<bool, UserProfile> LoadProfile(int SessionID, String Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign)) { 
                var uProfile = new UserProfile();
                var profile = new ProfileProcessing.Profile { SessionID = SessionID, userProfile = uProfile };
                if (profile.LoadProfile())
                {
                    return new Tuple<bool, UserProfile>(true, profile.userProfile);
                }
                else return new Tuple<bool, UserProfile>(false, new UserProfile());
            }
            else
            {
                return new Tuple<bool, UserProfile>(false, new UserProfile { FirstName="WrongSign"});
            }
        }
        public Tuple<bool, string> Logon(int SessionID, String EncryptedRequest)
        {
            var userSession = new ProfileProcessing.SessionKey { SessionID = SessionID };
            if (userSession.CheckUserSessionKey(EncryptedRequest))
            {
                userSession.UpdateSessionKey();
                var NewSessionKey = userSession.EncryptedSessionKey();
                return new Tuple<bool, String>(true, NewSessionKey);
            }
            else
            {
                return new Tuple<bool, String>(false, "x11002");
            }
        }
        public Tuple<bool, String> UpdateProfile(int SessionID, String Sign, UserProfile UserProfile) {
            if (ProfileProcessing.CheckSign(SessionID, Sign)){
                if (UserProfile.FirstName.Length > 70 || UserProfile.LastName.Length > 70 || UserProfile.Phone.Length > 12) {
                    return new Tuple<bool, String>(false, "x12004");
                }
                var uprofile = new ProfileProcessing.Profile { SessionID = SessionID };
                if (uprofile.ChangeUserProfile(UserProfile))
                {
                    return new Tuple<bool, String>(true, uprofile.userProfile.Version.ToString());
                }
                else {
                    return new Tuple<bool, String>(false, "x12003");
                }
            }
            else {
                return new Tuple<bool, String>(false, "x12001");
            }
        }
        public Tuple<bool, String> ChangeDriverMode(int SessionID, String Sign, bool isDriver) {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var uprofile = new ProfileProcessing.Profile { SessionID = SessionID };
                if (uprofile.ChangeDriverMode(isDriver)) {
                    return new Tuple<bool, String>(true, uprofile.userProfile.Version.ToString());
                }
                else {
                    return new Tuple<bool, String>(false, "x13002");
                }
                
            }
            else {
                return new Tuple<bool, String>(false, "x12001");
            }
        }

        public Tuple<bool, String> BingMapsAPIKey(int SessionID, String Sign) {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                return new Tuple<bool, String>(true, BingMap.BingMapsAPI(SessionID));

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001");
            }
        }

        public Tuple<bool, string> StartOrganizationRegistration(int SessionID, string ClientSign, string email, string name)
        {
           
            if (ProfileProcessing.CheckSign(SessionID, ClientSign))
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

        public Tuple<bool, string> ConfirmOrganizationRegistration(int SessionID, string Sign, string encyptedConfirmationKey, string domain)
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
                        return new Tuple<bool, String>(false, "x14003"); //registration in approval
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



        public Tuple<bool, string> ChangeVisibility(int SessionID, string Sign, int ID, bool isVisible, bool isOrganization)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (isOrganization)
                {

                    if (OrganizationProcessing.ChangeVisibility(ID, isVisible))
                    {
                        return new Tuple<bool, string>(true, "");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "x14004");
                    }
                }
                else {
                    if (GroupProcessing.ChangeVisibility(ID, isVisible))
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


        public Tuple<bool, List<UserGroup>> LoadOrganizations(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new List<UserGroup>();

                var organizations = new OrganizationProcessing();
                var groups = new GroupProcessing();

                organizations.UserOrganizations = result;
                groups.UserGroups = result;
                if (organizations.LoadOrganizations() && groups.LoadGroups()) return new Tuple<bool, List<UserGroup>>(true, result);
                else {
                    return new Tuple<bool, List<UserGroup>>(false, new List<UserGroup>()); 
                }
            }
            else
            {
                return new Tuple<bool, List<UserGroup>>(false, new List<UserGroup>()); //wrong sign
            }
        }

        public Tuple<bool, List<OfficeOnMap>> LoadOffices(int SessionID, string Sign, double longtitude, double latitude)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new OrganizationProcessing();
                if (result.LoadOffices(longtitude, latitude)) return new Tuple<bool, List<OfficeOnMap>>(true, result.UserOffices);
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

        public Tuple<bool, string> StartJoinToOrganization(int SessionID, string Sign, string email)
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
                    var newJoiner = new OrganizationProcessing.Join { UserEmail = email, Domain=Domain };
                    
                    if (OrganizationProcessing.isOrgAlreadyCreated(Domain))
                    {
                        if (OrganizationProcessing.isAlreadyMember(Domain))
                        {
                            return new Tuple<bool, String>(false, "x14006"); //already member
                        }
                        else {
                            if (newJoiner.GenerateJoinerRegistrationEntry())
                            {
                                if (General.SendKeyToEmail(email, newJoiner.Password))
                                {
                                    return new Tuple<bool, String>(true, "x14001"); //email sent success
                                }
                                else {
                                    return new Tuple<bool, String>(false, "x10001"); //email sent not success
                                }
                            }
                            else {
                                return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
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




        public Tuple<bool, String> ConfirmJoinOrganization(int SessionID, string Sign, string encyptedConfirmationKey, string domain) {
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

        public Tuple<bool, string> LeaveOrganization(int SessionID, string Sign, int OrgID)
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

        public Tuple<bool, string> CreateOffice(int SessionID, string Sign, int TeamID, string Name, double longtitude, double latitude)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (OrganizationProcessing.isMemberOfOrganization(TeamID))
                {
                    if (!OrganizationProcessing.isOfficeAlreadyCreated(TeamID, Name))
                    {
                        if (OrganizationProcessing.CreateOffice(TeamID, Name, longtitude, latitude))
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
                else {
                    return new Tuple<bool, String>(false, "x14011"); //you are not a member
                }

               

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> SetHome(int SessionID, string Sign, double longtitude, double latitude)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (MapPoints.Houses.SetHome(longtitude, latitude)) {
                    return new Tuple<bool, String>(true, "x15000"); //created
                }
                else {
                    return new Tuple<bool, String>(false, "x15001"); //created
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, double, double> LoadHome(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var UserHome = MapPoints.Houses.LoadHome();
                if (UserHome.Item1!=0 && UserHome.Item2 != 0)
                {
                    return new Tuple<bool, double, double>(true, UserHome.Item1, UserHome.Item2); //created
                }
                else
                {
                    return new Tuple<bool, double, double>(false,0,0); //created
                }
            }
            else
            {
                return new Tuple<bool, double, double>(false, 0, 0); //created
            }
        }

        public Tuple<bool, int> SaveRoutePoint(int SessionID, string Sign, double longtitude, double latitude, bool Way,int PathID, WeekActuality Actuality)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points();
                if (NewPoint.SaveRoutePoint(longtitude, latitude, Way, PathID, Actuality)) {
                    return new Tuple<bool, int>(true, NewPoint.RouteID); //saved N;
                }
                else {
                    return new Tuple<bool, int>(false, 0); //saved
                }
            }
            else
            {
                return new Tuple<bool, int>(false, 0); //saved
            }
        }

        public Tuple<bool, List<Route>> LoadRoutePoints(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var PointsArr = new MapPoints.Points();
                if (PointsArr.LoadUserRoutePoints())
                {
                    return new Tuple<bool, List<Route>>(true, PointsArr.RoutePointsArr);
                }
                else
                {
                    return new Tuple<bool, List<Route>>(false, new List<Route>());
                }
            }
            else
            {
                return new Tuple<bool, List<Route>>(false, new List<Route>());
            }
        }

        public Tuple<bool, List<OnMapPoint>> FindingCompanions(int SessionID, string Sign, double longtitude, double latitude, Boolean Way, DateTime Date)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var result = new CompanionProcessing();
                result.FindCompanionsRoutePoints(Date, Way);
                result.FindCompanionsHomePoints();
                return new Tuple<bool, List<OnMapPoint>>(true, result.PointsArray);

            }
            else
            {
                return new Tuple<bool, List<OnMapPoint>>(false, new List<OnMapPoint>());
            }
        }

        public Tuple<bool, string> ChangeRoutePoint(int SessionID, string Sign, int RouteID, int SysCode)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points { RouteID = RouteID };
                if (NewPoint.ChangeRoutePoint(SysCode))
                {
                    return new Tuple<bool, String>(true, "x17001"); //Success
                }
                else
                {
                    return new Tuple<bool, String>(false, "x17000"); 
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> ChangePath(int SessionID, string Sign, int PathID, int SysCode)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var NewPoint = new MapPoints.Points { PathID = PathID };
                if (NewPoint.ChangePath(SysCode))
                {
                    return new Tuple<bool, String>(true, "x17001"); //Success
                }
                else
                {
                    return new Tuple<bool, String>(false, "x17000");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> NewComplaint(int SessionID, string Sign, int SysCode, int UniqID)
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
                    else {
                        return new Tuple<bool, String>(false, "x18003"); //wrong syscode
                    }
                }
                else {
                    return new Tuple<bool, String>(false, "x18002"); //wrong sign
                }


            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, List<String>> GetCarBrands()
        {
            return new Tuple<bool, List<string>>(true, CarProcessing.GetCurrentAvailableCarBrands());
        }

        public Tuple<bool, List<string>> GetCarModels(int CarBrandID)
        {
            return new Tuple<bool, List<string>>(true, CarProcessing.GetCurrentAvailableCaModels(CarBrandID));
        }

        public Tuple<bool, UserCar> LoadUserCar(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CarProcessing();
                if(car.LoadUserCar()) return new Tuple<bool, UserCar>(true, car.UserCar);
                else return new Tuple<bool, UserCar>(false, new UserCar());

            }
            else
            {
                return new Tuple<bool, UserCar>(false, new UserCar());
            }
        }

        public Tuple<bool, string> UpdateCar(int SessionID, string Sign, UserCar UserCar)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CarProcessing { UserCar = UserCar };
                if (car.ChangeUserCar()) return new Tuple<bool, string>(true, "x19000");
                else return new Tuple<bool, string>(true, "x19001");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, List<UserGroup>> LoadGroups(int SessionID, string Sign)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> GenerateInviteToGroup(int SessionID, string Sign, int GroupID)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> CreateGroup(int SessionID, string Sign, string Name)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> JoinGroup(int SessionID, string Sign, int GroupID, string Answer)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> LeaveGroup(int SessionID, string Sign, int GroupID, string Answer)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, UserCompanion> GetUserInfo(int SessionID, string Sign, int SysId)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var car = new CompanionProcessing { FriendUID = SysId };
                if (car.isFriend()) {
                    try
                    {
                        return new Tuple<bool, UserCompanion>(true, car.GetUserInfo());
                    }
                    catch {
                        return new Tuple<bool, UserCompanion>(false, new UserCompanion());
                    }
                }
                else return new Tuple<bool, UserCompanion>(false, new UserCompanion());

            }
            else
            {
                return new Tuple<bool, UserCompanion>(false, new UserCompanion());
            }
        }

        public Tuple<bool, WeeklySchedule> LoadSchedule(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing();
                if (schedule.LoadSchedule())
                {
                    try
                    {
                        return new Tuple<bool, WeeklySchedule>(true, schedule.UserSchedule);
                    }
                    catch
                    {
                        return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
                    }
                }
                else return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());

            }
            else
            {
                return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
            }
        }

        public Tuple<bool, string> UpdateSchedule(int SessionID, string Sign, WeeklySchedule UserSchedule)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var schedule = new ScheduleProcessing { UserSchedule = UserSchedule };
                if (schedule.UpdateSchedule())
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x21001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x21002");
                    }
                }
                else return new Tuple<bool, String>(false, "x21002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> UpdateLocation(int SessionID, string Sign, double longtitude, double latitude)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var location = new LocationProcessing { longtitude = longtitude, latitude = latitude };
                if (location.UpdateLocation())
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x22001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x22002");
                    }
                }
                else return new Tuple<bool, String>(false, "x22002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, Tuple<double,double>> LoadPreviousLocation(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var location = new LocationProcessing();
                if (location.LoadLocation())
                {
                    try
                    {
                        return new Tuple<bool, Tuple<double,double>>(true, new Tuple<double, double>(location.longtitude,location.latitude));
                    }
                    catch
                    {
                        return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
                    }
                }
                else return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));

            }
            else
            {
                return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
            }
        }

        public Tuple<bool, List<Conversation>> LoadMessages(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var messages = new MessagesProcessing();
                if (messages.UserMessages())
                {
                    try
                    {
                        return new Tuple<bool, List<Conversation>>(true, messages.Convesations);
                    }
                    catch
                    {
                        return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
                    }
                }
                else return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
            }
            else
            {
                return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
            }
        }

        public Tuple<bool, string> SendMessage(int SessionID, string Sign, int OpponentUID, string Text)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if(Text.Length>300) return new Tuple<bool, String>(false, "x24003");
                var messages = new MessagesProcessing();
                if (messages.SendMessage(App.UID,OpponentUID,Text))
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x24001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x24002");
                    }
                }
                else return new Tuple<bool, String>(false, "x24002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, List<TripOffer>> LoadTrips(int SessionID, string Sign, DateTime CurrentUserTime)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (Trip.LoadOffers(CurrentUserTime))
                {
                    return new Tuple<bool, List<TripOffer>>(true, Trip.Offers);
                }
                else return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());
            }
            else
            {
                return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());
            }
        }

        public Tuple<bool, string> SendTripOffer(int SessionID, string Sign, int ToUID, int RouteID, bool isToHome, DateTime atTime)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing { isToHome = isToHome };
                if (Trip.SendOffer(ToUID, RouteID, atTime)) {
                    return new Tuple<bool, String>(true, "x25001");
                }
                else return new Tuple<bool, String>(false, "x25002");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> AcceptOffer(int SessionID, string Sign, int OfferID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (Trip.AcceptOffer(OfferID))
                {
                    return new Tuple<bool, String>(true, "x26001");
                }
                else return new Tuple<bool, String>(false, "x26002");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> RejectOffer(int SessionID, string Sign, int OfferID)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var Trip = new TripProcessing();
                if (Trip.RejectOffer(OfferID))
                {
                    return new Tuple<bool, String>(true, "x27001");
                }
                else return new Tuple<bool, String>(false, "x27002");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public Tuple<bool, string> InternalOrganizationKey(int SessionID, string Sign, string email, string RSAPublic)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var decryptedemail = "";
                var sessionid=0;
                try
                {
                    var RSAEngine = new Shared.Crypting.RSA();
                    var decrypted = RSAEngine.Decrypt(email, "BwIAAACkAABSU0EyAAgAAAEAAQD5qOFWlF+s48mh+D7+nMKKVMtgd0jiSeum8ZUnDAReke5vdkG95JUwmCbVnfntXl4iR6R4PoyhT4BYT0f0CXZnNrEmjtNl2ZfRqQBoqdbwCfBwjG9pb5LvjSR2PaadDtuujEL5+jTOSmR01/RhsUj8EYPFXemLG5QNgHDbF4oDuuyq+jXVR00LCxqDHZ2xyJeNkCWIVWrafB8Y7+gZ+Ny4Di6wpA1/TRD//4se2RHuXtjF2SfFcdME+a4nfGAlpv5aLsRdHZ8c9juhfPSxtn7DxlqxCmvQmWqWmPVqL4euFivPmqAi1zt6vaxjE+7drTbcNYVzkMjtiZReAB4s+Oa2z9IWTpDiR59BHJSf4TFrJy26mhyy9ukbH/7n6GVZWi9ZOYpTOQNjix1eEFDKJMKck1nJfphA87vymY5avxPWw1eKw3xmTB5xqQ2h4Gemi2WdvcNBVPBh7n+MDUB44MaG8LMZrEmb5vnK0ccy9sI7IIRTtx8orqbxP3rnbt83ZLy3WQkUL7wozcEDoQY+UoeA1hqlw3GVqsr9aJMYi/XN/Kpq+GW06OkCI2HPz0sYtAxytfM/r0xQwtOdVFgdOfe7QUBmaWwSD5JPB4pzS5M+CmLXa58pbbuXIlXVTZ0KHvTkvoLT6ATanFtKCn7KllPWj6qDJVWL+crKpoq7LHKK+CGCqmrQrQTMqcHi0wOwg0cwVTrPs+vSG0nq1FnLmWJ3aB4u8D+Sbx/av2xM4l4SyaDOPoeUuEcl8XaA4LdJcZrFX1/PFuFF3O2MkP/XFnXk+abo3T8nFxDxWiqee+VeHtY9je0QHd2RdUYHr/lpWJMh/meDcCVh+w52/Qw1equKz2pI6MKWQZsseiA+eoXIJZjTOmYgVc9AHCCsPtJffrZ0MPh0Zp7fFlHIaTm4MdHwLo263qM3SWYa9eDicoQVfrHtBPyRhhU5WEWRjc84f2UG61LzcsoXqUVHIkO3Ric8j0tA85MIwuWFva7Zx1l4t5SP+3hXvvdI+l6PJ7sI23X7+1fOZf+IPq5tKCIkELEkwMB6paf8cnrd1CHtgs69s6ZVbHV3/I4C/dGPaPeiW5HKN5RtCPUZdGlTS08DsxtxCWwI7ECdoFwGZg9jjX5j0/qfne+MQpPdWVJpTLfXU+OCAFG7Lbvrw4hjwZs2dRIdtLC9bYbrxKKQfsolZpx5FH2bRlq/LRjRKClMezx6XnBHTp1EkaGcM+I7xu1QS7tFXIshe8EXBgN5KpxJ94fdTvgFmI4xp4bBlhS/BxJM1NHoVeKsYYv62UWIfPspub675y9M5LjCIbppJcQfMnqqcTtVYiFeLMv4hvI61V+Yk6x4iKDJKDwHJsL/Rp9wyfFz3tyV2c9f8KeqcxU2RA49xJ/qSHIfGKBh9MLOPQMnlJIrJHs4vwa12zcUTFkzEfmvrOkKPrj1WeTLCH109jptk8RAQ4blJnoW8PmzXWwj/hxgVXE9P+BtPupfeOaY9LFxDDe2CsVkkRxgfNZjcLKACB6j1ZwZqyhK83VkmgIDRAw=");
                    var val= decrypted.Split('|');
                    decryptedemail = val[0];
                    sessionid = int.Parse(val[1]);
                    if(sessionid!= SessionID) return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                }
                catch (Exception) {
                    return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                }
                var Domain = decryptedemail.Split('@')[1];
                if (OrganizationProcessing.isAlreadyMember(Domain))
                {
                    return new Tuple<bool, String>(false, "x14006"); //already member
                }
                else
                {
                    var newJoiner = new OrganizationProcessing.Join { UserEmail = decryptedemail, Domain = Domain };
                    if (newJoiner.GenerateJoinerRegistrationEntry())
                    {
                        if (General.SendKeyToEmail(decryptedemail, newJoiner.Password))
                        {
                            var aesEngine = new Shared.Crypting.RSA();
                            var response = aesEngine.Encrypt(newJoiner.Password, RSAPublic);
                            return new Tuple<bool, String>(true, response); //email sent success
                        }
                        else
                        {
                            return new Tuple<bool, String>(false, "x10001"); //email sent not success
                        }
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                    }
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }

        public List<AppFeedback> LoadAppFeedbacks()
        {
            return FeedbackProcessing.Load();
        }

        public Tuple<bool, string> SaveFeedBack(int SessionID, string Sign, byte Stars, string Feedback)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (FeedbackProcessing.Save(Feedback,Stars))
                {
                    return new Tuple<bool, String>(true, "x47001"); //stored feedback
                }
                else return new Tuple<bool, String>(false, "x47002");
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
