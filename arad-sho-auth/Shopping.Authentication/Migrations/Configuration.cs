using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Models;
using Shopping.Authentication.Repository.DbContexts;
using Shopping.Authentication.Repository.Entities;
using Shopping.Authentication.SeedWorks.Enums;

namespace Shopping.Authentication.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(AuthContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<PanelUser>(context);
                var manager = new UserManager<PanelUser>(store);
                var user = new PanelUser("admin", "سینا", "علیزاده", "4000218190", "09195291442", "peji@gmail.com");
                manager.Create(user, "123456!");
                manager.AddToRole(user.Id, "Admin");
            }
            context.Clients.AddOrUpdate(new Client
            {
                ApplicationType = ApplicationType.ShopUserApp,
                Active = true,
                AllowedOrigin = "*",
                Id = "ShopUserApp",
                Name = "app user & web user",
                RefreshTokenLifeTime = 5000000,
                Secret = "ICXDroz4HhR1Elx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA="
            });
            context.Clients.AddOrUpdate(new Client
            {
                ApplicationType = ApplicationType.CustomerUserApp,
                Active = true,
                AllowedOrigin = "*",
                Id = "CustomerUserApp",
                Name = "app user & web user",
                RefreshTokenLifeTime = 5000000,
                Secret = "ICXDroz4HhR1Elx8qaz4C13z/quTXBkQ3Q5hj7Qx3aA="
            });
            context.Clients.AddOrUpdate(new Client
            {
                ApplicationType = ApplicationType.JavaScript,
                Active = true,
                AllowedOrigin = "*",
                Id = "ngAuthApp",
                Name = "app user & web user",
                RefreshTokenLifeTime = 7200,
                Secret = "ICXDroz4HhR1Elx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA="
            });
        }
    }
}
