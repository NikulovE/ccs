using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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

namespace CCS.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RegisterTest(object sender, RoutedEventArgs e)
        {
           // await Shared.Model.Requests.Register(usermailbox.Text);
        }

        //private async void RegisterTest(object sender, RoutedEventArgs e)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:59524/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var newuser = new model();
        //    newuser.Email = "test@list.ru";
        //    newuser.Password = "GeForceFX0 ";
        //    newuser.PasswordConfirm = "GeForceFX0 ";
        //    newuser.Year = "21";
        //    var jsonx = Serialize(newuser);
        //    var content = new StringContent(Serialize(newuser), Encoding.UTF8, "application/json");
        //    var s = content.ToString();
        //    HttpResponseMessage response = await client.PostAsync("api/Account/Register", content);
        //    string json = null;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        json = await response.Content.ReadAsStringAsync();
        //        //var o= Deserialize<Tuple<bool, String>>(json);
        //    }
        //    else
        //    {
        //        //return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}


        public class model
        {
            public string Email = "";
            public string Password = "";
            public string Year = "";
            public string PasswordConfirm = "";
        }

        public class Logi
        {
            public string Email = "";
            public string Password = "";
        }

        public static T Deserialize<T>(string json)
        {
            using (MemoryStream _Stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(T));
                return (T)_Serializer.ReadObject(_Stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                _Stream.Position = 0;
                return (new StreamReader(_Stream)).ReadToEnd();
            }
        }
        CookieContainer cookie = new CookieContainer();
    }
}
