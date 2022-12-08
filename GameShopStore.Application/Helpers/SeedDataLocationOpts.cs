using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Helpers
{
    public class SeedDataLocationOpts
    {
        public const string SeedDataLoc = "SeedDataLocation";

        public string PhotoSeedData { get; set; }
        public string ProductSeedData { get; set; }
        public string RequirementsSeedData { get; set; }
        public string SubCategorySeedData { get; set; }
        public string UserSeedData { get; set; }
        public string DeliveryOptSeedData { get; set; }
    }
}
