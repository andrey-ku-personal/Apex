using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Portfolio.Persistance.Configurations.Bond;

public class BondConfiguration : IEntityTypeConfiguration<Domain.Entities.Bond.Bond>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Bond.Bond> builder)
    {
        builder.ToTable("Bond");

        builder.Property(p => p.CurrencyId)
            .HasColumnName("CurrencyId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.PaymentFrequencyId)
            .HasColumnName("PaymentFrequencyId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.IssueDate)
            .HasColumnName("IssueDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.MaturityDate)
            .HasColumnName("MaturityDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.PaymentDay)
            .HasColumnName("PaymentDay")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.ParPrice)
            .HasColumnName("ParPrice")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Rate)
            .HasColumnName("Rate")
            .HasPrecision(9, 4)
            .IsRequired();

        builder.Property(p => p.IsFloating)
            .HasColumnName("IsFloating")
            .HasColumnType("boolean")
            .IsRequired();

        builder.HasOne(p => p.Currency)
            .WithMany()
            .HasForeignKey(k => k.CurrencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PaymentFrequency)
            .WithMany()
            .HasForeignKey(k => k.PaymentFrequencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}