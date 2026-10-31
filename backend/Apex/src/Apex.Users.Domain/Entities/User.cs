using Apex.Shared.Core;
using Microsoft.AspNetCore.Identity;

namespace Apex.Users.Domain.Entities;

public class User : IdentityUser, IAnalytical
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}