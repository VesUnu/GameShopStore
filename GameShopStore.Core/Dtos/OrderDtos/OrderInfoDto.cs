using GameShopStore.Core.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.OrderDtos
{
    public class OrderInfoDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string Surname { get; set; } = null!;

        [Required]
        [StringLength(80)]
        public string Street { get; set; } = null!;

        [Required]
        [StringLength(12)]
        public string PostCode { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [StringLength(40)]
        public string City { get; set; } = null!;

        [StringLength(40)]
        public string Country { get; set; } = null!;

        [DeliveryType("DHL", "SPEEDY", "FedEx", "Econt")]
        public string DeliveryType { get; set; } = null!;
    }
}
