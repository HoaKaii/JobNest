using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobsFinder_Main.Models
{
    public class UserCreate
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        public List<string> SelectedRoles { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
