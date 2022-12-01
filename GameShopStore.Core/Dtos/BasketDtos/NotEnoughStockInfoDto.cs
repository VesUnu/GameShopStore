using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.BasketDtos
{
    public class NotEnoughStockInfoDto
    {
        public int StockId { get; set; }
        public int AvailableStockQty { get; set; }
        public string ProductName { get; set; }
    }
}
