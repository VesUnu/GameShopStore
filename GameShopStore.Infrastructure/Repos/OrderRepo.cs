using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.OrderDtos;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class OrderRepo : BaseRepo<Order>, IOrderRepo
    {
        public OrderRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {

        }

        public Order ScaffoldOrderForCreation(OrderInfoDto customerInfo, BasketPaymentDto basketPaymentDto) 
        {
            var result = new Order()
            {
                OrderRef = Guid.NewGuid(),
                Name = customerInfo.Name,
                SurName = customerInfo.Surname,
                Street = customerInfo.Street,
                PostCode = customerInfo.PostCode,
                Phone = customerInfo.Phone,
                Email = customerInfo.Email,
                City = customerInfo.City,
                Country = customerInfo.Country,
                DeliveryType = customerInfo.DeliveryType,
                OrderPrice = basketPaymentDto.BasketPrice,
                StripeRef = basketPaymentDto.StripeRef,
                OrderStocks = basketPaymentDto.ProductsBasket.Select(x => new OrderStock()
                {
                    StockId = x.StockId,
                    Quantity = x.StockQty,
                    Price = _appDbCtx.Stocks.Include(s => s.Product).FirstOrDefault(s => s.Id == x.StockId).Product.Price
                }).ToList()

            };

            return result;
        }
    }
}
