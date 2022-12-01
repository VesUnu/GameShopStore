using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Interfaces
{
    public interface IOrderLogisticInfo
    {
        int ProductId { get; set; }
        int StockId { get; set; }
        int StockQty { get; set; }
        decimal Price { get; set; }
    }
}
