using System.ComponentModel.DataAnnotations;

namespace GameShopStore.Core.Dtos.AddressDtos
{
    public class UserAddressInfoForCreationDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [Required]
        [StringLength(80)]
        public string Street { get; set; }

        [Required]
        [StringLength(12)]
        public string PostCode { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(40)]
        public string Country { get; set; }
    }
}
