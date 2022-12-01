using GameShopStore.Core.Dtos.RequirementsDtos;
using GameShopStore.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace GameShopStore.Core.Dtos.ProductDtos
{
    public class EditingProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public byte Pegi { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool IsDigitalMedia { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public EditingRequirementsDto Requirements { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public ICollection<int> LanguagesId { get; set; }
        public ICollection<int> SubCategoriesId { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}
