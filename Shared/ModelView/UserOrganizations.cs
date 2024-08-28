
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;


#if NETFX_CORE
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif

using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Shared.Model;
#else


#if WPF
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.ObjectModel;
#endif
#if XAMARIN
using System.Collections.ObjectModel;
#endif
#endif
namespace Shared.ModelView
{
    class UserOrganizations : INotifyPropertyChanged
    {
        private static UserOrganizations defaultInstance = new UserOrganizations();
        public static UserOrganizations Default
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
#if XAMARIN
        private bool fullAcitivities = false;
        ObservableCollection<Shared.Model.UserGroup> organizationsList = new ObservableCollection<Shared.Model.UserGroup>();
        ObservableCollection<String> officeList = new ObservableCollection<String>();
        public List<Model.OfficeOnMap> officelst = new List<Model.OfficeOnMap>();

        public ObservableCollection<Shared.Model.UserGroup> OrganizationsList
        {
            get
            {
                return this.organizationsList;
            }
            set
            {
                this.organizationsList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<String> OfficeList
        {
            get
            {
                return this.officeList;
            }
            set
            {
                this.officeList = value;
                NotifyPropertyChanged();
            }
        }

#else
        private ObservableCollection<ListViewItem> organizationsList = new ObservableCollection<ListViewItem>();
        private ObservableCollection<ComboBoxItem> officeList = new ObservableCollection<ComboBoxItem>();
        private ItemsControl visibilityList = new ItemsControl();
        private int selectedOrganization;
        private Shared.Model.Location clickedlocation;
        private string newOfficeName;
        private Visibility fullAcitivities = Visibility.Collapsed;

        private Model.OfficeOnMap userOffice = new Model.OfficeOnMap();

        public Model.OfficeOnMap CurrentOffice
        {
            get
            {
                return this.userOffice;
            }
            set
            {
                this.userOffice = value;
                NotifyPropertyChanged();
            }
        }
        public string NewOfficeName
        {
            get
            {
                return this.newOfficeName;
            }
            set
            {
                this.newOfficeName = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ListViewItem> OrganizationsList
        {
            get
            {
                return this.organizationsList;
            }
            set
            {
                this.organizationsList = value;
                NotifyPropertyChanged();
            }
        }

        public Shared.Model.Location ClickedLocation
        {
            get
            {
                return this.clickedlocation;
            }
            set
            {
                this.clickedlocation = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<ComboBoxItem> OfficeList
        {
            get
            {
                return this.officeList;
            }
            set
            {
                this.officeList = value;
                NotifyPropertyChanged();
            }
        }

        public ItemsControl VisibilityList
        {
            get
            {
                return this.visibilityList;
            }
            set
            {
                this.visibilityList = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedOrganization
        {
            get
            {
                return this.selectedOrganization;
            }
            set
            {
                this.selectedOrganization = value;
                NotifyPropertyChanged();
            }
        }
        public Visibility FullAcitivities
        {
            get
            {
                return this.fullAcitivities;
            }
            set
            {
                this.fullAcitivities = value;
                NotifyPropertyChanged();
            }
        }
        
#endif
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }

    }
}
