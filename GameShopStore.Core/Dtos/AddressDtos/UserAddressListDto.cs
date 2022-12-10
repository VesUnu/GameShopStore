using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.AddressDtos
{
    public class UserAddressListDto
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string PostCode { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;
    }
}
