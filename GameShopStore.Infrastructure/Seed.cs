using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using GameShopStore.Application.Helpers;

namespace GameShopStore.Infrastructure
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration appSettings)
        {
            if (!userManager.Users.Any())
            {
                var seedDataLocation = appSettings.GetSection(SeedDataLocationOpts.SeedDataLoc).Get<SeedDataLocationOpts>();
                var currDirectory = Directory.GetCurrentDirectory();

                string userData;
                var users = new List<User>();
                if (!currDirectory.Contains("GameShopStore"))
                {
                    var userSeedDataLocation = seedDataLocation.UserSeedData;

                    var testProjectDirectory = Directory.GetParent(currDirectory).Parent.Parent.Parent.FullName;

                    var combined = Path.GetFullPath(Path.Combine(testProjectDirectory, userSeedDataLocation));

                    userData = System.IO.File.ReadAllText(combined);
                    users = JsonConvert.DeserializeObject<List<User>>(userData);
                }
                else
                {
                    userData = System.IO.File.ReadAllText("../GameShopStore.Infrastructure/SeedDataS/UserSeedData.json");
                    users = JsonConvert.DeserializeObject<List<User>>(userData);
                }


                var roles = new List<Role>
                {
                    new Role{Name = "Customer"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "Admin"}
                };


                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }


                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Customer").Wait();
                }


                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "admin@shop.eu"
                };

                var result = userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
                }
            }

        }

        public static void SeedProductsFKs(ApplicationDbContext ctx)
        {
            if (ctx.Products.Select(p => p.Category).Any(l => l == null))
            {


                var products = ctx.Products;
                int categoryId = 1;
                foreach (var product in products)
                {
                    if (categoryId > 3)
                    {
                        categoryId = 1;
                    }

                    ctx.Entry(product).Property("CategoryId").CurrentValue = categoryId++;
                }
                ctx.SaveChanges();
            }


        }
    }
}
