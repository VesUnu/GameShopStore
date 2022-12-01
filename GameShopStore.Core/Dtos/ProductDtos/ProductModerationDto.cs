using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.ProductDtos
{
    public class ProductModerationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<string> SubCategories { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CategoryName { get; set; }
    }
}
