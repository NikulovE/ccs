using Android.Widget;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace Shared.ViewModel
{
    class Organization
    {
        public static async Task<bool> Registration(String WorkEmail, String PrefferedName, ProgressBar progressBar, TextView OutPut)
        {

            Shared.View.General.inLoading(progressBar,OutPut);
            var newSign = await Model.Requests.StartCompanyRegisration(WorkEmail, PrefferedName);
            Shared.View.General.outLoading(progressBar);
            if (newSign.Item1 == true)
            {
                //Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(newSign.Item2);
                return true;
            }
            else
            {
                OutPut.Text = ConvertMessages.Message(newSign.Item2);
                return false;
            }


        }

        public static async Task<bool> Confirmation(String DomainWorkEmail, String Password, ProgressBar progressBar, TextView OutPut)
        {

            Shared.View.General.inLoading(progressBar, OutPut);
            var newSign = await Model.Requests.ConfirmCompanyRegistration(Password, DomainWorkEmail);
            Shared.View.General.outLoading(progressBar);
            if (newSign.Item1 == true)
            {

                OutPut.Text = ConvertMessages.Message(newSign.Item2);
                return true;
            }
            else
            {
                OutPut.Text = ConvertMessages.Message(newSign.Item2);
                return false;
            }

        }

        public static async Task<bool> LoadOrganizations(ProgressBar progressBar, TextView OutPut)
        {

            Shared.View.General.inLoading(progressBar, OutPut);
            var listOfGroups = await Model.Requests.LoadOrganizations();
            Shared.View.General.outLoading(progressBar);

            if (listOfGroups.Item1 == true)
            {
                ModelView.UserOrganizations.Default.OrganizationsList = listOfGroups.Item2;
                //View.Organization.ShowListOfOrganizations(listOfGroups.Item2);
                return true;
            }
            else
            {
                OutPut.Text = ConvertMessages.Message("x50000");
                return false;
            }
        }

        public static async void LeaveOrganization(ProgressBar progressBar, TextView OutPut)
        {

            Shared.View.General.inLoading(progressBar, OutPut);
            var OrgID = ModelView.UserOrganizations.Default.SelectedOrganization;


            var Request = await Model.Requests.LeaveOrganization(OrgID);
            if (Request.Item1 == true)
            {

                var listOfGroups = await Model.Requests.LoadOrganizations();

                Shared.View.General.outLoading(progressBar);
                if (listOfGroups.Item1 == true)
                {
                    Shared.Actions.refreshOffices();
                }
                else
                {
                    OutPut.Text = ConvertMessages.Message("x50000");
                }
            }
        }



        public static async Task<bool> LoadOffices(ProgressBar progressBar, TextView OutPut, Boolean OnlyUserOffice = false)
        {


            Shared.View.General.inLoading(progressBar, OutPut);
            if (OnlyUserOffice) {
                var useroffice = await Model.Requests.LoadUserOffice();
                Shared.View.General.outLoading(progressBar);

                if (useroffice.Item1 == true)
                {
                    Shared.ModelView.UserOrganizations.Default.CurrentOffice = useroffice.Item2;
                    return true;
                }
                else
                {
                    OutPut.Text = ConvertMessages.Message("x50000");
                    return false;
                }
            }
            else
            {
                var listOfGroups = await Model.Requests.LoadOffices();
                Shared.View.General.outLoading(progressBar);

                if (listOfGroups.Item1 == true)
                {
                    Shared.ModelView.UserOrganizations.Default.Offices = listOfGroups.Item2;
                    return true;

                }
                else
                {
                    OutPut.Text = ConvertMessages.Message("x50000");
                    return false;
                }
            }


        }

        public static async void ChangeVisibility(int ID, bool isVisible, bool isOrganization, ProgressBar progressBar, TextView OutPut)
        {

            Shared.View.General.inLoading(progressBar, OutPut);
            var changeGroup = await Model.Requests.ChangeGroupVisibility(ID, isVisible, isOrganization);
            Shared.View.General.outLoading(progressBar);

            if (changeGroup.Item1 == true)
            {

            }
            else
            {
                OutPut.Text = ConvertMessages.Message(changeGroup.Item2);
            }


        }

        public static async Task<bool> StartJoin(String WorkEmail, ProgressBar progressBar, TextView OutPut)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var joinerflow = await Model.Requests.StartJoinToOrganization(WorkEmail);
            Shared.View.General.outLoading(progressBar);

            if (joinerflow.Item1 == true)
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return true;

            }
            else
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return false;

            }
        }

        public static async Task<bool> AutoJoin(String WorkEmail, ProgressBar progressBar, TextView OutPut)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
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
            Shared.View.General.outLoading(progressBar);

            if (joinerflow.Item1 == true)
            {
                var Password = RSAEngine.Decrypt(joinerflow.Item2, RSAPair.Item1);
                Shared.View.General.inLoading(progressBar, OutPut);
                joinerflow = await Model.Requests.ConfirmJoinToOrganization(Password, WorkEmail.Split('@')[1]);
                Shared.View.General.outLoading(progressBar);
                if (joinerflow.Item1 == true)
                {
                    OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                    Shared.Actions.refreshOffices();
                    Shared.Actions.refreshOrganizations();
                    return true;
                }
                else
                {
                    OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                    return false;
                }

            }
            else
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return false;

            }

        }



        public static async Task<bool> CompleteJoiner(String DomainWorkEmail, String Password, ProgressBar progressBar, TextView OutPut)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var joinerflow = await Model.Requests.ConfirmJoinToOrganization(Password, DomainWorkEmail);
            Shared.View.General.outLoading(progressBar);
            if (joinerflow.Item1 == true)
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
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
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }

        public static async Task<bool> CreateOffice(int TeamID, string name, double longtitude, double latitude, ProgressBar progressBar, TextView OutPut)
        {
            Shared.View.General.inLoading(progressBar, OutPut);
            var joinerflow = await Model.Requests.CreateOffice(TeamID, name, longtitude, latitude);
            Shared.View.General.outLoading(progressBar);
            if (joinerflow.Item1 == true)
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return true;
            }
            else
            {
                OutPut.Text = ConvertMessages.Message(joinerflow.Item2);
                return false;
            }
        }
    }
}
