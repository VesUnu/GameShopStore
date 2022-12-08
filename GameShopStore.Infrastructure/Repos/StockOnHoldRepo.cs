using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.StockDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class StockOnHoldRepo : BaseRepo<StockOnHold>, IStockOnHoldRepo
    {
        public StockOnHoldRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {
        }

        public async Task<List<StockOnHoldProductCountPriceDto>> GetStockOnHoldWithProductForCharge(ISession session, List<ProductFromBasketCookieDto> basketProductsCookie)
        {
            var stockIds = basketProductsCookie.Select(s => s.StockId).ToList();

            var result = await _appDbCtx.StockOnHolds.Where(s => stockIds.Contains(s.StockId) && s.SessionId == session.Id).Select(s => new StockOnHoldProductCountPriceDto()
            {
                StockId = s.StockId,
                ProductId = s.ProductId,
                Price = s.Stock.Product.Price,
                StockQty = s.StockQty
            }).ToListAsync();


            return result;
        }

        public async Task<bool> SetExpiredForStocksOnHoldAsync(ISession session)
        {
            var stocksExpire = await _appDbCtx.StockOnHolds.Where(s => s.SessionId.Equals(session.Id)).ToListAsync();

            if (stocksExpire.Any())
            {
                stocksExpire.ForEach(s => s.ExpireTime = DateTime.Now.AddYears(-1));
                return true;
            }

            return false;
        }
    }
}
