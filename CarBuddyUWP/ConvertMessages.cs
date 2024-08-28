using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace CarBuddyUWP
{
    class ConvertMessages
    {
        public static String Message(String Code)
        {
            ResourceLoader loader = new ResourceLoader();
            return loader.GetString(Code);
           
        }
    }
}
