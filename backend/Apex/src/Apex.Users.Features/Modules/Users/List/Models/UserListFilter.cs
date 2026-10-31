using Apex.Shared.Core.Pagination.Models;

namespace Apex.Users.Features.Modules.Users.List.Models;

public class UserListFilter : PageFilter
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}
