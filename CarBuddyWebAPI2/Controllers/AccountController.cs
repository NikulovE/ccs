//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;
//using WebAPI2.Models;
//using Microsoft.Owin.Security.DataProtection;

//namespace WebAPI2.Controllers
//{
//    public class AccountController : ApiController
//    {
//        private ApplicationUserManager UserManager
//        {
//            get
//            {
//                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            }
//        }
//        [HttpGet]
//        public string EmailConfirmation(string userId, string code)
//        {
//            if (userId == null || code == null)
//            {
//                return "Error";
//            }
//            var provider = new DpapiDataProtectionProvider("CCS");
//            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
//            var result = UserManager.ConfirmEmail(userId, code);
//            if (result.Succeeded)
//            {
//                return "Successfully confirmed";
//            }
//            else {
//                return "Confirmation code is not correct";
//            }
//        }


//        //public ActionResult Register()
//        //{
//        //    return View();
//        ////}
//        //[HttpPost]
//        //public string Post()
//        //{
//        //    return "POST";
//        //}
//        //[HttpPost]
//        //public string Post(String s)
//        //{
//        //    return "POST String";
//        //}
//        [HttpPost]
//        [AllowAnonymous]
//        public Tuple<bool, String> Register(ApplicationRegistration model)
//        {
//            if (ModelState.IsValid)
//            {
//                var newuser = new ProfileProcessing.UserRegistration();
//                newuser.UserEmail = model.Email.ToLower();
//                //return UserRegistration.CheckEmailAlreadyPresent(useremail);
//                if (General.isEmailFormatValid(model.Email))
//                {
//                    if (OrganizationProcessing.isOrgMailbox(model.Email.Split('@')[1])) return new Tuple<bool, String>(false, "x10007"); //email already tried to register
//                    if (newuser.isEmailAlreadyPresent())
//                    {
//                        if (newuser.GeneratePreRegistrationEntry())
//                        {
//                            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
//                            ApplicationUser user = new ApplicationUser { UserName = model.Email, UID=int.Parse(newuser.UID) };
//                            IdentityResult result;
//                            try
//                            {
//                                result = UserManager.Create(user, model.Password);
//                            }
//                            catch {
//                                return new Tuple<bool, String>(false, "x10002"); //can`t generate preinfo
//                            }
//                            if (result.Succeeded)
//                            {
//                                if (SendConfirmationEmail(user.Id, model.Email))
//                                {
//                                    return new Tuple<bool, String>(true, newuser.UID); //sent
//                                }
//                                else {
//                                    return new Tuple<bool, String>(false, "x10001"); //can`t send
//                                }
//                            }
//                            else return new Tuple<bool, String>(false, "x10002"); //can`t generate preinfo
//                        }
//                        else return new Tuple<bool, String>(false, "x10002"); //can`t generate preinfo
//                    }
//                    else return new Tuple<bool, String>(false, "x10003"); //email already tried to register

//                }
//                else return new Tuple<bool, String>(false, "x10005"); //email not valid                
//            }
//            else
//            {
//                return new Tuple<bool, String>(false, "x10005"); //email not valid
//            }
//        }

//        private bool SendConfirmationEmail(string userId, string email) {
//            var provider = new DpapiDataProtectionProvider("CCS");
//            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
//            var code = UserManager.GenerateEmailConfirmationToken(userId);
//            string ui = userId;
//#if DEBUG
//            var url = "http://localhost:59524/api/account/EmailConfirmation?userId=" + ui + "&code=" + Uri.EscapeDataString(code);
//#else
//            var url = "https://api.commutecarsharing.ru/api/account/EmailConfirmation?userId=" + ui + "&code=" + Uri.EscapeDataString(code);
            
//#endif
//            if (General.SendKeyToEmail(email, url)) return true;
//            else return false;
//        }

//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return HttpContext.Current.GetOwinContext().Authentication;
//            }
//        }


//        [HttpPut]
//        public async Task<ApplicationLogin> Login(ApplicationLogin model)
//        {
//            if (ModelState.IsValid)
//            {
//                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
//                if (user == null)
//                {
//                    ModelState.AddModelError("", "Неверный логин или пароль.");
//                }
//                else
//                {
//                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
//                                            DefaultAuthenticationTypes.ApplicationCookie);
//                    AuthenticationManager.SignOut();
//                    AuthenticationManager.SignIn(new AuthenticationProperties
//                    {
//                        IsPersistent = true
//                    }, claim);
//                }
//            }
//            return model;
//        }
//        [HttpDelete]
//        public void Logout()
//        {
//            AuthenticationManager.SignOut();
//        }
//    }
//}
