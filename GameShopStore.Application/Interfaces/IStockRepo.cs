using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.StockDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IStockRepo : IBaseRepository<Stock>
    {
        Task<Stock> GetByProductId(int productId);
        Task<List<StockProductBasketDto>> GetStockWithProductForBasket(List<ProductFromBasketCookieDto> basketCookie);
        Task<Stock> FindWithProductAsync(Expression<Func<Stock, bool>> expression);
    }
}
