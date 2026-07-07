using Apex.Invest.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Invest.Persistance.Extensions;

public static class Configurations
{
    public static void AddAnalyticalEntityColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : AnalyticalEntity
    {
        builder.Property(x => x.CreatedAt)
            .HasColumnName(@"CreatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}
