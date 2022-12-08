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
    public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(sc => sc.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
