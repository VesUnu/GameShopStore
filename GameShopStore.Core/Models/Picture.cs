using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public bool isMain { get; set; }
        public string PublicId { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
