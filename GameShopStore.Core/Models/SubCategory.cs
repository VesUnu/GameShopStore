using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<CategorySubCategory> Categories { get; set; } = null!;
        public virtual ICollection<ProductSubCategory> Products { get; set; } = null!;
    }
}
