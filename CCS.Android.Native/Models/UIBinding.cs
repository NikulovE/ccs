using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
#if NETFX_CORE
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
#if WINDOWS_UWP
#else
using Bing.Maps;
#endif

#else
#if WPF
using Microsoft.Maps.MapControl.WPF;
#endif
#endif
#if WPF
#else
using Shared.Model;
#endif

namespace Shared.ModelView
{
    class UIBinding : INotifyPropertyChanged
    {
        private static UIBinding defaultInstance = new UIBinding { isSearchCompanionsToHome = true, selectedPath = 1 };
        public static UIBinding Default
        {
            get
            {
                return defaultInstance;
            }

            set
            {
                defaultInstance = value;
            }
        }

        private ObservableCollection<Model.Conversation> messages = new ObservableCollection<Model.Conversation>();
        private List<Model.TripOffer> tripOffers = new List<Model.TripOffer>();
        private List<Model.Direction> routes = new List<Model.Direction>();
        private List<Model.OfficeOnMap> offices = new List<Model.OfficeOnMap>();

        private Boolean isChangedProfile = false;
        private Boolean isSearchingMode;

        private String outPutString = "";
        private String TeamsoutPutString;


        private int selectedOffice = -1;

        private int selectedPath = 1;

        private Location homeLocation = new Model.Location(0, 0);
        private Location currentCenter = new Model.Location(0, 0);

        private Location currentUserPosition = new Model.Location(0, 0);

        private Location officeOnMap = new Model.Location(0, 0);
        private Boolean IsOnLoading;

        private String carTripMode = Shared.ConvertMessages.Message("CarAndTrip");

        private String assistant = "";
        private bool driverRoutesVisibility = true;
        private bool autoJoinButton = false;



        private List<String> minutes = new List<String> { "00", "10", "20", "30", "40", "50" };
        private List<String> hours = new List<String> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };


        public String Assistant
        {
            get
            {
                return this.assistant;
            }
            set
            {
                this.assistant = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedPath
        {
            get
            {
                return this.selectedPath;
            }
            set
            {
                this.selectedPath = value;
                NotifyPropertyChanged();
            }
        }


        public List<Model.OfficeOnMap> Officies
        {
            get
            {
                return this.offices;
            }
            set
            {
                this.offices = value;
                NotifyPropertyChanged();
            }
        }


        public List<Model.TripOffer> TripOffers
        {
            get
            {
                return this.tripOffers;
            }
            set
            {
                this.tripOffers = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Model.Conversation> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                NotifyPropertyChanged();
            }
        }

        public List<Model.Direction> Routes
        {
            get
            {
                return this.routes;
            }
            set
            {
                this.routes = value;
                NotifyPropertyChanged();
            }
        }

        public List<String> Minutes
        {
            get
            {
                return this.minutes;
            }
            set
            {
                this.minutes = value;
                NotifyPropertyChanged();
            }
        }

        public List<String> Hours
        {
            get
            {
                return this.hours;
            }
            set
            {
                this.hours = value;
                NotifyPropertyChanged();
            }
        }

        public Boolean isSearchCompanionsToHome
        {
            get
            {
                return this.isSearchingMode;
            }
            set
            {
                this.isSearchingMode = value;
                NotifyPropertyChanged();
            }
        }

        public String CarTripMode
        {
            get
            {
                return this.carTripMode;
            }
            set
            {
                this.carTripMode = value;
                NotifyPropertyChanged();
            }
        }
        public bool DriverRoutesVisibility


        {
            get
            {
                return this.driverRoutesVisibility;
            }
            set
            {
                this.driverRoutesVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool AutoJoinVisibility

        {
            get
            {
                return this.autoJoinButton;
            }
            set
            {
                this.autoJoinButton = value;
                NotifyPropertyChanged();
            }
        }


        public Location OfficeOnMap
        {
            get
            {
                return this.officeOnMap;
            }
            set
            {
                this.officeOnMap = value;
                NotifyPropertyChanged();
            }
        }

        public Location HomeLocation
        {
            get
            {
                return this.homeLocation;
            }
            set
            {
                this.homeLocation = value;
                NotifyPropertyChanged();
            }
        }

        public Location CurrentUserPosition
        {
            get
            {
                return this.currentUserPosition;
            }
            set
            {
                this.currentUserPosition = value;
                NotifyPropertyChanged();
            }
        }

        public Location CurrentCenter
        {
            get
            {
                return this.currentCenter;
            }
            set
            {
                this.currentCenter = value;
                NotifyPropertyChanged();
            }
        }

        public Boolean isProfileChanged
        {
            get
            {
                return this.isChangedProfile;
            }
            set
            {
                this.isChangedProfile = value;
                NotifyPropertyChanged();
            }
        }

        public Boolean isOnLoading
        {
            get
            {
                return this.IsOnLoading;
            }
            set
            {
                this.IsOnLoading = value;
                NotifyPropertyChanged();
            }
        }


        public int SelectedOffice
        {
            get
            {
                return this.selectedOffice;
            }
            set
            {
                this.selectedOffice = value;
                NotifyPropertyChanged();
            }
        }
        //public String OutPut
        //{
        //    get
        //    {
        //        return this.outPutString;
        //    }
        //    set
        //    {
        //        this.outPutString = value;
        //        NotifyPropertyChanged();
        //    }
        //}


        public String TeamsOutPut
        {
            get
            {
                return this.TeamsoutPutString;
            }
            set
            {
                this.TeamsoutPutString = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                try
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception) { }
            }

        }
    }
}
