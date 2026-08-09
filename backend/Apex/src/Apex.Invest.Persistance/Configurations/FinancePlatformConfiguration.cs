using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class FinancePlatformConfiguration : IEntityTypeConfiguration<FinancePlatform>
{
    public void Configure(EntityTypeBuilder<FinancePlatform> builder)
    {
        builder.ToTable(@"FinancePlatform");

        builder.HasData(
            new FinancePlatform { Id = 1, Name = "Аигенис" },
            new FinancePlatform { Id = 2, Name = "Беларус Банк" },
            new FinancePlatform { Id = 3, Name = "Альфа Банк" }
        );

        builder.HasKey(k => k.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName(@"Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(p => p.Name)
            .HasColumnName(@"Name")
            .HasColumnType("character varying(128)")
            .HasMaxLength(128)
            .IsRequired();

        builder
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}
