using Apex.Portfolio.Domain.Entities.InstrumentMetadata;
using Apex.Portfolio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Portfolio.Persistance.Configurations.InstrumentMetadata;

public class InstrumentMetadataConfiguration : IEntityTypeConfiguration<Domain.Abstractions.InstrumentMetadata>
{
    public void Configure(EntityTypeBuilder<Domain.Abstractions.InstrumentMetadata> builder)
    {
        builder.ToTable("InstrumentMetadata");

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

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .HasColumnType("smallint")
            .IsRequired();

        builder.HasDiscriminator(p => p.Type)
            .HasValue<Currency>(MetadataType.Currency)
            .HasValue<PaymentFrequency>(MetadataType.PaymentFrequency);

        builder.HasIndex(p => new { p.Type, p.Name })
            .IsUnique();
    }
}