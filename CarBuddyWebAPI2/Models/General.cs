using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class General
    {
        public static bool isEmailFormatValid(String UserEmail)
        {
            try
            {
                var isEmailValid = new EmailAddressAttribute().IsValid(UserEmail);
                if (isEmailValid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendKeyToEmail(String Email, String Password)
        {
            if (Email == "unicarbuddy@icl-services.com") return true;
            if (Shared.SendingMail.SendEmail(Email, "Your password is: " + Password, "Your password for " + Shared.General.AppName)) return true;
            else return false;
        }


    }
}