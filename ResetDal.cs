using MVCEventCalendar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.DAL
{
    public class ResetDal : DbContext
    {
        public DbSet<Reset> reset { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reset>().ToTable("tblReset");
        }
    }
}