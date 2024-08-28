


using Shared.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


#if NETFX_CORE
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif

using Windows.UI.Xaml;
#else
#if WPF
using Microsoft.Maps.MapControl.WPF;
#endif
using System.Windows;
#if XAMARIN
using Xamarin.Forms.Maps;
#endif
#endif


namespace Shared.ViewModel
{
    class Organization
    {
        public static async Task<bool> Registration(String WorkEmail, String PrefferedName)
        {

            Shared.View.General.inLoading();
            var newSign = await Model.Requests.StartCompanyRegisration(WorkEmail, PrefferedName);
            Shared.View.General.outLoading();
            if (newSign.Item1 == true)
            {
                //Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(newSign.Item2);
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(newSign.Item2);
                return false;
            }


        }

        public static async Task<bool> Confirmation(String DomainWorkEmail, String Password)
        {

            Shared.View.General.inLoading();
            var newSign = await Model.Requests.ConfirmCompanyRegistration(Password, DomainWorkEmail);
            Shared.View.General.outLoading();
            if (newSign.Item1 == true)
            {

                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(newSign.Item2);
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(newSign.Item2);
                return false;
            }

        }

        public static async Task<bool> LoadOrganizations()
        {

            Shared.View.General.inLoading();
            var listOfGroups = await Model.Requests.LoadOrganizations();
            Shared.View.General.outLoading();

            if (listOfGroups.Item1 == true)
            {
                View.Organization.ShowListOfOrganizations(listOfGroups.Item2);
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                return false;
            }
        }

        public static async void LeaveOrganization()
        {

            Shared.View.General.inLoading();
#if XAMARIN
            var OrgID = 0;
#else
            var OrgID = ModelView.UserOrganizations.Default.SelectedOrganization;
#endif


            var Request = await Model.Requests.LeaveOrganization(OrgID);
            if (Request.Item1 == true)
            {

                var listOfGroups = await Model.Requests.LoadOrganizations();

                Shared.View.General.outLoading();
                if (listOfGroups.Item1 == true)
                {
#if XAMARIN
                    View.Organization.ShowListOfOrganizations(listOfGroups.Item2);
#endif
#if WPF
                    View.Organization.ShowListOfOrganizations(listOfGroups.Item2);
#endif
                    Shared.Actions.refreshOffices();
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                }
            }
        }


#if WINDOWS_APP || WPF
        public static async Task<bool> LoadOffices(MapLayer GlobalMap,Boolean OnlyUserOffice = false) {
            GlobalMap.Visibility = Visibility.Visible;
#else
        public static async Task<bool> LoadOffices(Boolean OnlyUserOffice = false){
#endif


            Shared.View.General.inLoading();
#if NETFX_CORE
            if (OnlyUserOffice)
            {

                var listOfGroups = await Model.Requests.LoadUserOffice();
                Shared.View.General.outLoading();
                if (listOfGroups.Item1 == true)
                {
                    Shared.ModelView.UserOrganizations.Default.CurrentOffice = listOfGroups.Item2;
                    return true;
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                    return false;
                }
            }
            else
            {
                var listOfGroups = await Model.Requests.LoadOffices();
                Shared.View.General.outLoading();
                if (listOfGroups.Item1 == true)
                {
#if WINDOWS_APP || WPF
                View.Organization.ShowListOfOffices(listOfGroups.Item2, GlobalMap, OnlyUserOffice);
#else
                    View.Organization.ShowListOfOffices(listOfGroups.Item2, OnlyUserOffice);
#endif
                    return true;
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                    return false;
                }
            }


#else
            var listOfGroups = await Model.Requests.LoadOffices();
            Shared.View.General.outLoading();
            if (listOfGroups.Item1 == true)
            {
#if WINDOWS_APP || WPF
                View.Organization.ShowListOfOffices(listOfGroups.Item2, GlobalMap, OnlyUserOffice);
#else
                    View.Organization.ShowListOfOffices(listOfGroups.Item2, OnlyUserOffice);
#endif
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50000");
                return false;
            }
            
#endif




        }

        public static async void ChangeVisibility(int ID, bool isVisible, bool isOrganization)
        {

            Shared.View.General.inLoading();
            var changeGroup = await Model.Requests.ChangeGroupVisibility(ID, isVisible, isOrganization);
            Shared.View.General.outLoading();

            if (changeGroup.Item1 == true)
            {

            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(changeGroup.Item2);
            }


        }

        public static async Task<bool> StartJoin(String WorkEmail)
        {
            Shared.View.General.inLoading();
            var joinerflow = await Model.Requests.StartJoinToOrganization(WorkEmail);
            Shared.View.General.outLoading();

            if (joinerflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return true;

            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return false;

            }
        }

        public static async Task<bool> AutoJoin(String WorkEmail)
        {
            Shared.View.General.inLoading();
            var RSAEngine = new Shared.Crypting.RSA();

            var RSAPair = Shared.Crypting.RSA.CreateKeyPair();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localserverru.testrussia.local/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("api/default/" + Shared.Model.LocalStorage.SessionID.ToString());
            string json = null;

            json = await response.Content.ReadAsStringAsync();
            var LocalKeyValue=JsonEngine.Deserialize<String>(json);    

            var joinerflow = await Model.Requests.AutoJoinToOrganization(LocalKeyValue.ToString(), RSAPair.Item2);
            Shared.View.General.outLoading();

            if (joinerflow.Item1 == true)
            {
                var Password = RSAEngine.Decrypt(joinerflow.Item2, RSAPair.Item1);
                Shared.View.General.inLoading();
                joinerflow = await Model.Requests.ConfirmJoinToOrganization(Password, WorkEmail.Split('@')[1]);
                Shared.View.General.outLoading();
                if (joinerflow.Item1 == true)
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                    Shared.Actions.refreshOffices();
                    Shared.Actions.refreshOrganizations();
                    return true;
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                    return false;
                }

            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return false;

            }

        }



        public static async Task<bool> CompleteJoiner(String DomainWorkEmail, String Password)
        {
            Shared.View.General.inLoading();
            var joinerflow = await Model.Requests.ConfirmJoinToOrganization(Password, DomainWorkEmail);
            Shared.View.General.outLoading();
            if (joinerflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                try
                {
                    Shared.Actions.refreshOffices();
                    Shared.Actions.refreshOrganizations();
                }
                catch (Exception) { }
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }

        public static async Task<bool> CreateOffice(int TeamID, string name, double longtitude, double latitude)
        {
            Shared.View.General.inLoading();
            var joinerflow = await Model.Requests.CreateOffice(TeamID, name, longtitude, latitude);
            Shared.View.General.outLoading();
            if (joinerflow.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);

                return true;
            }
            else
            {

                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }
    }
}
