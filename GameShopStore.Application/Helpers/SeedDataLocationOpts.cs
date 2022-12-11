using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Helpers
{
    public class SeedDataLocationOpts
    {
        public const string SeedDataLocation = "SeedDataLocation";

        public string PictureSeedData { get; set; } = null!;
        public string ProductSeedData { get; set; } = null!;
        public string RequirementsSeedData { get; set; } = null!;
        public string SubCategorySeedData { get; set; } = null!;
        public string UserSeedData { get; set; } = null!;
        public string DeliveryOptSeedData { get; set; } = null!;
    }
}
