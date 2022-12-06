using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IProductToBasket
    {
        void Do(ISession session, StockOnHold stockUpdated);
    }
}
