using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarBuddyServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window//, INotifyPropertyChanged

    {
        public MainWindow()
        {
            InitializeComponent();
            //this.MyDotNetProperty = "Go ahead. Change my value.";
            //TopLevelContainer.DataContext = this;
        }

        //private string m_sValue;
        //public static string sss;
        //public string MyDotNetProperty
        //{
        //    get { return m_sValue; }
        //    set
        //    {
        //        m_sValue = value;
        //        if (null != this.PropertyChanged)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("MyDotNetProperty"));
        //        }
        //    }
        //}

        //#region INotifyPropertyChanged Member
        //public event PropertyChangedEventHandler PropertyChanged;
        //#endregion


        private void GenKey(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.mytext = "dfaf";
            //MyDotNetProperty = "Evgeniy";
        }
    }
}
