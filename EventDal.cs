using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCEventCalendar.Models;

namespace MVCEventCalendar.DAL
{
    public class EventDal : DbContext
    {
        public DbSet<Models.Event> Event { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Event>().ToTable("tblEvent");
        }
    }
}
