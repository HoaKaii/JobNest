using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsFinder_Main.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Plese enter username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Plese enter password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
