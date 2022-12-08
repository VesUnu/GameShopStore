using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.StockDtos;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class StockRepository : BaseRepo<Stock>, IStockRepo
    {
        public StockRepository(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {
        }

        public async Task<Stock> FindWithProductAsync(Expression<Func<Stock, bool>> expression)
        {
            return await _appDbCtx.Stocks.Include(s => s.Product).Where(expression).FirstOrDefaultAsync();
        }

        public async Task<Stock> GetByProductId(int productId)
        {
            return await _appDbCtx.Stocks.Where(s => s.ProductId == productId)
                                .FirstOrDefaultAsync();
        }

        public async Task<List<StockProductBasketDto>> GetStockWithProductForBasket(List<ProductFromBasketCookieDto> basketCookie)
        {
            var stockIds = basketCookie.Select(bc => bc.StockId).ToList();
            var result = await _appDbCtx.Stocks.Where(s => stockIds.Contains(s.Id)).Include(s => s.Product).Select(s => new StockProductBasketDto()
            {
                ProductId = s.ProductId,
                StockId = s.Id,
                Name = s.Product.Name,
                CategoryName = _appDbCtx.Categories.FirstOrDefault(c => c.Id == EF.Property<int>(s.Product, "CategoryId")).Name,
                Price = s.Product.Price
            }).ToListAsync();
            foreach (var basketProduct in basketCookie)
            {
                result.FirstOrDefault(s => s.StockId == basketProduct.StockId).StockQty = basketProduct.StockQty;
            }

            return result;
        }
    }
}
