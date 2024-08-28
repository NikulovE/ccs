using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPI2.Models;
using Microsoft.Owin.Security.DataProtection;

namespace WebAPI2.Controllers.Profile
{
    public class ProfileController : ApiController
    {        

        [HttpGet]
        public Tuple<bool, UserProfile> LoadProfile(int SessionID, String Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var uProfile = new UserProfile();
                var profile = new ProfileProcessing.Profile { SessionID = SessionID, userProfile = uProfile };
                if (profile.LoadProfile())
                {
                    return new Tuple<bool, UserProfile>(true, profile.userProfile);
                }
                else return new Tuple<bool, UserProfile>(false, new UserProfile());
            }
            else
            {
                return new Tuple<bool, UserProfile>(false, new UserProfile { FirstName = "WrongSign" , LastName=Sign, Extension=ProfileProcessing.ErrorCode });
            }
        }

        [HttpPost]
        public Tuple<bool, string> Logon(int SessionID, [FromBody]String EncryptedRequest)
        {
            var userSession = new ProfileProcessing.SessionKey { SessionID = SessionID };
            if (userSession.CheckUserSessionKey(EncryptedRequest))
            {
                userSession.UpdateSessionKey();
                var NewSessionKey = userSession.EncryptedSessionKey();
                if (ProfileProcessing.SetCookies(userSession.Key))
                {
                    return new Tuple<bool, String>(true, NewSessionKey);
                }
                else
                {
                    return new Tuple<bool, String>(false, "x11002");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x11002");
            }
        }

        [HttpPost]
        public Tuple<bool, String> ChangeDriverMode(int SessionID, String Sign, [FromBody]bool isDriver)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var uprofile = new ProfileProcessing.Profile { SessionID = SessionID };
                if (uprofile.ChangeDriverMode(isDriver))
                {
                    return new Tuple<bool, String>(true, uprofile.userProfile.Version.ToString());
                }
                else
                {
                    return new Tuple<bool, String>(false, "x13002");
                }

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001");
            }
        }


    }

    public class RegistrationProfileController : ApiController
    {
        //private ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //}
        [HttpPut]
        public Tuple<bool, String> StartRegistration([FromBody]string email)
        {
            var newuser = new ProfileProcessing.UserRegistration();
            email = email.ToLower();
            newuser.UserEmail = email.ToLower();
            //return UserRegistration.CheckEmailAlreadyPresent(useremail);
            if (General.isEmailFormatValid(email))
            {
                if (OrganizationProcessing.isOrgMailbox(email.Split('@')[1])) return new Tuple<bool, String>(false, "x10007"); //email already tried to register
                if (newuser.isEmailAlreadyPresent())
                {
                    if (newuser.GeneratePreRegistrationEntry())
                    {
                        if (General.SendKeyToEmail(email, newuser.Password))
                        {
                            //try
                            //{
                            //    UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
                            //    ApplicationUser user = new ApplicationUser { UserName = email, UID = int.Parse(newuser.UID) };                               
                            //    UserManager.Create(user);
                            //}
                            //catch { }
                            return new Tuple<bool, String>(true, newuser.GeneratePreRegistrationAESKey());
                        }
                        else return new Tuple<bool, String>(false, "x10001"); //can`t send
                    }
                    else return new Tuple<bool, String>(false, "x10002"); //can`t generate preinfo
                }
                else return new Tuple<bool, String>(false, "x10003"); //email already tried to register

            }
            else return new Tuple<bool, String>(false, "x10005"); //email not valid

        }

        [HttpPost]
        public Tuple<bool, String> ConfirmRegistration(int UID, [FromBody]String Answer)
        {
            var userConfirmation = new ProfileProcessing.Confirmation { Answer = Answer, UID = UID };

            if (userConfirmation.FindUserPassword())
            {
                if (userConfirmation.isAlreadyCreated())
                {
                    if (userConfirmation.CheckAnswer())
                    {
                        var SessionKey = userConfirmation.ConfirmRegistration(true);
                        switch (SessionKey)
                        {
                            case "error0": return new Tuple<bool, String>(false, "x11006");
                            case "error1": return new Tuple<bool, String>(false, "x11007");
                            case "error2": return new Tuple<bool, String>(false, "x11008");
                            default:
                                {
                                    //UserManager.AddPassword("123", SessionKey);
                                    return new Tuple<bool, String>(true, SessionKey);
                                }

                        }
                        //if (SessionKey != "error0" && ) return new Tuple<bool, String>(true, SessionKey);
                        //else return new Tuple<bool, String>(false, "x11002");
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x11003");
                    }
                }
                else
                {
                    if (userConfirmation.CheckAnswer())
                    {
                        var SessionKey = userConfirmation.ConfirmRegistration();
                        if (SessionKey != "error") return new Tuple<bool, String>(true, SessionKey);
                        else return new Tuple<bool, String>(false, "x11002");
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x11003");
                    }
                }
            }
            else return new Tuple<bool, String>(false, "x11004");
        }
    }

    public class RestoreProfileController : ApiController
    {
        [HttpPost]
        public Tuple<bool, String> RestoreSessionKey(int UID, [FromBody]String ClientRequest)
        {
            var userConfirmation = new ProfileProcessing.Confirmation { Answer = ClientRequest, UID = UID };
            if (userConfirmation.CheckPreSign())
            {
                var newSessionPair = ProfileProcessing.SessionKey.CreateSessionKey(UID);
                if (newSessionPair.Item2 != "error")
                {
                    return new Tuple<bool, String>(true, ProfileProcessing.SessionKey.EncryptedSession(userConfirmation.ClientRSA, newSessionPair.Item1, newSessionPair.Item2));
                }
                else
                {
                    return new Tuple<bool, String>(false, "x23004");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x23005");
            }
        }
    }
    public class UpdateProfileController : ApiController
    {
        [HttpPost]
        public Tuple<bool, String> UpdateProfile(int SessionID, String Sign, [FromBody]UserProfile UserProfile)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (UserProfile.FirstName.Length > 70 || UserProfile.LastName.Length > 70 || UserProfile.Phone.Length > 12)
                {
                    return new Tuple<bool, String>(false, "x12004");
                }
                var uprofile = new ProfileProcessing.Profile { SessionID = SessionID };
                if (uprofile.ChangeUserProfile(UserProfile))
                {
                    return new Tuple<bool, String>(true, uprofile.userProfile.Version.ToString());
                }
                else
                {
                    return new Tuple<bool, String>(false, "x12003");
                }
            }
            else
            {
                return new Tuple<bool, String>(false, "x12001");
            }
        }
    }
    public class RestoreRegistrationController : ApiController
    {
        [HttpPost]
        public Tuple<bool, String> RestoreRegistration([FromBody]string email)
        {
            var alreadyRegisteredUser = new ProfileProcessing.UserRegistration { UserEmail = email };
            if (alreadyRegisteredUser.RefreshPassword())
            {
                General.SendKeyToEmail(email, alreadyRegisteredUser.Password);
                return new Tuple<bool, String>(true, alreadyRegisteredUser.GeneratePreRegistrationAESKey());
            }
            else return new Tuple<bool, String>(false, "x10006"); //can`t generate preinfo

        }


    }
}
