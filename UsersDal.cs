using MVCEventCalendar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Controllers
{

        public class UsersDal : DbContext
        {
            public DbSet<Users> User { get; set; }


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {

                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Users>().ToTable("tblUser");
            }
        }
    
}