using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.BasketDtos
{
    public class BasketPaymentDto
    {
        public decimal BasketPrice { get; set; }
        public string StripeRef { get; set; }
        public List<ProductForBasketDto> ProductsBasket { get; set; }
    }
}
