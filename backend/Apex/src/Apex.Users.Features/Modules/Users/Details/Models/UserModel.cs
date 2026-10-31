namespace Apex.Users.Features.Modules.Users.Details.Models;

public record UserModel(
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public IReadOnlyCollection<string> Roles { get; init; } = null!;
}
