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
    public class DeliveryOptConfig : IEntityTypeConfiguration<DeliveryOpt>
    {
        public void Configure(EntityTypeBuilder<DeliveryOpt> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(9,2)")
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(400);
        }
    }
}
