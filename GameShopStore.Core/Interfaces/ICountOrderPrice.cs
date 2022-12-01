using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShopStore.Core.Interfaces;

namespace GameShopStore.Core.Interfaces
{
    public interface ICountOrderPrice
    {
        decimal Do<T>(List<T> productsForBasketDto) where T : IOrderLogisticInfo;
    }
}
