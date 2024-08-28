
using Shared.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

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
        private int selectedOrganization;

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

        private List<UserOrganization> organizationsList = new List<UserOrganization>();
        private OfficeOnMap currentOffice;

        public OfficeOnMap CurrentOffice
        {
            get
            {
                return this.currentOffice;
            }
            set
            {
                this.currentOffice = value;
                NotifyPropertyChanged();
            }
        }

        public List<UserOrganization> OrganizationsList
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
        private List<OfficeOnMap> offices = new List<OfficeOnMap>();
        public List<OfficeOnMap> Offices {
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
