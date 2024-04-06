using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsFinder_Main.Models
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Username cannot be left empty.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }
    }
}