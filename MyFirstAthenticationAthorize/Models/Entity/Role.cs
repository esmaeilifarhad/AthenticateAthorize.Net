using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFirstAthenticationAthorize.Models.Entity
{
    public class ROLE
    {
        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        [MaxLength(200)]
        public string TITLE { get; set; }
        [MaxLength(200)]
        public string TITLE_PERSIAN { get; set; }

        public virtual List<User_Role> users { get; set; }

    }
}