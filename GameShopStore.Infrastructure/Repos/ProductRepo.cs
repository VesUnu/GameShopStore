using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Core.Dtos.RequirementsDtos;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class ProductRepo : BaseRepo<Product>, IProductRepo
    {
        public ProductRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {

        }

        public async Task<PagedList<ProductModerationDto>> GetProductsForModerationAsync(ProductParameters productParameters)
        {
            var products = _appDbCtx.Products.OrderBy(x => x.Id)
                .Select(product => new ProductModerationDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ReleaseDate = product.ReleaseDate,
                    SubCategories = (from productSubCategory in product.SubCategories
                                     join subCategory in _appDbCtx.SubCategories
                                     on productSubCategory.SubCategoryId
                                     equals subCategory.Id
                                     select subCategory.Name).ToList(),
                    CategoryName = _appDbCtx.Categories.FirstOrDefault(x => x.Id == EF.Property<int>(product, "CategoryId")).Name,
                });

            return await PagedList<ProductModerationDto>.CreateAsync(products, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task<PagedList<ProductSearchDto>> GetProductsForSearchingAsync(ProductParameters productParameters)
        {
            var products = _appDbCtx.Products.Select(product => new ProductSearchDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Picture = product.Pictures.Where(p => p.ProductId == product.Id).Select(p => new Picture
                {
                    Id = p.Id,
                    Url = p.Url,
                    DateAdded = p.DateAdded,
                    isMain = p.isMain
                }).FirstOrDefault(p => p.isMain == true),
                CategoryName = _appDbCtx.Categories.FirstOrDefault(c => c.Id == EF.Property<int>(product, "CategoryId")).Name
            });

            return await PagedList<ProductSearchDto>.CreateAsync(products, productParameters.PageNumber, productParameters.PageSize);
        }


        public async Task<Product> ScaffoldProductForCreationAsync(ProductCreationDto productCreationDto, Requirements requirements, Category selectedCategory)
        {
            if (productCreationDto == null)
            {
                throw new ArgumentException("Product that was passed to CreateAsync method is null");
            }

            var product = new Product
            {
                Name = productCreationDto.Name,
                Description = productCreationDto.Description,
                Pegi = productCreationDto.Pegi,
                Price = productCreationDto.Price,
                IsDigitalMedia = productCreationDto.IsDigitalMedia,
                ReleaseDate = productCreationDto.ReleaseDate,
                Category = selectedCategory,
                Requirements = requirements,
                Languages = new List<ProductLanguage>(),
                SubCategories = new List<ProductSubCategory>(),
                Stock = new Stock()
            };

            if (productCreationDto.LanguagesId != null)
            {
                foreach (var languageId in productCreationDto.LanguagesId)
                {

                    var selectedLanguages = await _appDbCtx.Languages.FirstOrDefaultAsync(l => l.Id == languageId);
                    var pl = new ProductLanguage { Product = product, Language = selectedLanguages };
                    product.Languages.Add(pl);

                }
            }

            if (productCreationDto.SubCategoriesId != null)
            {
                foreach (var subCategoryId in productCreationDto.SubCategoriesId)
                {
                    var selectedSubCategory = await _appDbCtx.SubCategories.FirstOrDefaultAsync(sc => sc.Id == subCategoryId);
                    var psc = new ProductSubCategory { Product = product, SubCategory = selectedSubCategory };
                    product.SubCategories.Add(psc);
                }
            }

            return product;
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await _appDbCtx.Products.Where(p => p.Id == id)
                                .Include(l => l.Languages)
                                    .ThenInclude(productLanguage => productLanguage.Language)
                                .Include(p => p.Pictures)
                                .Include(p => p.Category)
                                .Include(p => p.SubCategories)
                                    .ThenInclude(productSubCategory => productSubCategory.SubCategory)
                                .Include(p => p.Requirements)
                                .Include(s => s.Stock)
                            .FirstOrDefaultAsync();
        }

        public async Task<Product> GetWithStockOnlyAsync(int productId)
        {
            return await _appDbCtx.Products.Where(x => x.Id == productId)
                                .Include(s => s.Stock)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<ProductForBasketDto>> GetProductsForBasketAsync(List<ProductFromBasketCookieDto> basketCookie)
        {
            var productsIds = basketCookie.Select(b => b.ProductId).ToList();
            var productsToReturn = await _appDbCtx.Products.Where(p => productsIds.Contains(p.Id))
                            .Select(p => new ProductForBasketDto()
                            {
                                ProductId = p.Id,
                                Name = p.Name,
                                Price = p.Price,
                                CategoryName = _appDbCtx.Categories.FirstOrDefault(c => c.Id == EF.Property<int>(p, "CategoryId")).Name,
                            }).ToListAsync();
            foreach (var basketItem in basketCookie)
            {
                productsToReturn.FirstOrDefault(p => p.ProductId == basketItem.ProductId).StockQty = basketItem.StockQty;
                productsToReturn.FirstOrDefault(p => p.ProductId == basketItem.ProductId).StockId = basketItem.StockId;
            }
            return productsToReturn;
        }

        public async Task<PagedList<ProductStockModerationDto>> GetProductsForStockModeration(ProductParameters productParameters)
        {
            var products = _appDbCtx.Products.Include(s => s.Stock).Select(x => new ProductStockModerationDto()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = _appDbCtx.Categories.FirstOrDefault(c => c.Id == EF.Property<int>(x, "CategoryId")).Name,
                StockQuantity = x.Stock.Quantity
            });

            return await PagedList<ProductStockModerationDto>.CreateAsync(products, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task<Product> ScaffoldProductForEditAsync(int id, ProductEditDto productToEditDto, Requirements requirements, Category selectedCategory, Product productFromDb)
        {

            var updatedProduct = new Product
            {
                Id = id,
                Name = productToEditDto.Name,
                Description = productToEditDto.Description,
                Pegi = productToEditDto.Pegi,
                Price = productToEditDto.Price,
                IsDigitalMedia = productToEditDto.IsDigitalMedia,
                ReleaseDate = productToEditDto.ReleaseDate,
                Category = selectedCategory,
                Requirements = requirements,
                Languages = new List<ProductLanguage>(),
                SubCategories = new List<ProductSubCategory>()

            };

            foreach (var languageId in productToEditDto.LanguagesId)
            {

                var selectedLanguages = await _appDbCtx.Languages.FirstOrDefaultAsync(l => l.Id == languageId);

                var pl = new ProductLanguage { Product = updatedProduct, Language = selectedLanguages };
                if (!productFromDb.Languages.Contains(pl))
                {
                    updatedProduct.Languages.Add(pl);
                }

            }

            foreach (var subCategoryId in productToEditDto.SubCategoriesId)
            {
                var selectedSubCategory = await _appDbCtx.SubCategories.FirstOrDefaultAsync(sc => sc.Id == subCategoryId);
                var psc = new ProductSubCategory { Product = updatedProduct, SubCategory = selectedSubCategory };
                if (!productFromDb.SubCategories.Contains(psc))
                {
                    updatedProduct.SubCategories.Add(psc);
                }
            }

            return updatedProduct;
        }

        public async Task<Product> GetWithPicturesOnly(int productId)
        {
            var product = await _appDbCtx.Products
            .Include(p => p.Pictures)
            .FirstOrDefaultAsync(x => x.Id == productId);

            return product;
        }

        public async Task<EditingProductDto> GetProductToEditAsync(EditingRequirementsDto requirements, IEnumerable<Picture> picturesFromRepo, int productId)
        {
            var result = await _appDbCtx.Products.Where(x => x.Id == productId)
                .Select(product => new EditingProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Pegi = product.Pegi,
                    Price = product.Price,
                    ReleaseDate = product.ReleaseDate,
                    IsDigitalMedia = product.IsDigitalMedia,
                    SubCategoriesId = (from productSubCategory in product.SubCategories
                                       join subCategory in _appDbCtx.SubCategories
                                       on productSubCategory.SubCategoryId
                                       equals subCategory.Id
                                       select subCategory.Id).ToList(),
                    LanguagesId = (from productLangauge in product.Languages
                                   join language in _appDbCtx.Languages
                                   on productLangauge.LanguageId
                                   equals language.Id
                                   select language.Id).ToList(),
                    Requirements = requirements,
                    CategoryId = _appDbCtx.Categories.FirstOrDefault(c => c.Id == EF.Property<int>(product, "CategoryId")).Id,
                    Pictures = picturesFromRepo.ToList()
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
