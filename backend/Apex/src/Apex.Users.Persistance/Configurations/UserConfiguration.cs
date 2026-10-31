using Apex.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Users.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Id).HasMaxLength(64);

        builder.Property(x => x.FirstName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(64).IsRequired();

        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.FirstName).HasMethod("gin").HasOperators("gin_trgm_ops");
        builder.HasIndex(x => x.LastName).HasMethod("gin").HasOperators("gin_trgm_ops");

        builder.HasIndex(x => x.NormalizedEmail).IsUnique();
    }
}
