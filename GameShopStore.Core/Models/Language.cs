using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<ProductLanguage> Products { get; set; } = null!;
    }
}
