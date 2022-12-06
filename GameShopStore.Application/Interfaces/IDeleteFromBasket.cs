using GameShopStore.Core.Dtos.BasketDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IDeleteFromBasket
    {
        Task<bool> Do(ISession session, List<ProductFromBasketCookieDto> basketCookie, int stockIdToDelete);
    }
}
