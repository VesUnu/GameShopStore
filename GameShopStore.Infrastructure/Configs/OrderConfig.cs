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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(o => o.SurName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(o => o.Street)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(o => o.PostCode)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(o => o.Phone)
                .HasMaxLength(15);

            builder.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(o => o.City)
                .HasMaxLength(40);

            builder.Property(x => x.Country)
                .HasMaxLength(40);

            builder.Property(x => x.DeliveryType)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(o => o.OrderRef)
                .IsRequired();

            builder.Property(o => o.StripeRef)
                .HasMaxLength(35)
                .IsRequired();

            builder.Property(o => o.OrderPrice)
                 .HasColumnType("decimal(9,2)")
                 .IsRequired();
        }
    }
}
