using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte Pegi { get; set; }
        public decimal Price { get; set; }
        public bool IsDigitalMedia { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual Requirements Requirements { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ProductLanguage> Languages { get; set; } = null!;
        public virtual ICollection<ProductSubCategory> SubCategories { get; set; } = null!;
        public virtual ICollection<Picture> Pictures { get; set; } = null!;
        public Stock Stock { get; set; } = null!;
    }
}
