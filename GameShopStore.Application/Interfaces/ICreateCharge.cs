using GameShopStore.Core.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface ICreateCharge
    {
        BasketPaymentDto Do(string stripeToken, decimal basketPrice, List<ProductFromBasketCookieDto> basketProductsCookie);
    }
}
