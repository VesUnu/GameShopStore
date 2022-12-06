using GameShopStore.Core.Enums;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface ITransferStockToBeOnHold
    {
        Task<StockOnHold> Do(StockTransferToOnHoldStockTypeEnum transferType, ISession session, Stock stockToSubtract, int stockQty);
    }
}
