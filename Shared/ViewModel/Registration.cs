using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



#if NETFX_CORE
using Windows.UI.Xaml.Controls;
#else
#if WPF
using System.Windows.Controls;
#endif
#endif
#if XAMARIN
using Xamarin.Forms;
#endif

namespace Shared.ViewModel
{
    class Registration
    {


#if NETFX_CORE
        public static async void CheckEmail(TextBox InputedMail, Button SendKey, Grid CurrentGrid, EventHandler<object> ShowConfirmationGrid) {
#endif
#if XAMARIN
        public static async void CheckEmail(Entry InputedMail, Button SendKey, Grid CurrentGrid, Action ShowConfirmationGrid) {
#endif
#if WPF
            public static async void CheckEmail(TextBox InputedMail, Button SendKey, Grid CurrentGrid, EventHandler ShowConfirmationGrid){
#endif
            SendKey.IsEnabled = false;

            var response = new Tuple<bool, string>(false, "");
            try
            {
                Shared.View.General.inLoading();
                 response = await Model.Requests.StartRegistration(InputedMail.Text);

                if (response.Item1 == true)
                {                    
                    Shared.Model.LocalStorage.RegistrationAESKey = response.Item2;
                    Model.LocalStorage.ProfileVersion = 1;
                    Shared.View.General.ShowNextRegistrationGrid(CurrentGrid, ShowConfirmationGrid);

                    /*Model.LocalStorage.ProfileVersion = 1;
                    Shared.Model.LocalStorage.Email = InputedMail.Text;
                    Shared.Model.LocalStorage.UID = int.Parse(response.Item2);
                    Shared.View.General.ShowNextRegistrationGrid(CurrentGrid, ShowConfirmationGrid);*/
                    //Shared.Model.LocalStorage.Email = InputedMail.Text;
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(response.Item2);
                    if (response.Item2 == "x10003")
                    {
                        //SendKey.Visibility = Visibility.Collapsed;
                        response = await Model.Requests.RestoreRegistration(InputedMail.Text);
                        if (response.Item1 == true)
                        {
                            Model.LocalStorage.ProfileVersion = 1;
                            Shared.Model.LocalStorage.RegistrationAESKey = response.Item2;
                            Shared.View.General.ShowNextRegistrationGrid(CurrentGrid, ShowConfirmationGrid);
                        }
                        else
                        {
                            Model.LocalStorage.ProfileVersion = 0;
                            Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(response.Item2);
                            View.General.EnableButton(SendKey);
                        }
                    }
                    
                    View.General.EnableButton(SendKey);
                }
                Shared.View.General.outLoading();
            }
            catch (Exception)
            {
                View.General.EnableButton(SendKey);
                Model.LocalStorage.ProfileVersion = 0;
                Shared.View.General.outLoading();
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50010");
            }
        }



#if NETFX_CORE
        public static async void Confirm(Button ConfirmPassword, PasswordBox PasswordFromMail, Grid CurrentGrid, EventHandler<object> ShowFilliningProfileGrid)
#else
#if XAMARIN
        public static async void Confirm(Button ConfirmPassword, Entry PasswordFromMail, Grid CurrentGrid, Action ShowFilliningProfileGrid)
#else
        public static async void Confirm(Button ConfirmPassword, PasswordBox PasswordFromMail, Grid CurrentGrid, EventHandler ShowFilliningProfileGrid)
#endif

#endif
        {
            View.General.DisableButton(ConfirmPassword);
            try
            {
#if XAMARIN
                var Password = PasswordFromMail.Text.Replace(" ", "");
#else
                var Password = PasswordFromMail.Password.Replace(" ", "");
#endif
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
                        Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50003");
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
                            Shared.View.General.inLoading();
                            var Response = await Model.Requests.ConfirmRegistration(UID, Answer);
                            Shared.View.General.outLoading();
                            if (Response.Item1 == true)
                            {
#if WPF
                                Shared.Model.LocalStorage.ContainerName = "CCS";
                                Shared.Model.LocalStorage.MasterAESKey = Shared.Crypting.GeneratePassword(32);
#endif
                                Shared.Model.LocalStorage.ServerRSAKey = RSAPub;
                                Shared.Model.LocalStorage.UID = UID;
                                var ServerSign = AesEngine.Decrypt(Response.Item2, Password);
                                Shared.Model.LocalStorage.SessionKey = ServerSign.Split(',')[1];
                                Shared.Model.LocalStorage.SessionID = int.Parse(ServerSign.Split(',')[0]);
                                Shared.Model.LocalStorage.ProfileVersion = 2;
                                Shared.View.General.ShowNextRegistrationGrid(CurrentGrid, ShowFilliningProfileGrid);
                            }
                            else
                            {
                                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(Response.Item2);
                            }
                        }
                        catch (Exception)
                        {
                            Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50004");
                        }
                    }
                }
                catch (Exception)
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50003");


                }

            }
            catch (Exception)
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50002");
            }
            View.General.EnableButton(ConfirmPassword);
        }

        public static async Task<bool> RestoreAccess()
        {


            var AESEngine = new Shared.Crypting.AES();

            var ClientRSAPublic = Shared.Model.LocalStorage.ServerRSAKey;
            var FirstPart = AESEngine.Encrypt(Shared.Model.LocalStorage.UID.ToString(), ClientRSAPublic);

            var ClientRSAPair = Shared.Crypting.RSA.CreateKeyPair();

            var ClientAESSign = AESEngine.Encrypt(ClientRSAPair.Item2, FirstPart);

            Shared.View.General.inLoading();
            var newSign = await Model.Requests.RestoreSessionKey(ClientAESSign);
            Shared.View.General.outLoading();
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
