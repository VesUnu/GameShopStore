using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Helpers
{
    public class BasketSettings
    {
        public int CookieDaysExpired { get; set; }
        public int StockOnHoldMinutesExpired { get; set; }
    }
}
