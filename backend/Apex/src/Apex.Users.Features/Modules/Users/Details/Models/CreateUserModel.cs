using Apex.Users.Domain.Entities;

namespace Apex.Users.Features.Modules.Users.Details.Models;

public record CreateUserModel
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public IReadOnlyCollection<string> Roles { get; init; } = [UserRole.Investor];
}
