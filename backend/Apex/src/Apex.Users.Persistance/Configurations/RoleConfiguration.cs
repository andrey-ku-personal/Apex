using Apex.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Users.Persistance.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Id).HasMaxLength(32);

        builder.HasData(
            new UserRole
            {
                Id = "admin",
                Name = UserRole.Admin,
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "role-admin"
            },
            new UserRole
            {
                Id = "investor",
                Name = UserRole.Investor,
                NormalizedName = "INVESTOR",
                ConcurrencyStamp = "role-investor"
            });
    }
}
