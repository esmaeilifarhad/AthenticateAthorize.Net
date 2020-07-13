using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFirstAthenticationAthorize.Models.Entity
{
    public class User_Role
    {
        [Key]
        public int ID { get; set; }
        //public Nullable<int> Roles_ID { get; set; }
        //public Nullable<decimal> Users_ID { get; set; }
        public virtual ROLE Roles { get; set; }
        public virtual USER Users { get; set; }
    }
}