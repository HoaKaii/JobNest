using System.ComponentModel.DataAnnotations;

namespace JobsFinder_Main.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
