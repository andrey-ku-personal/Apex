namespace Apex.Users.Features.Modules.Users.List.Models;

public record UserListModel(
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt);
