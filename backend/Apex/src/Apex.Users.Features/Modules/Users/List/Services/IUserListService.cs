using Apex.Shared.Models.Pagination;
using Apex.Users.Features.Modules.Users.List.Models;

namespace Apex.Users.Features.Modules.Users.List.Services;

public interface IUserListService
{
    Task<PageDataResponse<UserListModel>> List(UserListFilter filter, CancellationToken cancellationToken = default);
}
