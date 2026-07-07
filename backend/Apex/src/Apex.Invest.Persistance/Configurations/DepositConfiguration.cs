using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder.ToTable(@"Deposit");

        builder.Property(p => p.InterestRate)
            .HasColumnName(@"InterestRate")
            .HasColumnType("decimal")
            .HasPrecision(6, 2)
            .IsRequired();

        builder.Property(p => p.Frequence)
            .HasColumnName(@"Frequence")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.IsReplenishable)
            .HasColumnName(@"IsReplenishable")
            .HasColumnType("boolean")
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

        builder.Property(p => p.IsTaxFree)
            .HasColumnName(@"IsTaxFree")
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(p => p.Capitalization)
            .HasColumnName(@"Capitalization")
            .HasColumnType("boolean")
            .IsRequired();
    }
}
