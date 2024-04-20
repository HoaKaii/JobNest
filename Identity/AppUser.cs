using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;


namespace JobsFinder_Main.Identity
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool Status { get; set; }
        public string Avatar { get; set; }
    }
}
