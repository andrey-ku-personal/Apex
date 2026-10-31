using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Models.Pagination;
using Apex.Users.Features.Modules.Users.List.Models;
using Apex.Users.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Users.Features.Modules.Users.List.Services;

public sealed class UserListService(
    UsersDbContext db,
    UserListFilterMapper userListFilterMapper,
    UserListModelMapper userListModelMapper) : IUserListService
{
    public async Task<PageDataResponse<UserListModel>> List(UserListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = userListFilterMapper.MapToQuery(filter);

        var (total, items) = await db.Users
            .AsNoTracking()
            .ByQuery(query)
            .PaginateAsync(filter, cancellationToken);

        return new PageDataResponse<UserListModel>(total, userListModelMapper.MapToModels(items));
    }
}
