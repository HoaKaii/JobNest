using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsFinder_Main.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { get; set; }

        [StringLength(32, MinimumLength = 6, ErrorMessage = "The username must contain at least 6 characters.")]
        [Required(ErrorMessage = "Please enter your username.")]
        public string UserName { get; set; }

        [StringLength(32, MinimumLength = 6, ErrorMessage = "The password must contain at least 6 characters.")]
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your fullname.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        public string Email { get; set; }

        public string Avatar { get; set; }
    }
}