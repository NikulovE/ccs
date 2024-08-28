
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using CCS.Android.Native;
using Firebase.Iid;

namespace Shared.ViewModel
{
    class Registration
    {

        public static async void CheckEmail(EditText InputedMail, Button SendKey, ProgressBar progressBar, TextView OutPut) { 
        SendKey.Enabled = false;
        var response = new Tuple<bool, string>(false, "");
            try
            {
                Shared.View.General.inLoading(progressBar, OutPut);
                 response = await Model.Requests.StartRegistration(InputedMail.Text);

                if (response.Item1 == true)
                {                    
                    Shared.Model.LocalStorage.RegistrationAESKey = response.Item2;
                    Model.LocalStorage.ProfileVersion = 1;
                    Shared.Actions.showConfirmationGrid();
                }
                else
                {
                    OutPut.Text = ConvertMessages.Message(response.Item2);
                    if (response.Item2 == "x10003")
                    {
                        response = await Model.Requests.RestoreRegistration(InputedMail.Text);
                        if (response.Item1 == true)
                        {
                            Model.LocalStorage.ProfileVersion = 1;
                            Shared.Model.LocalStorage.RegistrationAESKey = response.Item2;
                            Shared.Actions.showConfirmationGrid();
                        }
                        else
                        {
                            Model.LocalStorage.ProfileVersion = 0;
                            OutPut.Text = ConvertMessages.Message(response.Item2);
                            View.General.EnableButton(SendKey);
                        }
                    }
                    
                    View.General.EnableButton(SendKey);
                }
                Shared.View.General.outLoading(progressBar);
            }
            catch (Exception)
            {
                View.General.EnableButton(SendKey);
                Model.LocalStorage.ProfileVersion = 0;
                Shared.View.General.outLoading(progressBar);
                OutPut.Text = ConvertMessages.Message("x50010");
            }
        }



        public static async void Confirm(Button ConfirmPassword, EditText PasswordFromMail, ProgressBar progressBar, TextView OutPut)
        {
            View.General.DisableButton(ConfirmPassword);
            try
            {
                var Password = PasswordFromMail.Text.Replace(" ", "");
                try
                {
                    var AesEngine = new Shared.Crypting.AES();
                    var OpenedKey = "";
                    try
                    {
                        OpenedKey = AesEngine.Decrypt(Shared.Model.LocalStorage.RegistrationAESKey, Password);
                    }
                    catch (Exception)
                    {
                        OutPut.Text = ConvertMessages.Message("x50003");
                        OpenedKey = "";
                    }
                    if (OpenedKey != "")
                    {
                        var RegData = OpenedKey.Split(',');
                        var UID = int.Parse(RegData[0]);
                        var RSAPub = RegData[1];
                        try
                        {
                            var RSAEngine = new Shared.Crypting.RSA();
                            var Answer = RSAEngine.Encrypt(Password, RSAPub);
                            Shared.View.General.inLoading(progressBar, OutPut);
                            var Response = await Model.Requests.ConfirmRegistration(UID, Answer);
                            Shared.View.General.outLoading(progressBar);
                            if (Response.Item1 == true)
                            {
                                Shared.Model.LocalStorage.ServerRSAKey = RSAPub;
                                Shared.Model.LocalStorage.UID = UID;
                                var ServerSign = AesEngine.Decrypt(Response.Item2, Password);
                                Shared.Model.LocalStorage.SessionKey = ServerSign.Split(',')[1];
                                Shared.Model.LocalStorage.SessionID = int.Parse(ServerSign.Split(',')[0]);
                                Shared.Model.LocalStorage.ProfileVersion = 2;


                                RefreshToken();
                                Shared.Actions.showFillingProfileGrid();
                            }
                            else
                            {
                                OutPut.Text = ConvertMessages.Message(Response.Item2);
                            }
                        }
                        catch (Exception)
                        {
                            OutPut.Text = ConvertMessages.Message("x50004");
                        }
                    }
                }
                catch (Exception)
                {
                    OutPut.Text = ConvertMessages.Message("x50003");


                }

            }
            catch (Exception)
            {
                OutPut.Text = ConvertMessages.Message("x50002");
            }
            View.General.EnableButton(ConfirmPassword);
        }


        public static void RefreshToken() {
            //Shared.Actions.RegisterInAzureHub();
        }

        public static async Task<bool> RestoreAccess(ProgressBar progressBar, TextView OutPut)
        {


            var AESEngine = new Shared.Crypting.AES();

            var ClientRSAPublic = Shared.Model.LocalStorage.ServerRSAKey;
            var FirstPart = AESEngine.Encrypt(Shared.Model.LocalStorage.UID.ToString(), ClientRSAPublic);

            var ClientRSAPair = Shared.Crypting.RSA.CreateKeyPair();

            var ClientAESSign = AESEngine.Encrypt(ClientRSAPair.Item2, FirstPart);

            Shared.View.General.inLoading(progressBar, OutPut);
            var newSign = await Model.Requests.RestoreSessionKey(ClientAESSign);
            Shared.View.General.outLoading(progressBar);
            if (newSign.Item1 == true)
            {
                var RSAEngine = new Shared.Crypting.RSA();
                var ServerResponse = RSAEngine.Decrypt(newSign.Item2, ClientRSAPair.Item1);
                Shared.Model.LocalStorage.SessionKey = ServerResponse.Split(',')[1];
                Shared.Model.LocalStorage.SessionID = int.Parse(ServerResponse.Split(',')[0]);
                return true;
            }
            else
            {
                return false;
            }
        }

        }
}
