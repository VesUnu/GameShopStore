using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IAddStockBasket
    {
        void Do(ISession session, StockOnHold stockToUpdate);
    }
}
