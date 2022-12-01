using GameShopStore.Core.Dtos.RequirementsDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.ProductDtos
{
    public class ProductCreationDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        public byte Pegi { get; set; }
        [Required]
        [Range(0, 9999999.99)]
        public decimal Price { get; set; }
        [Required]
        public bool IsDigitalMedia { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public CreationRequirementsDto Requirements { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int[] LanguagesId { get; set; }
        [Required]
        public int[] SubCategoriesId { get; set; }
    }
}
