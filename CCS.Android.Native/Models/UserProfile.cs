using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Shared.ModelView
{
    public class UserProfile : INotifyPropertyChanged
    {

        private static Model.UserProfile defaultInstance = new Model.UserProfile();
        public static Model.UserProfile Default
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
