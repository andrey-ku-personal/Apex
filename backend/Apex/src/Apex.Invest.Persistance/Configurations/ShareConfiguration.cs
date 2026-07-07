using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class ShareConfiguration : IEntityTypeConfiguration<Share>
{
    public void Configure(EntityTypeBuilder<Share> builder)
    {
        builder.ToTable(@"Share");

        builder.Property(p => p.Currency)
            .HasColumnName(@"Currency")
            .HasColumnType("int")
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

        builder.Property(p => p.DividendRate)
            .HasColumnName(@"DividendRate")
            .HasColumnType("decimal")
            .HasPrecision(6, 2)
            .IsRequired();
    }
}
