using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.OrderDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IOrderRepo : IBaseRepository<Order>
    {
        public Order ScaffoldOrderForCreation(OrderInfoDto customerInfo, BasketPaymentDto basketPaymentDto);
    }
}
