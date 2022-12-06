﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface ITransferStockOnHoldWhenExpired
    {
        Task<bool> Do();
    }
}
