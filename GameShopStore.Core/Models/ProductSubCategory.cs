using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class ProductSubCategory
    {
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }
    }
}
