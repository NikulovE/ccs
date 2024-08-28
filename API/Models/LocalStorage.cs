
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;


#if ANDROID
using Android.Content;
using Android.App;
#endif

#if XAMARIN

using Xamarin.Auth;
using Xamarin.Forms;
#endif
#if NETFX_CORE
using Windows.Security.Credentials;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

namespace Shared.Model
{
    public class LocalStorage
    {

        public static void Reset()
        {

            ProfileVersion = 0;
            SessionID = 0;
            SessionKey = "x";
            UID = 0;
            ServerRSAKey = "x";
            RegistrationAESKey = "";



        }

        public static Int64 SystemTime
        {
            get
            {

                return DateTime.UtcNow.Ticks;
            }

        }

        public static int ProfileVersion
        {
            get
            {
#if XAMARIN
                try
                {
                    return (int)Application.Current.Properties["ProfileVersion"];
                }
                catch
                {
                    Application.Current.Properties["ProfileVersion"] = 0;
                    return (int)Application.Current.Properties["ProfileVersion"];
                }
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return int.Parse(contextPref.GetString("ProfileVersion", "0"));
#else
                try
                {
                    return (int)ApplicationData.Current.RoamingSettings.Values["ProfileVersion"];
                }
                catch
                {
                    ApplicationData.Current.RoamingSettings.Values["ProfileVersion"] = 0;
                    return (int)ApplicationData.Current.RoamingSettings.Values["ProfileVersion"];
                }
#endif
#endif
            }
            set
            {
#if XAMARIN
                Application.Current.Properties["ProfileVersion"] = value;
                Application.Current.SavePropertiesAsync();
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("ProfileVersion", value.ToString());
                contextEdit.Commit();
#else
                ApplicationData.Current.RoamingSettings.Values["ProfileVersion"] = value;
#endif
#endif
#if WPF
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
            }

        }

        public static int SessionID
        {
            get
            {
#if XAMARIN
                try
                {
                    return (int)Application.Current.Properties["SessionID"];
                }
                catch
                {
                    Application.Current.Properties["SessionID"] = 0;
                    return (int)Application.Current.Properties["SessionID"];
                }
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return int.Parse(contextPref.GetString("SessionID", "0"));
#else
                try
                {
                    return (int)ApplicationData.Current.RoamingSettings.Values["SessionID"];
                }
                catch
                {
                    ApplicationData.Current.RoamingSettings.Values["SessionID"] = 0;
                    return (int)ApplicationData.Current.RoamingSettings.Values["SessionID"];
                }
#endif
#endif
            }
            set
            {
#if XAMARIN
                Application.Current.Properties["SessionID"] = value;
                Application.Current.SavePropertiesAsync();
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("SessionID", value.ToString());
                contextEdit.Commit();
#else
                ApplicationData.Current.RoamingSettings.Values["SessionID"] = value;
#endif
#endif
#if WPF
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
            }

        }

        public static String SessionKey
        {
            get
            {

#if NETFX_CORE
                try
                {
                    Windows.Security.Credentials.PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
                    var cred = vault.Retrieve("SessionKey", "Commute Car Sharing");
                    return cred.Password;
                }
                catch (Exception) { return "x"; };
#endif
#if WPF
                try
                {
                    return Shared.Crypting.RSA.DecryptUKey(ApplicationData.Current.RoamingSettings.Values["SessionKey"].ToString(), ContainerName);
                }
                catch (Exception) {
                    return "";
                }
#endif

#if XAMARIN
                var account = AccountStore.Create().FindAccountsForService("unicarbuddy").FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
#endif

#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return contextPref.GetString("SessionKey", "0");
#endif
            }
            set
            {


#if NETFX_CORE
                Windows.Security.Credentials.PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
                try
                {

                    PasswordCredential cred = new PasswordCredential("SessionKey", "Commute Car Sharing", value);
                    vault.Add(cred);
                }
                catch (Exception)
                {
                    var cred = vault.Retrieve("SessionKey", "Commute Car Sharing");
                    cred.Password = value;
                }
#endif
#if WPF
                ApplicationData.Current.RoamingSettings.Values["SessionKey"] = Shared.Crypting.RSA.EncryptUKey(value, ContainerName);

                ApplicationData.Current.RoamingSettings.Values.Save();
#endif

#if XAMARIN
                if (!string.IsNullOrWhiteSpace("UniCarBuddy") && !string.IsNullOrWhiteSpace(value))
                {
                    Account account = new Account
                    {
                        Username = "UniCarBuddy"
                    };
                    account.Properties.Add("Password", value);
                    AccountStore.Create().Save(account, "unicarbuddy");
                }
#endif
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("SessionKey", value);
                contextEdit.Commit();
#endif

            }

        }


