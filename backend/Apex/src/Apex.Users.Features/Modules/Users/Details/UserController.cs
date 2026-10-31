using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Users.Details.Models;
using Apex.Users.Features.Modules.Users.Details.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Users.Features.Modules.Users.Details;

[ApiController]
[Route("Users")]
[Authorize(Roles = UserRole.Admin)]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{userId}")]
    public Task<UserModel> Get(string userId, CancellationToken cancellationToken) =>
        userService.GetByUserId(userId, cancellationToken);

    [HttpPost]
    public Task<UserModel> Create(CreateUserModel model, CancellationToken cancellationToken) =>
        userService.Create(model, cancellationToken);

    [HttpPut("{userId}")]
    public Task<UserModel> Update(string userId, UserModel model, CancellationToken cancellationToken) =>
        userService.Update(userId, model, cancellationToken);

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId, CancellationToken cancellationToken)
    {
        await userService.Delete(userId, cancellationToken);
        return NoContent();
    }
}
