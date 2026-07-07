using Apex.Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Configurations;

public class DepositOperationConfiguration : IEntityTypeConfiguration<DepositOperation>
{
    public void Configure(EntityTypeBuilder<DepositOperation> builder)
    {
        builder
            .ToTable(@"DepositOperation")
            .HasKey(k => k.Id)
            .HasName("PK_DepositOperation");

        builder
            .Property(p => p.Id)
            .HasColumnName(@"Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.DepositId)
            .HasColumnName(@"DepositId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasColumnName(@"Date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasColumnName(@"Amount")
            .HasColumnType("decimal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName(@"Type")
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(p => p.Deposit)
            .WithMany(d => d.Operations)
            .HasForeignKey(k => k.DepositId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
