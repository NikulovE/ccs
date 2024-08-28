using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CarBuddyAPI
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICarBuddyService
    {

        [OperationContract]
        Tuple<bool, String> StartRegistration(string email);

        [OperationContract]
        Tuple<bool, String> ConfirmRegistration(int UID, string answer);

        [OperationContract]
        Tuple<bool, String> RestoreRegistration(string email);

        [OperationContract]
        Tuple<bool, String> RestoreSessionKey(int UID, string ClientSign);

        [OperationContract]
        Tuple<bool, UserProfile> LoadProfile(int SessionID, string EncryptedSessionKey);

        [OperationContract]
        Tuple<bool, String> Logon(int SessionID, string EncryptedSessionKey);

        [OperationContract]
        Tuple<bool, String> UpdateProfile(int SessionID, String Sign, UserProfile UserProfile);

        [OperationContract]
        Tuple<bool, String> ChangeDriverMode(int SessionID, String Sign, bool isDriver);




        [OperationContract]
        Tuple<bool, List<UserGroup>> LoadGroups(int SessionID, String Sign);

        [OperationContract]
        Tuple<bool, string> GenerateInviteToGroup(int SessionID, String Sign, int GroupID);

        [OperationContract]
        Tuple<bool, string> CreateGroup(int SessionID, String Sign, String Name);

        [OperationContract]
        Tuple<bool, string> JoinGroup(int SessionID, String Sign, int GroupID, String Answer);

        [OperationContract]
        Tuple<bool, string> LeaveGroup(int SessionID, String Sign, int GroupID, String Answer);


        [OperationContract]
        Tuple<bool, List<UserGroup>> LoadOrganizations(int SessionID, String Sign);

        [OperationContract]
        Tuple<bool, List<OfficeOnMap>> LoadOffices(int SessionID, String Sign, double longtitude, double latitude);



        [OperationContract]
        Tuple<bool, String> ChangeVisibility(int SessionID, string Sign, int ID, bool isVisible, bool isOrganization);




        [OperationContract]
        Tuple<bool, String> StartOrganizationRegistration(int SessionID, string Sign, string email, string name);

        [OperationContract]
        Tuple<bool, String> ConfirmOrganizationRegistration(int SessionID, string Sign, string encyptedConfirmationKey, string domain);

        [OperationContract]
        Tuple<bool, String> InternalOrganizationKey(int SessionID, string Sign, string Organization, string RSAPublic);


        [OperationContract]
        Tuple<bool, String> StartJoinToOrganization(int SessionID, string Sign, string Email);

        [OperationContract]
        Tuple<bool, String> ConfirmJoinOrganization(int SessionID, string Sign, string EncryptedConfirmationKey, string domain);

        [OperationContract]
        Tuple<bool, String> LeaveOrganization(int SessionID, string Sign, int OrgID);

        

        [OperationContract]
        Tuple<bool, String> CreateOffice(int SessionID, string Sign, int TeamID, String Name, Double longtitude, Double latitude);



        [OperationContract]
        Tuple<bool, String> BingMapsAPIKey(int SessionID, String Sign);


        [OperationContract]
        Tuple<bool, String> SetHome(int SessionID, string Sign, Double longtitude, Double latitude);

        [OperationContract]
        Tuple<bool, int> SaveRoutePoint(int SessionID, string Sign, Double longtitude, Double latitude, bool Way, int PathID, WeekActuality Actuality);

        [OperationContract]
        Tuple<bool, String> ChangeRoutePoint(int SessionID, string Sign, int RouteID, int SysCode);


        [OperationContract]
        Tuple<bool, String> ChangePath(int SessionID, string Sign, int PathID, int SysCode);

        [OperationContract]
        Tuple<bool, List<Route>> LoadRoutePoints(int SessionID, string Sign);

        [OperationContract]
        Tuple<bool, double, double> LoadHome(int SessionID, string Sign);

        [OperationContract]
        Tuple<bool, List<OnMapPoint>> FindingCompanions(int SessionID, string Sign, Double longtitude, Double latitude, Boolean Way, DateTime Date);

        [OperationContract]
        Tuple<bool, String> NewComplaint(int SessionID, string Sign, int SysCode, int UniqID);
        [OperationContract]
        Tuple<bool, List<String>> GetCarBrands();
        [OperationContract]
        Tuple<bool, List<String>> GetCarModels(int CarBrandID);
        [OperationContract]
        Tuple<bool, UserCar> LoadUserCar(int SessionID, string Sign);
        [OperationContract]
        Tuple<bool, String> UpdateCar(int SessionID, String Sign, UserCar UserCar);
        [OperationContract]
        Tuple<bool, UserCompanion> GetUserInfo(int SessionID, String Sign, int SysId);



        [OperationContract]
        Tuple<bool, WeeklySchedule> LoadSchedule(int SessionID, string Sign);
        [OperationContract]
        Tuple<bool, String> UpdateSchedule(int SessionID, string Sign, WeeklySchedule UserSchedule);



        [OperationContract]
        Tuple<bool, String> UpdateLocation(int SessionID, string Sign, double longtitude, double latitude);

        [OperationContract]
        Tuple<bool, Tuple<double,double>> LoadPreviousLocation(int SessionID, string Sign);



        [OperationContract]
        Tuple<bool, List<Conversation>> LoadMessages(int SessionID, string Sign);

        [OperationContract]
        Tuple<bool, String> SendMessage(int SessionID, string Sign, int OpponentUID, String Text);


        [OperationContract]
        Tuple<bool, List<TripOffer>> LoadTrips(int SessionID, string Sign, DateTime CurrentUserTime);

        [OperationContract]
        Tuple<bool, String> SendTripOffer(int SessionID, string Sign, int ToUID, int RouteID, bool isToHome, DateTime atTime);

        [OperationContract]
        Tuple<bool, String> AcceptOffer(int SessionID, string Sign, int OfferID);

        [OperationContract]
        Tuple<bool, String> RejectOffer(int SessionID, string Sign, int OfferID);

        [OperationContract]
        List<AppFeedback> LoadAppFeedbacks();

        [OperationContract]
        Tuple<bool, String> SaveFeedBack(int SessionID, string Sign, byte Stars, string Feedback);

    }
    
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Conversation {
        List<UserMessage> conversation = new List<UserMessage> ();
        string with;
        int withUID;

        [DataMember]
        public String With
        {
            get { return with; }
            set { with = value; }
        }

        [DataMember]
        public int WithUID
        {
            get { return withUID; }
            set { withUID = value; }
        }

        [DataMember]
        public List<UserMessage> UserConversation
        {
            get { return conversation; }
            set { conversation = value; }
        }

    }


    [DataContract]
    public class UserMessage
    {
        string from;
        string to;
        int fromUID;
        int toUID;
        DateTime time;
        int syscode;
        string text;

        [DataMember]
        public int FromUID
        {
            get { return fromUID; }
            set { fromUID = value; }
        }

        [DataMember]
        public int ToUID
        {
            get { return toUID; }
            set { toUID = value; }
        }

        [DataMember]
        public String From
        {
            get { return from; }
            set { from = value; }
        }

        [DataMember]
        public String To
        {
            get { return to; }
            set { to = value; }
        }

        [DataMember]
        public String MessageText
        {
            get { return text; }
            set { text = value; }
        }

        [DataMember]
        public int SysCode
        {
            get { return syscode; }
            set { syscode = value; }
        }

        [DataMember]
        public DateTime TimeStamp
        {
            get { return time; }
            set { time = value; }
        }

    }

    [DataContract]
    public class TripOffer
    {
        String companion;
        int companionID;
        bool isCanBeAccepted;
        double longtitude;
        double latitude;
        DateTime startTime;
        bool tohome;
        bool confirmed;
        bool repeatable;
        int offerID;



        [DataMember]
        public int CompanionID
        {
            get { return companionID; }
            set { companionID = value; }
        }

        [DataMember]
        public bool IsRepeat
        {
            get { return repeatable; }
            set { repeatable = value; }
        }

        [DataMember]
        public bool Confirmed
        {
            get { return confirmed; }
            set { confirmed = value; }
        }

        [DataMember]
        public bool IsToHome
        {
            get { return tohome; }
            set { tohome = value; }
        }

        [DataMember]
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        [DataMember]
        public int OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }

        [DataMember]
        public String Companion
        {
            get { return companion; }
            set { companion = value; }
        }

        [DataMember]
        public bool IsCanBeAccepted
        {
            get { return isCanBeAccepted; }
            set { isCanBeAccepted = value; }
        }

        [DataMember]
        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

    }

    [DataContract]
    public class AppFeedback
    {
        int uid;
        String userName;
        byte stars;
        String feedbackText;
        String developerAnswer;



        [DataMember]
        public int UID
        {
            get { return uid; }
            set { uid = value; }
        }

        [DataMember]
        public byte Stars
        {
            get { return stars; }
            set { stars = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [DataMember]
        public string FeedbackText
        {
            get { return feedbackText; }
            set { feedbackText = value; }
        }

        [DataMember]
        public string DeveloperAnswer
        {
            get { return developerAnswer; }
            set { developerAnswer = value; }
        }


    }

    [DataContract]
    public class UserCar
    {
        int carBrand;
        int carModel;
        int passangerPlaces;
        String brand;
        String model;
        String govNumber;
        String comfortOptions;

        [DataMember]
        public int BrandID
        {
            get { return carBrand; }
            set { carBrand = value; }
        }

        [DataMember]
        public int ModelID
        {
            get { return carModel; }
            set { carModel = value; }
        }

        [DataMember]
        public String Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        [DataMember]
        public String Model
        {
            get { return model; }
            set { model = value; }
        }

        [DataMember]
        public int Places
        {
            get { return passangerPlaces; }
            set { passangerPlaces = value; }
        }

        [DataMember]
        public String GovNumber
        {
            get { return govNumber; }
            set { govNumber = value; }
        }

        [DataMember]
        public String ComfortOptions
        {
            get { return comfortOptions; }
            set { comfortOptions = value; }
        }

    }

    [DataContract]
    public class Route {
        int pathId;
        WeekActuality actuality;
        List<OnMapPoint> points = new List<OnMapPoint>();
        bool isToHome;


        [DataMember]
        public bool IsMon
        {
            get { return actuality.Monday; }
            set { actuality.Monday = value; }
        }

        [DataMember]
        public bool IsTue
        {
            get { return actuality.Tuesday; }
            set { actuality.Tuesday = value; }
        }

        [DataMember]
        public bool IsWed
        {
            get { return actuality.Wednesday; }
            set { actuality.Wednesday = value; }
        }

        [DataMember]
        public bool IsThu
        {
            get { return actuality.Thursday; }
            set { actuality.Thursday = value; }
        }

        [DataMember]
        public bool IsFri
        {
            get { return actuality.Monday; }
            set { actuality.Monday = value; }
        }

        [DataMember]
        public bool IsSat
        {
            get { return actuality.Saturday; }
            set { actuality.Saturday = value; }
        }

        [DataMember]
        public bool IsSun
        {
            get { return actuality.Sunday; }
            set { actuality.Sunday = value; }
        }

        [DataMember]
        public bool IsToHome
        {
            get { return isToHome; }
            set { isToHome = value; }
        }

        [DataMember]
        public WeekActuality Actuality
        {
            get { return actuality; }
            set { actuality = value; }
        }

        [DataMember]       
        public List<OnMapPoint> Points
        {
            get { return points; }
            set { points = value; }

        }
        [DataMember]
        public int PathID
        {
            get { return pathId; }
            set { pathId = value; }
        }

    }

    [DataContract]
    public class OnMapPoint
    {
        int uid;
        int pointId;
        Double longtitude;
        Double latitude;
        bool way;

        bool isHome;

        [DataMember]
        public bool IsHome
        {
            get { return isHome; }
            set { isHome = value; }
        }

        [DataMember]
        public int UID
        {
            get { return uid; }
            set { uid = value; }
        }

        [DataMember]
        public int PointID
        {
            get { return pointId; }
            set { pointId = value; }
        }

        [DataMember]
        public Double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        [DataMember]
        public Double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public bool Way
        {
            get { return way; }
            set { way = value; }
        }

    }

    [DataContract]
    public class WeekActuality {
        bool monday;
        bool tuesday;
        bool wednesday;
        bool thursday;
        bool friday;
        bool saturday;
        bool sunday;
        [DataMember]
        public bool Monday
        {
            get { return monday; }
            set { monday = value; }
        }
        [DataMember]
        public bool Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; }
        }
        [DataMember]
        public bool Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; }
        }
        [DataMember]
        public bool Thursday
        {
            get { return thursday; }
            set { thursday = value; }
        }
        [DataMember]
        public bool Friday
        {
            get { return friday; }
            set { friday = value; }
        }
        [DataMember]
        public bool Saturday
        {
            get { return saturday; }
            set { saturday = value; }
        }
        [DataMember]
        public bool Sunday
        {
            get { return sunday; }
            set { sunday = value; }
        }

    }

    [DataContract]
    public class UserProfile
    {
        String firstName;
        String lastName;
        String phone;
        string extension;
        bool isDriver;
        decimal rating;
        int officeID;
        int payment;        
        int version;

        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [DataMember]
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        [DataMember]
        public String Extension
        {
            get { return extension; }
            set { extension = value; }
        }
        [DataMember]
        public bool IsDriver
        {
            get { return isDriver; }
            set { isDriver = value; }
        }
        [DataMember]
        public int OfficeID
        {
            get { return officeID; }
            set { officeID = value; }
        }
       
        [DataMember]
        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }
        [DataMember]
        public decimal Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        [DataMember]
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

    }

    [DataContract]
    public class UserGroup {
        String name;
        int teamID;
        Boolean isVisible;
        Boolean isOrganization;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public int TeamID
        {
            get { return teamID; }
            set { teamID = value; }
        }

        [DataMember]
        public Boolean IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        [DataMember]
        public Boolean IsOrganization
        {
            get { return isOrganization; }
            set { isOrganization = value; }
        }


    }

    [DataContract]
    public class OfficeOnMap
    {
        String name;
        int id;
        int organizationID;
        Double longtitude;
        Double latitude;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public int OrganizationID
        {
            get { return organizationID; }
            set { organizationID = value; }
        }

        [DataMember]
        public Double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        [DataMember]
        public Double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }



    }


    [DataContract]
    public class WeeklySchedule
    {
        DaySchedule monday = new DaySchedule();
        [DataMember]
        public DaySchedule Monday {
            get { return monday; }
            set { monday = value; }
        }

        DaySchedule tuesday = new DaySchedule();
        [DataMember]
        public DaySchedule Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; }
        }


        DaySchedule wednesday = new DaySchedule();
        [DataMember]
        public DaySchedule Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; }
        }

        DaySchedule thursday = new DaySchedule();
        [DataMember]
        public DaySchedule Thursday
        {
            get { return thursday; }
            set { thursday = value; }
        }

        DaySchedule friday = new DaySchedule();
        [DataMember]
        public DaySchedule Friday
        {
            get { return friday; }
            set { friday = value; }
        }

        DaySchedule saturday = new DaySchedule();
        [DataMember]
        public DaySchedule Saturday
        {
            get { return saturday; }
            set { saturday = value; }
        }

        DaySchedule sunday = new DaySchedule();
        [DataMember]
        public DaySchedule Sunday
        {
            get { return sunday; }
            set { sunday = value; }
        }

    }

    [DataContract]
    public class DaySchedule {

        TimeSpan toHome;
        TimeSpan toWork;
        bool isEnabled;


        [DataMember]
        public TimeSpan ToHome
        {
            get { return toHome; }
            set { toHome = value; }
        }
        [DataMember]
        public TimeSpan ToWork
        {
            get { return toWork; }
            set { toWork = value; }
        }
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
    }

    [DataContract]
    public class UserCompanion
    {
        int passangerPlaces;
        String brand;
        String model;
        String govNumber;
        String comfortOptions;
        String firstName;
        String lastName;
        String email;
        String phone;
        Decimal rating;
        int uid;
        int payment;



        [DataMember]
        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }

        [DataMember]
        public int UID
        {
            get { return uid; }
            set { uid = value; }
        }

        [DataMember]
        public String Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        [DataMember]
        public String Model
        {
            get { return model; }
            set { model = value; }
        }

        [DataMember]
        public int Places
        {
            get { return passangerPlaces; }
            set { passangerPlaces = value; }
        }

        [DataMember]
        public String GovNumber
        {
            get { return govNumber; }
            set { govNumber = value; }
        }

        [DataMember]
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [DataMember]
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [DataMember]
        public String Email
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        [DataMember]
        public Decimal Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        [DataMember]
        public String Comfort
        {
            get { return comfortOptions; }
            set { comfortOptions = value; }
        }



    }


}
