using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class ProductLanguage
    {
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual Language Language { get; set; }
        public int LanguageId { get; set; }
    }
}
