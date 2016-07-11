namespace Angular.DomainModel.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Angular.DomainModel.DataContext.AngularDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Angular.DomainModel.DataContext.AngularDbContext context)
        {
            #region Admin project seed data

          

            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var store = new UserStore<IdentityUser>(context);
                var manager = new UserManager<IdentityUser>(store);
                var user = new IdentityUser { UserName = "admin" };
                manager.Create(user, "P@ssword");
               
            }
            #endregion


            #region Event Registration Seed Data
            context.EventRegistration.AddOrUpdate(x => x.Id, new Models.EventRegistration { Id = 1, Name = "Test" });
            context.SaveChanges();
            #endregion
        }
    }
}
