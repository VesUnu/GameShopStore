using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GameShopStore.Core.Models;
using GameShopStore.Infrastructure.Extensions;
using GameShopStore.Infrastructure.Configs;
using Microsoft.Extensions.Options;

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

        public DbSet<Picture> Pictures { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Requirements> Requirements { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<SubCategory> SubCategories { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<CategorySubCategory> CategoriesSubCategories { get; set; } = null!;
        public DbSet<ProductSubCategory> ProductsSubCategories { get; set; } = null!;
        public DbSet<ProductLanguage> ProductsLanaguages { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<OrderStock> OrderStocks { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<StockOnHold> StockOnHolds { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<DeliveryOpt> DeliveryOpts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.SeedProducts(_seedDataOptions);

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new RequirementsConfig());
            builder.ApplyConfiguration(new LanguageConfig());
            builder.ApplyConfiguration(new PictureConfig());
            builder.ApplyConfiguration(new CategoryConfig());
            builder.ApplyConfiguration(new SubCategoryConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new OrderStockConfig());
            builder.ApplyConfiguration(new StockOnHoldConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new DeliveryOptConfig());


            builder.Entity<Product>()
                .HasOne(p => p.Requirements)
                .WithOne(r => r.Product)
                .HasForeignKey<Requirements>(r => r.ProductId);

            builder.Entity<CategorySubCategory>()
                .HasKey(k => new { k.CategoryId, k.SubCategoryId });

            builder.Entity<CategorySubCategory>()
                .HasOne(csc => csc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(csc => csc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CategorySubCategory>()
                .HasOne(csc => csc.SubCategory)
                .WithMany(sc => sc.Categories)
                .HasForeignKey(csc => csc.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<ProductSubCategory>()
                .HasKey(k => new { k.ProductId, k.SubCategoryId });

            builder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(psc => psc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(psc => psc.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<ProductLanguage>()
                .HasKey(k => new { k.ProductId, k.LanguageId });

            builder.Entity<ProductLanguage>()
                .HasOne(pl => pl.Product)
                .WithMany(p => p.Languages)
                .HasForeignKey(pl => pl.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductLanguage>()
                .HasOne(pl => pl.Language)
                .WithMany(l => l.Products)
                .HasForeignKey(pl => pl.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<OrderStock>()
                .HasKey(k => new { k.StockId, k.OrderId });
        }

    }
}