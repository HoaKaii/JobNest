using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsFinder_Main.Models
{
    public class CompanyModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The company name cannot be empty.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The company homepage cannot be empty.")]
        public string LinkPage { get; set; }

        [Required(ErrorMessage = "The company description cannot be empty.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please upload a company logo.")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Please upload a cover photo for the company.")]
        public string Background { get; set; }

        [Required(ErrorMessage = "Please enter the number of employees in the company.")]
        public int? Employees { get; set; }

        [Required(ErrorMessage = "The company location cannot be empty.")]
        public string Location { get; set; }
    }
}