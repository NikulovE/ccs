using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Shared;
using System.Web.Http;

namespace WebAPI2.Models
{
    public class ProfileProcessing
    {
        public class UserRegistration
        {
            public String UserEmail;
            public String Password;
            public String UID;
            Tuple<string, string> KeyPair;


            public bool isEmailAlreadyPresent()
            {
                var dbo = new AppDbDataContext();
                if (dbo.Registrations.Any(req => req.Mail == UserEmail))
                {
                    return false;
                }
                else return true;
            }

            public bool GeneratePreRegistrationEntry()
            {
                Password = Shared.Crypting.GeneratePassword(10);
                UID = Shared.Crypting.GeneratePassword(9, true);
                try
                {
                    var dbo = new AppDbDataContext();

                    if (dbo.Registrations.Any(req => req.Mail == UserEmail))
                    {
                        var alreadyRegisteredUser = dbo.Registrations.First(req => req.Mail == UserEmail);

                        alreadyRegisteredUser.Password = Password;
                        alreadyRegisteredUser.Errors++;
                        alreadyRegisteredUser.TimeStamp = DateTime.Now;

                        var RSAEng = new Shared.Crypting.RSA();
                        KeyPair = RSAEng.CreateKeyPair();

                        alreadyRegisteredUser.RSAprivate = KeyPair.Item1;
                        alreadyRegisteredUser.RSApublic = KeyPair.Item2;


                        if (alreadyRegisteredUser.Errors > 15) return false;
                        dbo.SubmitChanges();
                    }
                    else
                    {
                        var newUserRegistration = new Registration();
                        newUserRegistration.Password = Password;
                        newUserRegistration.Errors = 0;
                        newUserRegistration.TimeStamp = DateTime.Now;
                        newUserRegistration.UID = int.Parse(UID);
                        newUserRegistration.Mail = UserEmail;


                        var RSAEng = new Crypting.RSA();
                        KeyPair = RSAEng.CreateKeyPair();

                        newUserRegistration.RSAprivate = KeyPair.Item1;
                        newUserRegistration.RSApublic = KeyPair.Item2;

                        try
                        {
                            dbo.Registrations.InsertOnSubmit(newUserRegistration);
                            dbo.SubmitChanges();
                        }
                        catch
                        {

                            UID = Shared.Crypting.GeneratePassword(9, true);
                            newUserRegistration.UID = int.Parse(UID);
                            dbo.Registrations.InsertOnSubmit(newUserRegistration);
                            dbo.SubmitChanges();
                        }

                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public String GeneratePreRegistrationAESKey()
            {
                var aesEngine = new Crypting.AES();
                var AESKey = aesEngine.Encrypt(UID + ',' + KeyPair.Item2, Password);
                return AESKey;

            }

            public bool RefreshPassword() {
                try
                {
                    var dbo = new AppDbDataContext();
                    var UserRegEntry = dbo.Registrations.First(req => req.Mail == UserEmail);
                    //var UserRegEntry = dbo.Registrations.First(req => req.Mail == UserEmail && req.isConfirmed == true);
                    Password = Shared.Crypting.GeneratePassword(8);
                    UserRegEntry.Password = Password;

                    KeyPair = new Tuple<string, string>("", UserRegEntry.RSApublic);
                    UID = UserRegEntry.UID.ToString();
                    dbo.SubmitChanges();
                    return true;
                }
                catch (Exception) {
                    return false;
                }

            }

        }

        public class Confirmation
        {
            public String Answer;
            public int UID;
            String Password;
            public String ClientRSA;

            public String ConfirmRegistration(bool RestoreAccess = false)
            {
                var UserID = UID;                

                var SessionKey = ProfileProcessing.SessionKey.CreateSessionKey(UID);
                if (SessionKey.Item1 != 0)
                {
                    var aesEngine = new Crypting.AES();
                    if (RestoreAccess) {
                        try
                        {
                            var Response = SessionKey.Item1.ToString() + "," + SessionKey.Item2;
                            return aesEngine.Encrypt(Response, Password);
                        }
                        catch (Exception) {
                            return "error";
                        }
                    }
                    else
                    {
                        if (CreateProfile())
                        {

                            var Response = SessionKey.Item1.ToString() + "," + SessionKey.Item2;
                            return aesEngine.Encrypt(Response, Password);
                        }

                        else return "error";
                    }
                }
                else
                {
                    return "error";
                }
            }

            public bool isAlreadyCreated()
            {
                var dbo = new AppDbDataContext();
                if (dbo.Users.Any(req => req.UID == UID)) return true;
                else return false;

            }

            public bool FindUserPassword()
            {
                try
                {
                    var dbo = new AppDbDataContext();
                    Password = dbo.Registrations.First(req => req.UID == UID).Password;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }


            public bool CheckAnswer()
            {
                try
                {
                    var dbo = new AppDbDataContext();
                    var User = dbo.Registrations.Single(req => req.UID == UID);
                    try
                    {
                        var RSAEng = new Crypting.RSA();
                        var Decrypt = RSAEng.Decrypt(Answer, User.RSAprivate);
                        if (Decrypt == User.Password)
                        {

                            User.isConfirmed = true;
                            dbo.SubmitChanges();
                            return true;
                        }
                        else
                        {
                            User.Errors++;
                            dbo.SubmitChanges();
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private bool CreateProfile()
            {
                var dbo = new AppDbDataContext();
                var newuser = new User();
                App.UID = UID;
                newuser.UID = UID;
                newuser.version = 2;
                newuser.OfficeID = -1;
                newuser.Rating = -1;
                try
                {

                    dbo.Users.InsertOnSubmit(newuser);
                    dbo.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally {
                    ScheduleProcessing.CreateDefaultSchedule(App.UID);
                }
            }



            public bool CheckPreSign() {
                try
                {
                    var dbo = new AppDbDataContext();
                    var User = dbo.Registrations.Single(req => req.UID == UID);
                    try
                    {
                        var AESEngine = new Crypting.AES();

                        var FirstPart = AESEngine.Encrypt(UID.ToString(), User.RSApublic);

                        //var RSAEngine = new Crypting.RSA();

                        ClientRSA = AESEngine.Decrypt(Answer, FirstPart);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }


            }

        }

        public class SessionKey
        {
            public Session Session;
            public int SessionID;
            public String Key;
            public String ClienPublicRSAKey;

            static AppDbDataContext dbo = new AppDbDataContext();

            public static Tuple<int, String> CreateSessionKey(int UID)
            {
                var newSession = new Session();
                newSession.UID = UID;
                var Crypkey = GenerateKey();
                newSession.CryptKey = Crypkey;
               // newSession.Number = int.Parse(Crypting.GeneratePassword(3, true));
                newSession.TimeStamp = DateTime.Now;
                //newSession.Iteration = 1;
                try
                {
                    dbo.Sessions.InsertOnSubmit(newSession);
                    dbo.SubmitChanges();
                    return new Tuple<int, String>(newSession.ID, Crypkey);
                }
                catch (Exception)
                {
                    return new Tuple<int, String>(0, "error");
                }

            }

            private static String GenerateKey()
            {
                return Crypting.GeneratePassword(8);
            }

            public static string EncryptedSession(String ClientRSAPublicKey, int SessionID, String Key) {
                var RSAEngine = new Crypting.RSA();
                return RSAEngine.Encrypt(SessionID.ToString() + ',' + Key.ToString(), ClientRSAPublicKey);

            }

            public string EncryptedSessionKey() {
                var RSAEngine = new Shared.Crypting.RSA();
                var NewKey = Key;
                return RSAEngine.Encrypt(NewKey, ClienPublicRSAKey);
            }

            public void UpdateSessionKey() {
                Session.TimeStamp = DateTime.Now;
                //Session.Number = Number;
                Key = GenerateKey();
                //Session.Iteration = 1;
                Session.CryptKey = Key;
                dbo.SubmitChanges();


            }

            public bool CheckUserSessionKey(String EncRequest)
            {
                try
                {
                    dbo = new AppDbDataContext();
                    Session = dbo.Sessions.Single(req => req.ID == SessionID);
                    //var RsaPub = Session.Registration.RSApublic;
                    //Password = Session.Registration.Password;
                    Key = Session.CryptKey;
                    try
                    {
                        var AESEngine = new Shared.Crypting.AES();
                        ClienPublicRSAKey = AESEngine.Decrypt(EncRequest, Key);
                        return true;
                    }
                    catch (Exception)
                    {
                        dbo.Sessions.DeleteOnSubmit(Session);
                        dbo.SubmitChanges();
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }


        }

        public class Profile
        {
            public UserProfile userProfile = new UserProfile();
            public int SessionID;
            User user;

            public bool LoadProfile()
            {

                try
                {
                    var dbo = new AppDbDataContext();
                    var userSession = dbo.Sessions.First(req => req.ID == SessionID);
                    user = userSession.User;
                    userProfile.FirstName = user.FirstName;
                    userProfile.LastName = user.LastName;
                    userProfile.Version = user.version.HasValue ? user.version.Value : 2;//
                    userProfile.Payment = user.Payment.HasValue ? user.Payment.Value : 0;//
                    userProfile.Rating = user.Rating.HasValue ? user.Rating.Value : 0;//
                    userProfile.OfficeID = user.OfficeID;
                    userProfile.IsDriver = user.isDriver.HasValue ? user.isDriver.Value : false;//
                    var AESEngine = new Crypting.AES();
                    userProfile.Phone = AESEngine.Encrypt(user.Phone, userSession.CryptKey);
                    userProfile.Extension = AESEngine.Encrypt(userSession.Registration.Mail, userSession.CryptKey);
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }

            }

            public bool ChangeUserProfile(UserProfile uProfile)
            {
                var previous = false;
                try
                {
                    var dbo = new AppDbDataContext();
                    user = dbo.Sessions.First(req => req.ID == SessionID).User;

                    if (uProfile.FirstName != "") user.FirstName = uProfile.FirstName;
                    if (uProfile.LastName != "") user.LastName = uProfile.LastName;
                    user.version++;
                    userProfile.Version = user.version.HasValue ? user.version.Value : 3;
                    if (uProfile.Phone != "") user.Phone = uProfile.Phone;
                    user.Payment = uProfile.Payment;
                    user.OfficeID = uProfile.OfficeID;
                    try
                    {
                        previous = user.isDriver.Value;
                    }
                    catch {

                    }

                    user.isDriver = uProfile.IsDriver;
                    dbo.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally {
                    if (previous != uProfile.IsDriver) TripProcessing.CancelEveryOffer();
                }

            }

            public bool ChangeDriverMode(bool isDriver)
            {
                try
                {
                    var dbo = new AppDbDataContext();
                    user = dbo.Sessions.First(req => req.ID == SessionID).User;
                    user.isDriver = isDriver;
                    user.version++;
                    userProfile.Version = user.version.HasValue ? user.version.Value : 3;
                    try
                    {
                        var usertrips = dbo.Trips.Where(req => req.DriverUID == App.UID || req.PassengerUID == App.UID);
                        dbo.Trips.DeleteAllOnSubmit(usertrips);                       
                    }
                    catch { }
                    dbo.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (!CarProcessing.isAlreadHasCar() && isDriver)
                    {
                        CarProcessing.CreateCar(App.UID);
                    }
                    
                }
            }

        }

        public static bool CheckCookies(String SessionKey) {
            var AESEngine = new Shared.Crypting.AES();            
            try
            {
                var DBSha = Shared.Crypting.SHA.CreateHash(SessionKey, "saltS");
                var cookie = new HttpCookie("Token", DBSha);
                var userToken = HttpContext.Current.Request.Cookies.Get("Token").Value;
                if (DBSha == userToken) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetCookies(String SessionKey) {
            try
            {
                var DBSha = Shared.Crypting.SHA.CreateHash(SessionKey, "saltS");
                var cookie = new HttpCookie("Token", DBSha);
                cookie.Value = DBSha;
                try
                {
                    HttpContext.Current.Response.Cookies.Set(cookie);//.Add(cookie);
                }
                catch (Exception) {
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return true;
            }
            catch {
                return false;
            }
        }

        public static String ErrorCode = "";



        public static bool CheckSign(int SessionID, String Sign)
        {
            var dbo = new AppDbDataContext();
            var AESEngine = new Shared.Crypting.AES();
            var SignSessionTime = new DateTime();
            long SignTicks = 0;
            var ServerTime = DateTime.UtcNow.ToUniversalTime();
#if DEBUG
            var MaxTime = ServerTime.AddMinutes(250);
            var MinTime = ServerTime.AddMinutes(-250);

#else
             var MaxTime = ServerTime.AddMinutes(5);
            var MinTime = ServerTime.AddMinutes(-5);
#endif
            try
            {
                var UserSession = dbo.Sessions.First(req => req.ID == SessionID);
                var DecryptedSign = "";
                
                try
                {

                    DecryptedSign = AESEngine.Decrypt(Sign, UserSession.CryptKey);
                    var SecureKey = DecryptedSign.Split(',');
                    ErrorCode = "SessionKey not decrypeted";
                    try
                    {
                        SignTicks = long.Parse(SecureKey[0]);
                        SignSessionTime = new DateTime(SignTicks);    
                        if (SignSessionTime < MinTime || SignSessionTime >  MaxTime)
                        {
                            ErrorCode = "Time not correct " + MinTime +"||" +new DateTime(SignTicks)+ "||"+ MaxTime;
                            return false;
                        }
                        else
                        {
                            if (DateTime.Now.Day > (UserSession.TimeStamp.Value.Day + 2)) return false;
                            if (SecureKey[1] == UserSession.ID.ToString()) {
                                if (CheckCookies(UserSession.CryptKey)) return true;
                                else
                                {
                                    ErrorCode = "Wrong cookies";
                                    return false;
                                }
                                      
                            }
                            else
                            {
                                dbo.Sessions.DeleteOnSubmit(UserSession);
                                dbo.SubmitChanges();
                                return false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    finally
                    {
                        App.UID = UserSession.UID;
                        App.SessionID = SessionID;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception) {
                ErrorCode = "SID not found";
                return false;
            }
        }
        
    }
}