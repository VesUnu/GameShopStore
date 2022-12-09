using GameShopStore.Application.Helpers;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedProducts(this ModelBuilder modelBuilder, IConfiguration appSettings)
        {
            var seedDataLocationOptions = appSettings.GetSection(SeedDataLocationOpts.SeedDataLoc).Get<SeedDataLocationOpts>();
            string productSeedDataLocation = seedDataLocationOptions.ProductSeedData;


            var currDirectory = Directory.GetCurrentDirectory();

            List<Product> products = ModelBuilderExtension.GetSeedDataOf<Product>(currDirectory, productSeedDataLocation);


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "PC", Description = "PC Description" },
                new Category { Id = 2, Name = "PS4", Description = "PS4 Description" },
                new Category { Id = 3, Name = "XONE", Description = "XBOX One Description" }

            );

            modelBuilder.Entity<Stock>().HasData(
                new Stock { Id = 1, ProductId = 1, Quantity = 145 },
                new Stock { Id = 2, ProductId = 2, Quantity = 70 },
                new Stock { Id = 3, ProductId = 3, Quantity = 80 },
                new Stock { Id = 4, ProductId = 4, Quantity = 160 },
                new Stock { Id = 5, ProductId = 5, Quantity = 105 },
                new Stock { Id = 6, ProductId = 6, Quantity = 45 },
                new Stock { Id = 7, ProductId = 7, Quantity = 120 },
                new Stock { Id = 8, ProductId = 8, Quantity = 60 },
                new Stock { Id = 9, ProductId = 9, Quantity = 140 }
            );


            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Name = "Polish" },
                new Language { Id = 2, Name = "English" },
                new Language { Id = 3, Name = "German" },
                new Language { Id = 4, Name = "Russian" },
                new Language { Id = 5, Name = "French" },
                new Language { Id = 6, Name = "Italian" },
                new Language { Id = 7, Name = "Spanish"}
            );
            modelBuilder.Entity<ProductLanguage>().HasData(
                new ProductLanguage { ProductId = 1, LanguageId = 1 },
                new ProductLanguage { ProductId = 1, LanguageId = 2 },
                new ProductLanguage { ProductId = 1, LanguageId = 5 },
                new ProductLanguage { ProductId = 2, LanguageId = 2 },
                new ProductLanguage { ProductId = 2, LanguageId = 4 },
                new ProductLanguage { ProductId = 3, LanguageId = 2 },
                new ProductLanguage { ProductId = 4, LanguageId = 1 },
                new ProductLanguage { ProductId = 4, LanguageId = 2 },
                new ProductLanguage { ProductId = 4, LanguageId = 3 },
                new ProductLanguage { ProductId = 4, LanguageId = 4 },
                new ProductLanguage { ProductId = 4, LanguageId = 5 },
                new ProductLanguage { ProductId = 4, LanguageId = 6 },
                new ProductLanguage { ProductId = 5, LanguageId = 2 },
                new ProductLanguage { ProductId = 5, LanguageId = 3 },
                new ProductLanguage { ProductId = 6, LanguageId = 1 },
                new ProductLanguage { ProductId = 6, LanguageId = 3 },
                new ProductLanguage { ProductId = 7, LanguageId = 2 },
                new ProductLanguage { ProductId = 7, LanguageId = 6 },
                new ProductLanguage { ProductId = 8, LanguageId = 2 },
                new ProductLanguage { ProductId = 8, LanguageId = 7 },
                new ProductLanguage { ProductId = 9, LanguageId = 2 },
                new ProductLanguage { ProductId = 9, LanguageId = 3 },
                new ProductLanguage { ProductId = 9, LanguageId = 5 },
                new ProductLanguage { ProductId = 9, LanguageId = 7 }
            );

            modelBuilder.Entity<ProductSubCategory>().HasData(
                new ProductSubCategory { ProductId = 1, SubCategoryId = 7 },
                new ProductSubCategory { ProductId = 2, SubCategoryId = 8 },
                new ProductSubCategory { ProductId = 3, SubCategoryId = 3 },
                new ProductSubCategory { ProductId = 4, SubCategoryId = 8 },
                new ProductSubCategory { ProductId = 5, SubCategoryId = 4 },
                new ProductSubCategory { ProductId = 6, SubCategoryId = 6 },
                new ProductSubCategory { ProductId = 7, SubCategoryId = 7 },
                new ProductSubCategory { ProductId = 8, SubCategoryId = 1 },
                new ProductSubCategory { ProductId = 9, SubCategoryId = 4 }

            );

            string subCategorySeedDataLocation = seedDataLocationOptions.SubCategorySeedData;
            List<SubCategory> subCategories = ModelBuilderExtension.GetSeedDataOf<SubCategory>(currDirectory, subCategorySeedDataLocation);

            modelBuilder.Entity<SubCategory>().HasData(subCategories);

            modelBuilder.Entity<CategorySubCategory>().HasData(
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 1 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 2 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 3 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 4 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 5 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 6 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 7 },
                new CategorySubCategory { CategoryId = 1, SubCategoryId = 8 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 1 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 2 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 3 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 6 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 7 },
                new CategorySubCategory { CategoryId = 2, SubCategoryId = 9 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 1 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 2 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 3 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 4 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 6 },
                new CategorySubCategory { CategoryId = 3, SubCategoryId = 9 }
            );


            string requirementsSeedDataLocation = seedDataLocationOptions.RequirementsSeedData;
            List<Requirements> requirements = ModelBuilderExtension.GetSeedDataOf<Requirements>(currDirectory, requirementsSeedDataLocation);

            modelBuilder.Entity<Requirements>().HasData(requirements);




            string photoSeedDataLocation = seedDataLocationOptions.PhotoSeedData;
            List<Picture> photos = ModelBuilderExtension.GetSeedDataOf<Picture>(currDirectory, photoSeedDataLocation);

            modelBuilder.Entity<Picture>().HasData(photos);


            string deliveryOptSeedDataLocation = seedDataLocationOptions.DeliveryOptSeedData;
            List<DeliveryOpt> deliveryOpts = ModelBuilderExtension.GetSeedDataOf<DeliveryOpt>(currDirectory, deliveryOptSeedDataLocation);

            modelBuilder.Entity<DeliveryOpt>().HasData(deliveryOpts);
        }

        public static List<T> GetSeedDataOf<T>(string currDirectory, string seedDataLocationSource)
        {
            string seedDataAsJson;
            List<T> seedToReturn = new List<T>();
            if (!currDirectory.Contains("GameShopStore"))
            {
                var testProjectDirectory = Directory.GetParent(currDirectory).Parent.Parent.Parent.FullName;
                var combined = Path.GetFullPath(Path.Combine(testProjectDirectory, seedDataLocationSource));

                seedDataAsJson = System.IO.File.ReadAllText(combined);
                seedToReturn = JsonConvert.DeserializeObject<List<T>>(seedDataAsJson);
            }
            else
            {
                seedDataAsJson = System.IO.File.ReadAllText("../" + seedDataLocationSource);
                seedToReturn = JsonConvert.DeserializeObject<List<T>>(seedDataAsJson);
            }
            return seedToReturn;
        }
    }
}
