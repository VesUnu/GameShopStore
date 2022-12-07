using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Stripe
{
    public class ChargeCreate : ICreateCharge
    {
        public BasketPaymentDto Do(string stripeToken, decimal basketPrice, List<ProductFromBasketCookieDto> basketProdsCookie)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (int)(basketPrice * 100),
                Description = "Test Desc",
                Currency = "usd",
                Customer = customer.Id,

            });

            var basketPaymentDto = new BasketPaymentDto()
            {
                BasketPrice = basketPrice,
                StripeRef = charge.Id,
                ProductsBasket = basketProdsCookie
            };

            return basketPaymentDto;
        }
    }
}
