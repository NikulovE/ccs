using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Models
{
    public class UserFeedback
    {
        [Required(ErrorMessage = "Please, put some words")]
        public string FeedbackText { get; set; }
        public byte SelectedStarsCounter { get; set; }
        public IEnumerable<SelectListItem> StarsSelector { get; set; }

    }
}