using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Portfolio.Persistance.Configurations.Bond;

public class BondOperationConfiguration : IEntityTypeConfiguration<Domain.Entities.Bond.BondOperation>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Bond.BondOperation> builder)
    {
        builder.ToTable("BondOperations");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(p => p.BondId)
            .HasColumnName("BondId")
            .IsRequired();

        builder.Property(p => p.UserId)
            .HasColumnName("UserId")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasColumnName("Date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.Count)
            .HasColumnName("Count")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnName("Price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Commission)
            .HasColumnName("Commission")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(p => p.Bond)
            .WithMany()
            .HasForeignKey(p => p.BondId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.BondId, x.Date });
    }
}
