using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using Apex.Users.Features.Modules.Users.List.Models;

namespace Apex.Users.Features.Modules.Users.List.Services;

public sealed class UserListDecorator(UserListService userListService, IValidationRunner validationRunner) : IUserListService
{
    public async Task<PageDataResponse<UserListModel>> List(UserListFilter filter, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(filter, cancellationToken);
        return await userListService.List(filter, cancellationToken);
    }
}
