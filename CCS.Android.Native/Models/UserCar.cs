using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shared.ModelView
{
    public class UserCar : INotifyPropertyChanged
    {
        private static Model.UserCar defaultInstance = new Model.UserCar();

        public static Model.UserCar Default
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
