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
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Street { get; set; }

        public string PostCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
