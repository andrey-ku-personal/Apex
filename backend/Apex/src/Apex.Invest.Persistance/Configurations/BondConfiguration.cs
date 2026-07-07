using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class BondConfiguration : IEntityTypeConfiguration<Bond>
{
    public void Configure(EntityTypeBuilder<Bond> builder)
    {
        builder.ToTable(@"Bond");

        builder.Property(p => p.InterestRate)
            .HasColumnName(@"InterestRate")
            .HasColumnType("decimal")
            .HasPrecision(6, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .HasColumnName(@"Currency")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.Frequence)
            .HasColumnName(@"Frequence")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.NominalPrice)
            .HasColumnName(@"NominalPrice")
            .HasColumnType("decimal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.MarketPrice)
            .HasColumnName(@"MarketPrice")
            .HasColumnType("decimal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .HasColumnName(@"Quantity")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.BrokerCommission)
            .HasColumnName(@"BrokerCommission")
            .HasColumnType("decimal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.FirstPaymentDate)
            .HasColumnName(@"FirstPaymentDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.MaturityDate)
            .HasColumnName(@"MaturityDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.SaleDate)
            .HasColumnName(@"SaleDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
    }
}
