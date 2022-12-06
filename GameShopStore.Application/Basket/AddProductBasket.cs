using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Basket
{
    public class AddProductBasket : IAddStockBasket
    {
        public AddProductBasket()
        {

        }

        public void Do(ISession session, StockOnHold stockToUpdate)
        {
            var basketJson = session.GetString("Basket");
            var basket = new List<ProductFromBasketCookieDto>();

            if (!string.IsNullOrEmpty(basketJson))
            {
                basket = JsonConvert.DeserializeObject<List<ProductFromBasketCookieDto>>(basketJson);
            }
            if (basket.Any(x => x.StockId == stockToUpdate.StockId))
            {
                basket.FirstOrDefault(x => x.StockId == stockToUpdate.StockId).StockQty = stockToUpdate.StockQty;
            }
            else
            {
                var basketInfoForProduct = new ProductFromBasketCookieDto
                {
                    StockId = stockToUpdate.StockId,
                    ProductId = stockToUpdate.ProductId,
                    StockQty = stockToUpdate.StockQty
                };
                basket.Add(basketInfoForProduct);

            }

            basketJson = JsonConvert.SerializeObject(basket);
            session.SetString("Basket", basketJson);

        }
    }
}
