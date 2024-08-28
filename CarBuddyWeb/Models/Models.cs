using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AppWeb.Models
{
    [DataContract]
    public class Conversation
    {
        List<UserMessage> conversation = new List<UserMessage>();
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
        long time;
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
        public long TimeStamp
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
        long startTime;
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
        public long StartTime
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
    public class ShortOffer
    {
        int toUID;
        int routeID;
        bool isToHome;
        DateTime atTime;

        [DataMember]
        public int ToUID
        {
            get { return toUID; }
            set { toUID = value; }
        }

        [DataMember]
        public int RouteID
        {
            get { return routeID; }
            set { routeID = value; }
        }

        [DataMember]
        public bool IsToHome
        {
            get { return isToHome; }
            set { isToHome = value; }
        }
        [DataMember]
        public DateTime AtTime
        {
            get { return atTime; }
            set { atTime = value; }
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
    public class Path
    {
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
            get { return actuality.Friday; }
            set { actuality.Friday = value; }
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
    public class RP
    {
        double longtitude;
        double latitude;
        WeekActuality actuality;
        bool isToHome;
        int pathID;


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

        [DataMember]
        public WeekActuality Actuality
        {
            get { return actuality; }
            set { actuality = value; }
        }


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
            get { return actuality.Friday; }
            set { actuality.Friday = value; }
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
        public int PathID
        {
            get { return pathID; }
            set { pathID = value; }
        }

    }


    [DataContract]
    public class CompanionsRequest {
        double longtitude; double latitude; Boolean isToHome; DateTime date;

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
        [DataMember]
        public bool IsToHome
        {
            get { return isToHome; }
            set { isToHome = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
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
    public class WeekActuality
    {
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
    public class UserGroup
    {
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
        public DaySchedule Monday
        {
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
    public class DaySchedule
    {

        long toHome;
        long toWork;
        bool isEnabled;


        [DataMember]
        public long ToHome
        {
            get { return toHome; }
            set { toHome = value; }
        }
        [DataMember]
        public long ToWork
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
    public class Location
    {

        double longitude;
        double latidude;


        [DataMember]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        [DataMember]
        public double Latitude
        {
            get { return latidude; }
            set { latidude = value; }
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