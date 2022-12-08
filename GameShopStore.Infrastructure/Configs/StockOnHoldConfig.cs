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
    public class StockOnHoldConfig : IEntityTypeConfiguration<StockOnHold>
    {
        public void Configure(EntityTypeBuilder<StockOnHold> builder)
        {
            builder.Property(s => s.StockQty)
                    .IsRequired();

            builder.Property(s => s.SessionId)
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(s => s.ExpireTime)
                    .IsRequired();
        }
    }
}
