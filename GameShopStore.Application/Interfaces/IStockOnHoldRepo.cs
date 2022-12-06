using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.StockDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IStockOnHoldRepo : IBaseRepository<StockOnHold>
    {
        Task<List<StockOnHoldProductCountPriceDto>> GetStockOnHoldWithProductForCharge(ISession session, List<ProductFromBasketCookieDto> basketProductsCookie);
        Task<bool> SetExpiredForStocksOnHoldAsync(ISession session);
    }
}
