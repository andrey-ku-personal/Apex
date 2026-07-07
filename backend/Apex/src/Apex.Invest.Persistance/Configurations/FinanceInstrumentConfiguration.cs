using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class FinanceInstrumentConfiguration : IEntityTypeConfiguration<FinanceInstrument>
{
    public void Configure(EntityTypeBuilder<FinanceInstrument> builder)
    {
        builder.ToTable(@"FinanceInstrument");

        builder.HasKey(k => k.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName(@"Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.PlatformId)
            .HasColumnName(@"PlatformId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.Ticker)
            .HasColumnName(@"Ticker")
            .HasColumnType("character varying(64)")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Issuer)
            .HasColumnName(@"Issuer")
            .HasColumnType("character varying(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.PurchaseDate)
            .HasColumnName(@"PurchaseDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(p => p.Platform)
            .WithMany()
            .HasForeignKey(k => k.PlatformId)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName(@"Status")
            .HasColumnType("int")
            .IsRequired();
    }
}
