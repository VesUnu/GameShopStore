﻿using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.ProductDtos
{
    public class ProductSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Picture Picture { get; set; }
        public string CategoryName { get; set; }
    }
}
