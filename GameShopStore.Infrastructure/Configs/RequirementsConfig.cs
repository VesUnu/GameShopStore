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
    public class RequirementsConfig : IEntityTypeConfiguration<Requirements>
    {
        public void Configure(EntityTypeBuilder<Requirements> builder)
        {
            builder.Property(r => r.OS)
                .HasMaxLength(30);

            builder.Property(r => r.Processor)
                .HasMaxLength(100);

            builder.Property(r => r.HDD)
                .IsRequired();

            builder.Property(r => r.GraphicsCard)
                .HasMaxLength(100);

            builder.Property(r => r.IsNetworkConnectionRequire)
                .IsRequired();
        }
    }
}
