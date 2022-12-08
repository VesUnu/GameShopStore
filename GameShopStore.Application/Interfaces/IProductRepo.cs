using GameShopStore.Application.Helpers;
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
        Task<PagedList<ProductModerationDto>> GetProductsForModerationAsync(ProductParameters productParameters);
        Task<PagedList<ProductSearchDto>> GetProductsForSearchingAsync(ProductParameters productParameters);
        Task<Product> ScaffoldProductForCreationAsync(ProductCreationDto productCreationDto, Requirements requirements, Category selectedCategory);
        Task<Product> ScaffoldProductForEditAsync(int id, ProductEditDto productToEditDto, Requirements requirements, Category selectedCategory, Product productFromDb);
        Task<Product> GetWithPicturesOnly(int productId);
        Task<EditingProductDto> GetProductToEditAsync(EditingRequirementsDto requirements, IEnumerable<Picture> picturesFromRepo, int id);
        Task<Product> GetWithStockOnlyAsync(int productId);
        Task<List<ProductForBasketDto>> GetProductsForBasketAsync(List<ProductFromBasketCookieDto> basketCookie);
        Task<PagedList<ProductStockModerationDto>> GetProductsForStockModeration(ProductParameters productParameters);
    }
}
