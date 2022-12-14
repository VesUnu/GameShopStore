using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Models
{
    public class Requirements
    {
        public int Id { get; set; }
        public string OS { get; set; } = null!;
        public string Processor { get; set; } = null!;
        public byte RAM { get; set; }
        public string GraphicsCard { get; set; } = null!;
        public ushort HDD { get; set; }
        public bool IsNetworkConnectionRequire { get; set; }
        public virtual Product Product { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
