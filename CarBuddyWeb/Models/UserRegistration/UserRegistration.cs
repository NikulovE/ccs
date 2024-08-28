using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class ProfileRegistration
    {
        [Required(ErrorMessage = "Please, use your personal e-mail, not your work mail")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Inputed email is not correct")]
        public string Email { get; set; }

    }

    public class ProfileConfirmation
    {
        [Required(ErrorMessage = "Find pass in a mail")]
        public string Password { get; set; }

    }
}