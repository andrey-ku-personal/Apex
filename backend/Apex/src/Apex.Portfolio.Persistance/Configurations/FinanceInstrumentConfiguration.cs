using Apex.Portfolio.Domain.Abstractions;
using Apex.Portfolio.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Portfolio.Persistance.Configurations;

public class FinanceInstrumentConfiguration : IEntityTypeConfiguration<FinanceInstrument>
{
    public void Configure(EntityTypeBuilder<FinanceInstrument> builder)
    {
        builder.ToTable("FinanceInstrument");

        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.PlatformId)
            .HasColumnName("PlatformId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.Ticker)
            .HasColumnName("Ticker")
            .HasColumnType("character varying(64)")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Issuer)
            .HasColumnName("Issuer")
            .HasColumnType("character varying(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.AddAnalyticalEntityColumns();
    }
}
