using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations.Bond;

public class BondConfiguration : IEntityTypeConfiguration<Domain.Entities.Bond.Bond>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Bond.Bond> builder)
    {
        builder.ToTable("Bond");

        builder.Property(p => p.Currency)
            .HasColumnName("Currency")
            .IsRequired();

        builder.Property(p => p.PaymentFrequence)
            .HasColumnName("PaymentFrequence")
            .IsRequired();

        builder.Property(p => p.ParPrice)
            .HasColumnName("ParPrice")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.CouponRate)
            .HasColumnName("CouponRate")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(p => p.NextCouponDate)
            .HasColumnName("NextCouponDate")
            .IsRequired();

        builder.Property(p => p.MaturityDate)
            .HasColumnName("MaturityDate")
            .HasColumnType("date")
            .IsRequired();
    }
}
