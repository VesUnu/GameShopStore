using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.RequirementsDtos
{
    public class EditingRequirementsDto
    {
        [StringLength(30)]
        public string OS { get; set; } = null!;
        [StringLength(100)]
        public string Processor { get; set; } = null!;
        public byte RAM { get; set; }
        [StringLength(100)]
        public string GraphicsCard { get; set; } = null!;
        public ushort HDD { get; set; }
        public bool IsNetworkConnectionRequire { get; set; }
    }
}
