using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations.Bond;

public class BondOperationConfiguration : IEntityTypeConfiguration<Domain.Entities.Bond.BondOperation>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Bond.BondOperation> builder)
    {
        builder.ToTable("BondOperation");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(p => p.BondId)
            .HasColumnName("BondId")
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasColumnName("Date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnName("Price")
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(p => p.Count)
            .HasColumnName("Count")
            .IsRequired();

        builder.HasOne(p => p.Bond)
            .WithMany(b => b.Operations)
            .HasForeignKey(p => p.BondId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}