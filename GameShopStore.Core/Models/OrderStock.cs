using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class OrderStock
    {
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
