using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyFirstAthenticationAthorize.Models.Entity
{
    public class USER
    {
        [Key]//,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        [MaxLength(200)]
        public string NAME { get; set; }
        [MaxLength(200)]
        public string USERNAME { get; set; }
        [MaxLength(200)]
        public string PASSWORD { get; set; }
        [MaxLength(200), Required(AllowEmptyStrings = true)]
        public string PERSONNEL_CODE { get; set; }
        public decimal ROLE_ID { get; set; }
        public decimal IS_ACTIVE { get; set; }




        public virtual List<User_Role> users { get; set; }
    }
}