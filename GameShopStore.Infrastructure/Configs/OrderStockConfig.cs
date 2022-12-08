using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Configs
{
    public class OrderStockConfig : IEntityTypeConfiguration<OrderStock>
    {
        public void Configure(EntityTypeBuilder<OrderStock> builder)
        {
            builder.Property(x => x.Quantity)
                    .IsRequired();

            builder.Property(x => x.Price)
                    .HasColumnType("decimal(9,2)")
                    .IsRequired();
        }
    }
}
