using GameShopStore.Core.Dtos.BasketDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface ISynchronizeBasket
    {
        Task<bool> Do(ISession session, List<ProductFromBasketCookieDto> basketCookie);
        List<NotEnoughStockInfoDto> MissingStocks { get; }
    }
}
