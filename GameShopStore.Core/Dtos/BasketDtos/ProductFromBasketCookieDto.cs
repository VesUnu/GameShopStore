using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.BasketDtos
{
    public class ProductFromBasketCookieDto
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int StockQty { get; set; }
    }
}
