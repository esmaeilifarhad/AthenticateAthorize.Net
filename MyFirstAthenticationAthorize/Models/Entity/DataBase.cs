using MyFirstAthenticationAthorize.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyFirstAthenticationAthorize.Models.Entity
{
    public class DataBase:DbContext
    {
        public DataBase() : base("name=AthenticateDatabase") {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<USER> users { get; set; }
        public DbSet<ROLE> Roles { get; set; }
        public DbSet<User_Role> Users_Roles { get; set; }
    }
}