using Apex.Shared.Models.Pagination;
using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Users.List.Models;
using Apex.Users.Features.Modules.Users.List.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Users.Features.Modules.Users.List;

[ApiController]
[Route("Users")]
[Authorize(Roles = UserRole.Admin)]
public class UserListController(IUserListService userListService) : ControllerBase
{
    [HttpGet]
    public Task<PageDataResponse<UserListModel>> List([FromQuery] UserListFilter filter, CancellationToken cancellationToken) =>
        userListService.List(filter, cancellationToken);
}
