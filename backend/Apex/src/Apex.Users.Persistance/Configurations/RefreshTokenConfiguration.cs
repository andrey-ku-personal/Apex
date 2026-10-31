using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Users.Persistance.Configurations.RefreshToken;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<Domain.Entities.RefreshToken>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.Property(x => x.UserId).HasMaxLength(64);
        builder.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();

        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => x.ClosedAt);
        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
