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
    public class LanguageConfig : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
