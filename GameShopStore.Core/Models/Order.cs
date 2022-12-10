using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string DeliveryType { get; set; } = null!;

        public Guid OrderRef { get; set; }
        public string StripeRef { get; set; } = null!;
        public decimal OrderPrice { get; set; }

        public ICollection<OrderStock> OrderStocks { get; set; } = null!;
    }
}
