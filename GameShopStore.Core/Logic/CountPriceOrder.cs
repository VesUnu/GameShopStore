using GameShopStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Logic
{
    public class CountPriceOrder : ICountOrderPrice
    {
        public decimal Do<T>(List<T> productsForBasketDto) where T : IOrderLogisticInfo
        {
            decimal basketPrice = 0;
            foreach (var product in productsForBasketDto)
            {
                basketPrice += product.Price * product.StockQty;
            }

            return basketPrice;
        }
    }
}
