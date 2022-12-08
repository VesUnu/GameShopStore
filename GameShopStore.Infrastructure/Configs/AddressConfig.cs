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
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.SurName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Street)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.PostCode)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(40);

            builder.Property(x => x.Country)
                .HasMaxLength(40);
        }
    }
}
