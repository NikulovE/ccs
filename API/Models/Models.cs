using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Shared.Model
{
    public class Location
    {

        double longitude = 0;
        double latidude = 0;

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location()
        {

        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public double Latitude
        {
            get { return latidude; }
            set { latidude = value; }
        }
#if NETFX_CORE
#if WINDOWS_UWP
            public static implicit operator Windows.Devices.Geolocation.Geopoint(Location v)
            {
                return new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition { Longitude = v.Longitude, Latitude = v.Latitude });
            }
#else
            public static implicit operator Bing.Maps.Location(Location v)
            {
                return new Bing.Maps.Location { Longitude = v.longitude, Latitude = v.latidude };
            }

        public static implicit operator Location(Bing.Maps.Location v)
        {
            return new Location { Longitude = v.Longitude, Latitude = v.Latitude };
        }
#endif
#else
#if ANDROID
#else
#if WPF
        public static implicit operator Microsoft.Maps.MapControl.WPF.Location(Location v)
        {
                return new Microsoft.Maps.MapControl.WPF.Location { Longitude = v.longitude, Latitude = v.latidude };
        }

        public static explicit operator Location(Microsoft.Maps.MapControl.WPF.Location v)
        {
            return new Location { Longitude = v.Longitude, Latitude = v.Latitude };
        }
#endif
#endif

#endif

    }

    public partial class Conversation : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ObservableCollection<Model.UserMessage> UserConversationField;

        private string WithField;
        private string NewMessageField;

        private int WithUIDField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ObservableCollection<Model.UserMessage> UserConversation
        {
            get
            {
                return this.UserConversationField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UserConversationField, value) != true))
                {
                    this.UserConversationField = value;
                    this.RaisePropertyChanged("UserConversation");
                }
            }
        }


        public string NewMessage
        {
            get
            {
                return this.NewMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NewMessageField, value) != true))
                {
                    this.NewMessageField = value;
                    this.RaisePropertyChanged("NewMessageField");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string With
        {
            get
            {
                return this.WithField;
            }
            set
            {
                if ((object.ReferenceEquals(this.WithField, value) != true))
                {
                    this.WithField = value;
                    this.RaisePropertyChanged("With");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int WithUID
        {
            get
            {
                return this.WithUIDField;
            }
            set
            {
                if ((this.WithUIDField.Equals(value) != true))
                {
                    this.WithUIDField = value;
                    this.RaisePropertyChanged("WithUID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class UserMessage : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string FromField;

        private int FromUIDField;

        private string MessageTextField;

        private int SysCodeField;

        private long TimeStampField;

        private string ToField;

        private int ToUIDField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string From
        {
            get
            {
                return this.FromField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FromField, value) != true))
                {
                    this.FromField = value;
                    this.RaisePropertyChanged("From");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int FromUID
        {
            get
            {
                return this.FromUIDField;
            }
            set
            {
                if ((this.FromUIDField.Equals(value) != true))
                {
                    this.FromUIDField = value;
                    this.RaisePropertyChanged("FromUID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageText
        {
            get
            {
                return this.MessageTextField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MessageTextField, value) != true))
                {
                    this.MessageTextField = value;
                    this.RaisePropertyChanged("MessageText");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SysCode
        {
            get
            {
                return this.SysCodeField;
            }
            set
            {
                if ((this.SysCodeField.Equals(value) != true))
                {
                    this.SysCodeField = value;
                    this.RaisePropertyChanged("SysCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TimeStamp
        {
            get
            {
                return this.TimeStampField;
            }
            set
            {
                if ((this.TimeStampField.Equals(value) != true))
                {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string To
        {
            get
            {
                return this.ToField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ToField, value) != true))
                {
                    this.ToField = value;
                    this.RaisePropertyChanged("To");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ToUID
        {
            get
            {
                return this.ToUIDField;
            }
            set
            {
                if ((this.ToUIDField.Equals(value) != true))
                {
                    this.ToUIDField = value;
                    this.RaisePropertyChanged("ToUID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class TripOffer : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string CompanionField;

        private int CompanionIDField;

        private bool ConfirmedField;

        private bool IsCanBeAcceptedField;

        private bool IsRepeatField;

        private bool IsToHomeField;

        private double LatitudeField;

        private double LongtitudeField;

        private int OfferIDField;

        private long StartTimeField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Companion
        {
            get
            {
                return this.CompanionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CompanionField, value) != true))
                {
                    this.CompanionField = value;
                    this.RaisePropertyChanged("Companion");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CompanionID
        {
            get
            {
                return this.CompanionIDField;
            }
            set
            {
                if ((this.CompanionIDField.Equals(value) != true))
                {
                    this.CompanionIDField = value;
                    this.RaisePropertyChanged("CompanionID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Confirmed
        {
            get
            {
                return this.ConfirmedField;
            }
            set
            {
                if ((this.ConfirmedField.Equals(value) != true))
                {
                    this.ConfirmedField = value;
                    this.RaisePropertyChanged("Confirmed");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsCanBeAccepted
        {
            get
            {
                return this.IsCanBeAcceptedField;
            }
            set
            {
                if ((this.IsCanBeAcceptedField.Equals(value) != true))
                {
                    this.IsCanBeAcceptedField = value;
                    this.RaisePropertyChanged("IsCanBeAccepted");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsRepeat
        {
            get
            {
                return this.IsRepeatField;
            }
            set
            {
                if ((this.IsRepeatField.Equals(value) != true))
                {
                    this.IsRepeatField = value;
                    this.RaisePropertyChanged("IsRepeat");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsToHome
        {
            get
            {
                return this.IsToHomeField;
            }
            set
            {
                if ((this.IsToHomeField.Equals(value) != true))
                {
                    this.IsToHomeField = value;
                    this.RaisePropertyChanged("IsToHome");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                if ((this.LatitudeField.Equals(value) != true))
                {
                    this.LatitudeField = value;
                    this.RaisePropertyChanged("Latitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Longtitude
        {
            get
            {
                return this.LongtitudeField;
            }
            set
            {
                if ((this.LongtitudeField.Equals(value) != true))
                {
                    this.LongtitudeField = value;
                    this.RaisePropertyChanged("Longtitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OfferID
        {
            get
            {
                return this.OfferIDField;
            }
            set
            {
                if ((this.OfferIDField.Equals(value) != true))
                {
                    this.OfferIDField = value;
                    this.RaisePropertyChanged("OfferID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long StartTime
        {
            get
            {
                return this.StartTimeField;
            }
            set
            {
                if ((this.StartTimeField.Equals(value) != true))
                {
                    this.StartTimeField = value;
                    this.RaisePropertyChanged("StartTime");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ShortOffer
    {
        int toUID;
        int routeID;
        bool isToHome;
        DateTime atTime;


        public int ToUID
        {
            get { return toUID; }
            set { toUID = value; }
        }


        public int RouteID
        {
            get { return routeID; }
            set { routeID = value; }
        }


        public bool IsToHome
        {
            get { return isToHome; }
            set { isToHome = value; }
        }

        public DateTime AtTime
        {
            get { return atTime; }
            set { atTime = value; }
        }



    }


    public partial class UserCar : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string BrandField;

        private string ColorField;
        private int ColorIDField;

        private int BrandIDField;

        private string ComfortOptionsField;

        private string GovNumberField;

        private string ModelField;

        private int ModelIDField;

        private int PlacesField;

        public string Brand
        {
            get
            {
                return this.BrandField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BrandField, value) != true))
                {
                    this.BrandField = value;
                    this.RaisePropertyChanged("Brand");
                }
            }
        }

        public string Color
        {
            get
            {
                return this.ColorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ColorField, value) != true))
                {
                    this.ColorField = value;
                    this.RaisePropertyChanged("Color");
                }
            }
        }

        public int ColorID
        {
            get
            {
                return this.ColorIDField;
            }
            set
            {
                if ((this.ColorIDField.Equals(value) != true))
                {
                    this.ColorIDField = value;
                    this.RaisePropertyChanged("ColorID");
                }
            }
        }
        public int BrandID
        {
            get
            {
                return this.BrandIDField;
            }
            set
            {
                if ((this.BrandIDField.Equals(value) != true))
                {
                    this.BrandIDField = value;
                    this.RaisePropertyChanged("BrandID");
                }
            }
        }

        public string ComfortOptions
        {
            get
            {
                return this.ComfortOptionsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ComfortOptionsField, value) != true))
                {
                    this.ComfortOptionsField = value;
                    this.RaisePropertyChanged("ComfortOptions");
                }
            }
        }

        public string GovNumber
        {
            get
            {
                return this.GovNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GovNumberField, value) != true))
                {
                    this.GovNumberField = value;
                    this.RaisePropertyChanged("GovNumber");
                }
            }
        }

        public string Model
        {
            get
            {
                return this.ModelField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ModelField, value) != true))
                {
                    this.ModelField = value;
                    this.RaisePropertyChanged("Model");
                }
            }
        }

        public int ModelID
        {
            get
            {
                return this.ModelIDField;
            }
            set
            {
                if ((this.ModelIDField.Equals(value) != true))
                {
                    this.ModelIDField = value;
                    this.RaisePropertyChanged("ModelID");
                }
            }
        }

        public int Places
        {
            get
            {
                return this.PlacesField;
            }
            set
            {
                if ((this.PlacesField.Equals(value) != true))
                {
                    this.PlacesField = value;
                    this.RaisePropertyChanged("Places");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class Direction : object, System.ComponentModel.INotifyPropertyChanged
    {
        private Model.WeekActuality ActualityField;

        private int PathIDField;

        private bool IsMonField;

        private bool IsFriField;

        private bool IsSatField;

        private bool IsSunField;

        private bool IsThuField;

        private bool IsToHomeField;

        private bool IsTueField;

        private bool IsWedField;

        private string DirectionNameField;



        private List<Model.OnMapPoint> PointsField;


        public Model.WeekActuality Actuality
        {
            get
            {
                return this.ActualityField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ActualityField, value) != true))
                {
                    this.ActualityField = value;
                    this.RaisePropertyChanged("Actuality");
                }
            }
        }

        public string DirectionName
        {
            get
            {
                return this.DirectionNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DirectionNameField, value) != true))
                {
                    this.DirectionNameField = value;
                    this.RaisePropertyChanged("DirectionName");
                }
            }
        }

        public bool IsFri
        {
            get
            {
                return this.IsFriField;
            }
            set
            {
                if ((this.IsFriField.Equals(value) != true))
                {
                    this.IsFriField = value;
                    this.RaisePropertyChanged("IsFri");
                }
            }
        }

        


        public bool IsMon
        {
            get
            {
                return this.IsMonField;
            }
            set
            {
                if ((this.IsMonField.Equals(value) != true))
                {
                    this.IsMonField = value;
                    this.RaisePropertyChanged("IsMon");
                }
            }
        }


        public bool IsSat
        {
            get
            {
                return this.IsSatField;
            }
            set
            {
                if ((this.IsSatField.Equals(value) != true))
                {
                    this.IsSatField = value;
                    this.RaisePropertyChanged("IsSat");
                }
            }
        }


        public bool IsSun
        {
            get
            {
                return this.IsSunField;
            }
            set
            {
                if ((this.IsSunField.Equals(value) != true))
                {
                    this.IsSunField = value;
                    this.RaisePropertyChanged("IsSun");
                }
            }
        }


        public bool IsThu
        {
            get
            {
                return this.IsThuField;
            }
            set
            {
                if ((this.IsThuField.Equals(value) != true))
                {
                    this.IsThuField = value;
                    this.RaisePropertyChanged("IsThu");
                }
            }
        }


        public bool IsToHome
        {
            get
            {
                return this.IsToHomeField;
            }
            set
            {
                if ((this.IsToHomeField.Equals(value) != true))
                {
                    this.IsToHomeField = value;
                    this.RaisePropertyChanged("IsToHome");
                }
            }
        }


        public bool IsTue
        {
            get
            {
                return this.IsTueField;
            }
            set
            {
                if ((this.IsTueField.Equals(value) != true))
                {
                    this.IsTueField = value;
                    this.RaisePropertyChanged("IsTue");
                }
            }
        }


        public bool IsWed
        {
            get
            {
                return this.IsWedField;
            }
            set
            {
                if ((this.IsWedField.Equals(value) != true))
                {
                    this.IsWedField = value;
                    this.RaisePropertyChanged("IsWed");
                }
            }
        }


        public int PathID
        {
            get
            {
                return this.PathIDField;
            }
            set
            {
                if ((this.PathIDField.Equals(value) != true))
                {
                    this.PathIDField = value;
                    this.RaisePropertyChanged("PathID");
                }
            }
        }


        public List<Model.OnMapPoint> Points
        {
            get
            {
                return this.PointsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PointsField, value) != true))
                {
                    this.PointsField = value;
                    this.RaisePropertyChanged("Points");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public class CompanionsRequest
    {
        double longtitude; double latitude; Boolean isToHome; DateTime date;


        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }


        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public bool IsToHome
        {
            get { return isToHome; }
            set { isToHome = value; }
        }


        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }

    public partial class OnMapPoint : object, System.ComponentModel.INotifyPropertyChanged
    {

        private bool IsHomeField;

        private double LatitudeField;

        private double LongtitudeField;

        private int PointIDField;

        private int UIDField;

        private int PathIDField;


        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PathID
        {
            get
            {
                return this.PathIDField;
            }
            set
            {
                if ((this.PathIDField.Equals(value) != true))
                {
                    this.PathIDField = value;
                    this.RaisePropertyChanged("PathID");
                }
            }
        }


        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsHome
        {
            get
            {
                return this.IsHomeField;
            }
            set
            {
                if ((this.IsHomeField.Equals(value) != true))
                {
                    this.IsHomeField = value;
                    this.RaisePropertyChanged("IsHome");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                if ((this.LatitudeField.Equals(value) != true))
                {
                    this.LatitudeField = value;
                    this.RaisePropertyChanged("Latitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Longtitude
        {
            get
            {
                return this.LongtitudeField;
            }
            set
            {
                if ((this.LongtitudeField.Equals(value) != true))
                {
                    this.LongtitudeField = value;
                    this.RaisePropertyChanged("Longtitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PointID
        {
            get
            {
                return this.PointIDField;
            }
            set
            {
                if ((this.PointIDField.Equals(value) != true))
                {
                    this.PointIDField = value;
                    this.RaisePropertyChanged("PointID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UID
        {
            get
            {
                return this.UIDField;
            }
            set
            {
                if ((this.UIDField.Equals(value) != true))
                {
                    this.UIDField = value;
                    this.RaisePropertyChanged("UID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public partial class WeekActuality : object, System.ComponentModel.INotifyPropertyChanged
    {

        private bool FridayField;

        private bool MondayField;

        private bool SaturdayField;

        private bool SundayField;

        private bool ThursdayField;

        private bool TuesdayField;

        private bool WednesdayField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Friday
        {
            get
            {
                return this.FridayField;
            }
            set
            {
                if ((this.FridayField.Equals(value) != true))
                {
                    this.FridayField = value;
                    this.RaisePropertyChanged("Friday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Monday
        {
            get
            {
                return this.MondayField;
            }
            set
            {
                if ((this.MondayField.Equals(value) != true))
                {
                    this.MondayField = value;
                    this.RaisePropertyChanged("Monday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Saturday
        {
            get
            {
                return this.SaturdayField;
            }
            set
            {
                if ((this.SaturdayField.Equals(value) != true))
                {
                    this.SaturdayField = value;
                    this.RaisePropertyChanged("Saturday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Sunday
        {
            get
            {
                return this.SundayField;
            }
            set
            {
                if ((this.SundayField.Equals(value) != true))
                {
                    this.SundayField = value;
                    this.RaisePropertyChanged("Sunday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Thursday
        {
            get
            {
                return this.ThursdayField;
            }
            set
            {
                if ((this.ThursdayField.Equals(value) != true))
                {
                    this.ThursdayField = value;
                    this.RaisePropertyChanged("Thursday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Tuesday
        {
            get
            {
                return this.TuesdayField;
            }
            set
            {
                if ((this.TuesdayField.Equals(value) != true))
                {
                    this.TuesdayField = value;
                    this.RaisePropertyChanged("Tuesday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Wednesday
        {
            get
            {
                return this.WednesdayField;
            }
            set
            {
                if ((this.WednesdayField.Equals(value) != true))
                {
                    this.WednesdayField = value;
                    this.RaisePropertyChanged("Wednesday");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class UserProfile : object, INotifyPropertyChanged
    {

        private string ExtensionField;

        private string FirstNameField;

        private bool IsDriverField;

        private string LastNameField;

        private int OfficeIDField;

        private int PaymentField;

        private string PhoneField;

        private decimal RatingField;

        private int VersionField;


        public string Extension
        {
            get
            {
                return this.ExtensionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ExtensionField, value) != true))
                {
                    this.ExtensionField = value;
                    this.RaisePropertyChanged("Extension");
                }
            }
        }


        public string FirstName
        {
            get
            {
                return this.FirstNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true))
                {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }


        public bool IsDriver
        {
            get
            {
                return this.IsDriverField;
            }
            set
            {
                if ((this.IsDriverField.Equals(value) != true))
                {
                    this.IsDriverField = value;
                    this.RaisePropertyChanged("IsDriver");
                }
            }
        }


        public string LastName
        {
            get
            {
                return this.LastNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LastNameField, value) != true))
                {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }


        public int OfficeID
        {
            get
            {
                return this.OfficeIDField;
            }
            set
            {
                if ((this.OfficeIDField.Equals(value) != true))
                {
                    this.OfficeIDField = value;
                    this.RaisePropertyChanged("OfficeID");
                }
            }
        }


        public int Payment
        {
            get
            {
                return this.PaymentField;
            }
            set
            {
                if ((this.PaymentField.Equals(value) != true))
                {
                    this.PaymentField = value;
                    this.RaisePropertyChanged("Payment");
                }
            }
        }


        public string Phone
        {
            get
            {
                return this.PhoneField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PhoneField, value) != true))
                {
                    this.PhoneField = value;
                    this.RaisePropertyChanged("Phone");
                }
            }
        }


        public decimal Rating
        {
            get
            {
                return this.RatingField;
            }
            set
            {
                if ((this.RatingField.Equals(value) != true))
                {
                    this.RatingField = value;
                    this.RaisePropertyChanged("Rating");
                }
            }
        }


        public int Version
        {
            get
            {
                return this.VersionField;
            }
            set
            {
                if ((this.VersionField.Equals(value) != true))
                {
                    this.VersionField = value;
                    this.RaisePropertyChanged("Version");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class UserOrganization : object, System.ComponentModel.INotifyPropertyChanged
    {

        private bool IsOrganizationField;

        private bool IsVisibleField;

        private string NameField;

        private int TeamIDField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsOrganization
        {
            get
            {
                return this.IsOrganizationField;
            }
            set
            {
                if ((this.IsOrganizationField.Equals(value) != true))
                {
                    this.IsOrganizationField = value;
                    this.RaisePropertyChanged("IsOrganization");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsVisible
        {
            get
            {
                return this.IsVisibleField;
            }
            set
            {
                if ((this.IsVisibleField.Equals(value) != true))
                {
                    this.IsVisibleField = value;
                    this.RaisePropertyChanged("IsVisible");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NameField, value) != true))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TeamID
        {
            get
            {
                return this.TeamIDField;
            }
            set
            {
                if ((this.TeamIDField.Equals(value) != true))
                {
                    this.TeamIDField = value;
                    this.RaisePropertyChanged("TeamID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class OfficeOnMap : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int IDField;

        private double LatitudeField;

        private double LongtitudeField;

        private string NameField;

        private int OrganizationIDField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID
        {
            get
            {
                return this.IDField;
            }
            set
            {
                if ((this.IDField.Equals(value) != true))
                {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                if ((this.LatitudeField.Equals(value) != true))
                {
                    this.LatitudeField = value;
                    this.RaisePropertyChanged("Latitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Longtitude
        {
            get
            {
                return this.LongtitudeField;
            }
            set
            {
                if ((this.LongtitudeField.Equals(value) != true))
                {
                    this.LongtitudeField = value;
                    this.RaisePropertyChanged("Longtitude");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NameField, value) != true))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OrganizationID
        {
            get
            {
                return this.OrganizationIDField;
            }
            set
            {
                if ((this.OrganizationIDField.Equals(value) != true))
                {
                    this.OrganizationIDField = value;
                    this.RaisePropertyChanged("OrganizationID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class WeeklySchedule : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int ScheduleIDField;
        private bool IsEnabledField;

        private Model.DaySchedule FridayField;

        private Model.DaySchedule MondayField;

        private Model.DaySchedule SaturdayField;

        private Model.DaySchedule SundayField;

        private Model.DaySchedule ThursdayField;

        private Model.DaySchedule TuesdayField;

        private Model.DaySchedule WednesdayField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ScheduleID
        {
            get
            {
                return this.ScheduleIDField;
            }
            set
            {
                if ((this.ScheduleIDField.Equals(value) != true))
                {
                    this.ScheduleIDField = value;
                    this.RaisePropertyChanged("ScheduleID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsEnabled
        {
            get
            {
                return this.IsEnabledField;
            }
            set
            {
                if ((this.IsEnabledField.Equals(value) != true))
                {
                    this.IsEnabledField = value;
                    this.RaisePropertyChanged("IsEnabled");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Friday
        {
            get
            {
                return this.FridayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FridayField, value) != true))
                {
                    this.FridayField = value;
                    this.RaisePropertyChanged("Friday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Monday
        {
            get
            {
                return this.MondayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MondayField, value) != true))
                {
                    this.MondayField = value;
                    this.RaisePropertyChanged("Monday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Saturday
        {
            get
            {
                return this.SaturdayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SaturdayField, value) != true))
                {
                    this.SaturdayField = value;
                    this.RaisePropertyChanged("Saturday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Sunday
        {
            get
            {
                return this.SundayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SundayField, value) != true))
                {
                    this.SundayField = value;
                    this.RaisePropertyChanged("Sunday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Thursday
        {
            get
            {
                return this.ThursdayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ThursdayField, value) != true))
                {
                    this.ThursdayField = value;
                    this.RaisePropertyChanged("Thursday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Tuesday
        {
            get
            {
                return this.TuesdayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TuesdayField, value) != true))
                {
                    this.TuesdayField = value;
                    this.RaisePropertyChanged("Tuesday");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Model.DaySchedule Wednesday
        {
            get
            {
                return this.WednesdayField;
            }
            set
            {
                if ((object.ReferenceEquals(this.WednesdayField, value) != true))
                {
                    this.WednesdayField = value;
                    this.RaisePropertyChanged("Wednesday");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class DaySchedule : object, System.ComponentModel.INotifyPropertyChanged
    {

        private bool IsEnabledField;

        private TimeSpan tohome;

        private TimeSpan towork;

        [System.Runtime.Serialization.DataMember()]
        public bool IsEnabled
        {
            get
            {
                return this.IsEnabledField;
            }
            set
            {
                if ((this.IsEnabledField.Equals(value) != true))
                {
                    this.IsEnabledField = value;
                    this.RaisePropertyChanged("IsEnabled");
                }
            }
        }
        
        public TimeSpan ToHomeHlp
        {
            get
            {
                return this.tohome;
            }
            set
            {
                if ((this.tohome.Equals(value) != true))
                {
                    this.tohome = value;
                    this.RaisePropertyChanged("ToHome");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ToHome
        {
            get
            {
                return this.tohome.Ticks;
            }
            set
            {
                if ((this.tohome.Equals(value) != true))
                {
                    this.tohome = new TimeSpan(value);
                    this.RaisePropertyChanged("ToHome");
                    this.RaisePropertyChanged("ToHomeHlp");
                }
            }
        }

        public TimeSpan ToWorkHlp
        {
            get
            {
                return towork;
            }
            set
            {
                if ((this.towork.Equals(value) != true))
                {
                    this.towork = value; 
                    this.RaisePropertyChanged("ToWork");
                }
            }
        }
       

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ToWork
        {
            get
            {
                return towork.Ticks;
            }
            set
            {
                if ((this.towork.Equals(value) != true))
                {
                    this.towork = new TimeSpan(value);
                    this.RaisePropertyChanged("ToWork");
                    this.RaisePropertyChanged("ToWorkHlp");
                }
            }
        }
        

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class UserCompanion : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string BrandField;

        private string ComfortField;

        private string EmailField;

        private string FirstNameField;

        private string GovNumberField;

        private string LastNameField;

        private string ModelField;

        private int PaymentField;

        private string PhoneField;

        private int PlacesField;

        private decimal RatingField;

        private int UIDField;

        private TimeSpan tohome;

        private TimeSpan towork;

        public TimeSpan ToHomeHlp
        {
            get
            {
                return this.tohome;
            }
            set
            {
                if ((this.tohome.Equals(value) != true))
                {
                    this.tohome = value;
                    this.RaisePropertyChanged("ToHome");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ToHome
        {
            get
            {
                return this.tohome.Ticks;
            }
            set
            {
                if ((this.tohome.Equals(value) != true))
                {
                    this.tohome = new TimeSpan(value);
                    this.RaisePropertyChanged("ToHome");
                    this.RaisePropertyChanged("ToHomeHlp");
                }
            }
        }

        public TimeSpan ToWorkHlp
        {
            get
            {
                return towork;
            }
            set
            {
                if ((this.towork.Equals(value) != true))
                {
                    this.towork = value;
                    this.RaisePropertyChanged("ToWork");
                }
            }
        }


        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ToWork
        {
            get
            {
                return towork.Ticks;
            }
            set
            {
                if ((this.towork.Equals(value) != true))
                {
                    this.towork = new TimeSpan(value);
                    this.RaisePropertyChanged("ToWork");
                    this.RaisePropertyChanged("ToWorkHlp");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Brand
        {
            get
            {
                return this.BrandField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BrandField, value) != true))
                {
                    this.BrandField = value;
                    this.RaisePropertyChanged("Brand");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Comfort
        {
            get
            {
                return this.ComfortField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ComfortField, value) != true))
                {
                    this.ComfortField = value;
                    this.RaisePropertyChanged("Comfort");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email
        {
            get
            {
                return this.EmailField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EmailField, value) != true))
                {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName
        {
            get
            {
                return this.FirstNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true))
                {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GovNumber
        {
            get
            {
                return this.GovNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GovNumberField, value) != true))
                {
                    this.GovNumberField = value;
                    this.RaisePropertyChanged("GovNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName
        {
            get
            {
                return this.LastNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LastNameField, value) != true))
                {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Model
        {
            get
            {
                return this.ModelField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ModelField, value) != true))
                {
                    this.ModelField = value;
                    this.RaisePropertyChanged("Model");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Payment
        {
            get
            {
                return this.PaymentField;
            }
            set
            {
                if ((this.PaymentField.Equals(value) != true))
                {
                    this.PaymentField = value;
                    this.RaisePropertyChanged("Payment");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Phone
        {
            get
            {
                return this.PhoneField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PhoneField, value) != true))
                {
                    this.PhoneField = value;
                    this.RaisePropertyChanged("Phone");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Places
        {
            get
            {
                return this.PlacesField;
            }
            set
            {
                if ((this.PlacesField.Equals(value) != true))
                {
                    this.PlacesField = value;
                    this.RaisePropertyChanged("Places");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Rating
        {
            get
            {
                return this.RatingField;
            }
            set
            {
                if ((this.RatingField.Equals(value) != true))
                {
                    this.RatingField = value;
                    this.RaisePropertyChanged("Rating");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UID
        {
            get
            {
                return this.UIDField;
            }
            set
            {
                if ((this.UIDField.Equals(value) != true))
                {
                    this.UIDField = value;
                    this.RaisePropertyChanged("UID");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
