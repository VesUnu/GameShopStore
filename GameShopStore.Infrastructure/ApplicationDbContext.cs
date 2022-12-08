using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GameShopStore.Application.Helpers;
using Microsoft.Extensions.Configuration;
using GameShopStore.Core.Models;

namespace GameShopStore.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IConfiguration _seedDataOptions;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration seedDataOptions)
            : base(options)
        {
            _seedDataOptions = seedDataOptions;
        }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Requirements> Requirements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<CategorySubCategory> CategoriesSubCategories { get; set; }
        public DbSet<ProductSubCategory> ProductsSubCategories { get; set; }
        public DbSet<ProductLanguage> ProductsLanaguages { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StockOnHold> StockOnHolds { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DeliveryOpt> DeliveryOpts { get; set; }
    }
}