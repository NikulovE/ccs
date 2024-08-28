using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shared.ModelView
{
    class UserSchedule : INotifyPropertyChanged
    {
        private static List<Model.WeeklySchedule> defaultInstance = new List<Model.WeeklySchedule>();
        //{
        //    Monday = new Model.DaySchedule(),
        //    Tuesday = new Model.DaySchedule(),
        //    Wednesday = new Model.DaySchedule(),
        //    Thursday = new Model.DaySchedule(),
        //    Friday = new Model.DaySchedule(),
        //    Saturday = new Model.DaySchedule(),
        //    Sunday = new Model.DaySchedule()
        //};
        public static List<Model.WeeklySchedule> Default
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
