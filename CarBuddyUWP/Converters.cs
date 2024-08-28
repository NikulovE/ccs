using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CarBuddyUWP
{
    public class SelectedOffice : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)//object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((int)value == ModelView.UserProfile.Default.OfficeID)
            //{
            //    return true;
            //}
            //else return false;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)//object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
