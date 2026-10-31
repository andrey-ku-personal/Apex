using Microsoft.AspNetCore.Identity;

namespace Apex.Users.Domain.Entities;

public class UserRole : IdentityRole
{
    public const string Admin = "Admin";
    public const string Investor = "Investor";
}
