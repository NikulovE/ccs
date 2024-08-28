using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CCS.Web.CSHTML
{
    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            var mainPage = new Registration();
            //CCS.Web.CSHTML.Properties.Settings.Default
            //ApplicationData.Current.RoamingSettings;
            Window.Current.Content = mainPage;
        }
    }
}
