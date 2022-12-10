using GameShopStore.Core.Dtos.StockDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.BasketDtos
{
    public class BasketDto
    {
        public decimal PriceBasket { get; set; }
        public List<StockProductBasketDto> ProdcutsBasket { get; set; } = null!;
    }
}
