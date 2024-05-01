namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("Reviews")]

    public partial class Review
    {
        public int ID { get; set; }           
        public int CompanyID { get; set; }     
        public string Comment { get; set; }  
        public int Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set;}
        public string UserID { get; set; }
        public string Name { get; set; }
    }
}