        public static int UID
        {
            get
            {
#if XAMARIN
                return (int)Application.Current.Properties["UID"];
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return int.Parse(contextPref.GetString("UID", "0"));
#else
                return (int)ApplicationData.Current.RoamingSettings.Values["UID"];
#endif
#endif
            }
            set
            {
#if XAMARIN
                Application.Current.Properties["UID"] = value;
                Application.Current.SavePropertiesAsync();
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("UID", value.ToString());
                contextEdit.Commit();
#else
                ApplicationData.Current.RoamingSettings.Values["UID"] = value;
#endif
#endif

#if WPF
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
            }

        }

        public static String Email
        {
            get
            {
#if XAMARIN
                return (String)Application.Current.Properties["Email"];
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return contextPref.GetString("Email", "0");
#else
                return (String)ApplicationData.Current.RoamingSettings.Values["Email"];
#endif
#endif
            }
            set
            {
#if XAMARIN
                Application.Current.Properties["Email"] = value;
                Application.Current.SavePropertiesAsync();
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("Email", value);
                contextEdit.Commit();
#else
                ApplicationData.Current.RoamingSettings.Values["Email"] = value;
#endif
#endif

#if WPF
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
            }

        }

#if WPF
        public static String ContainerName
        {
            get
            {

                return ApplicationData.Current.RoamingSettings.Values["ContainerName"].ToString();


    }
    set
            {

                ApplicationData.Current.RoamingSettings.Values["ContainerName"] = Shared.Crypting.RSA.CreateUserKeyPair(value, 2048);
                ApplicationData.Current.RoamingSettings.Values.Save();
            }
        }
#endif




        public static String ServerRSAKey
        {
            get
            {
#if NETFX_CORE
                Windows.Security.Credentials.PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
                var cred = vault.Retrieve("ServerRSAKey", "Commute Car Sharing");
                return cred.Password;
#else
                var AESEngine = new Shared.Crypting.AES();
#if WPF
                return AESEngine.Decrypt(ApplicationData.Current.RoamingSettings.Values["ServerRSAKey"].ToString(), MasterAESKey);
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return contextPref.GetString("ServerRSAKey", "0");
#else
                var account = AccountStore.Create().FindAccountsForService("unicarbuddys").FirstOrDefault();
                return (account != null) ? account.Properties["ServerRSAKey"] : null;
#endif
#endif
#endif


            }
            set
            {
#if NETFX_CORE
                 Windows.Security.Credentials.PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
                try
                {
                    PasswordCredential cred = new PasswordCredential("ServerRSAKey", "Commute Car Sharing", value);
                    vault.Add(cred);
                }
                catch
                {
                    var cred = vault.Retrieve("ServerRSAKey", "Commute Car Sharing");
                    cred.Password = value;
                }
#else
                var AESEngine = new Shared.Crypting.AES();
#if WPF
                ApplicationData.Current.RoamingSettings.Values["ServerRSAKey"] = AESEngine.Encrypt(value, MasterAESKey);
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
#if XAMARIN
                if (!string.IsNullOrWhiteSpace("UniCarBuddyS") && !string.IsNullOrWhiteSpace(value))
                {
                    Account account = new Account
                    {
                        Username = "UniCarBuddyS"
                    };
                    account.Properties.Add("ServerRSAKey", value);
                    AccountStore.Create().Save(account, "unicarbuddys");
                }
#endif
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("ServerRSAKey", value);
                contextEdit.Commit();
#endif
#endif


            }

        }

#if WPF
        public static String MasterAESKey
        {
            get
            {

                return Shared.Crypting.RSA.DecryptUKey(ApplicationData.Current.RoamingSettings.Values["MasterAESKey"].ToString(), ContainerName);

        }
            set
            {

                ApplicationData.Current.RoamingSettings.Values["MasterAESKey"] = Shared.Crypting.RSA.EncryptUKey(value, ContainerName);
                ApplicationData.Current.RoamingSettings.Values.Save();

            }
        }
#endif



        public static String RegistrationAESKey
        {
            get
            {
#if XAMARIN
                return (string)Application.Current.Properties["RegistrationAESKey"];
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                return contextPref.GetString("RegistrationAESKey", "0");
#else
                return (string)ApplicationData.Current.RoamingSettings.Values["RegistrationAESKey"];
#endif
#endif

            }
            set
            {
#if XAMARIN
                Application.Current.Properties["RegistrationAESKey"] = value;
                Application.Current.Properties["ProfileVersion"] = 1;
                Application.Current.SavePropertiesAsync();
#else
#if ANDROID
                var contextPref = Application.Context.GetSharedPreferences("CCS", FileCreationMode.Private);
                var contextEdit = contextPref.Edit();
                contextEdit.PutString("RegistrationAESKey", value);
                contextEdit.PutString("ProfileVersion", "1");
                contextEdit.Commit();
#else
                ApplicationData.Current.RoamingSettings.Values["RegistrationAESKey"] = value;
                ApplicationData.Current.RoamingSettings.Values["ProfileVersion"] = 1;
                
#endif
#endif
#if WPF
                ApplicationData.Current.RoamingSettings.Values.Save();
#endif
            }
        }
    }
}

