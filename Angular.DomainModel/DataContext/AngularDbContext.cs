using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Angular.DomainModel.Models;
using Angular.DomainModel.Migrations;

namespace Angular.DomainModel.DataContext
{
    public class AngularDbContext : IdentityDbContext<IdentityUser>
    {
        public AngularDbContext() : base("AngularDbContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AngularDbContext, Configuration>("AngularDbContext"));
        }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<EventRegistration> EventRegistration { get; set; }
        public static AngularDbContext Create()
        {
            return new AngularDbContext();
        }
    }
}
