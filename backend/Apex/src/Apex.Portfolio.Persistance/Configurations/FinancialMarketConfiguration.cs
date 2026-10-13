using Apex.Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Portfolio.Persistance.Configurations;

public class FinancialMarketConfiguration : IEntityTypeConfiguration<FinancialMarket>
{
    public void Configure(EntityTypeBuilder<FinancialMarket> builder)
    {
        builder.ToTable("FinancialMarket");

        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasColumnType("character varying(128)")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(p => p.RefinancingRate)
            .HasColumnName("RefinancingRate")
            .HasPrecision(9, 4)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}