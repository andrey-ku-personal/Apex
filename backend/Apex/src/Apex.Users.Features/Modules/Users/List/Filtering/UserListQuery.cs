using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;
using Apex.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apex.Users.Features.Modules.Users.List.Filtering;

public class UserListQuery : BaseSortQuery<User>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public override Expression<Func<User, bool>> GetExpression() => user =>
        (FirstName == null || EF.Functions.ILike(user.FirstName, $"%{FirstName}%")) &&
        (LastName == null || EF.Functions.ILike(user.LastName, $"%{LastName}%"));
}