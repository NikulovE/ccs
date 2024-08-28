using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    class ConvertMessages
    {
        public static String Message(String Code)
        {
            if (Code == "Internal error") Code = "x50000";
            var resourceManager = Properties.Resources.ResourceManager;
            return resourceManager.GetString(Code);

        }
    }
}