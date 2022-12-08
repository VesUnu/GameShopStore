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
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.Pegi)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(9,2)");
        }
    }
}
