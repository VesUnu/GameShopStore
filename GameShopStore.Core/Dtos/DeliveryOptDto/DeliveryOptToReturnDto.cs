using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.DeliveryOptDto
{
    public class DeliveryOptToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
