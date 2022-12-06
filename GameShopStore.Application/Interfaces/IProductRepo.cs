using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Core.Dtos.RequirementsDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IProductRepo : IBaseRepository<Product>
    {
        Task<PagedList<ProductModerationDto>> GetProductsForModerationAsync(ProductParams productParams);
        Task<PagedList<ProductSearchDto>> GetProductsForSearchingAsync(ProductParams productParams);
        Task<Product> ScaffoldProductForCreationAsync(ProductCreationDto productForCreationDto, Requirements requirements, Category selectedCategory);
        Task<Product> ScaffoldProductForEditAsync(int id, ProductEditDto productToEditDto, Requirements requirements, Category selectedCategory, Product productFromDb);
        Task<Product> GetWithPicturesOnly(int productId);
        Task<EditingProductDto> GetProductToEditAsync(EditingRequirementsDto requirements, IEnumerable<Picture> photosFromRepo, int id);
        Task<Product> GetWithStockOnlyAsync(int productId);
        Task<List<ProductForBasketDto>> GetProductsForBasketAsync(List<ProductFromBasketCookieDto> basketCookie);
        Task<PagedList<ProductStockModerationDto>> GetProductsForStockModeration(ProductParams productParams);
    }
}
