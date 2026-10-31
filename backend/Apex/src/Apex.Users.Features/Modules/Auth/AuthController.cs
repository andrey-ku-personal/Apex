using System.Security.Claims;
using Apex.Shared.Core.Exceptions;
using Apex.Users.Features.Modules.Auth.Models;
using Apex.Users.Features.Modules.Users.Details.Models;
using Apex.Users.Features.Modules.Users.Details.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Users.Features.Modules.Auth;

[ApiController]
[Route("Auth")]
public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
{
    [HttpPost("Register")]
    public Task<TokenModel> Register(RegisterModel model, CancellationToken cancellationToken) =>
        authService.Register(model, cancellationToken);

    [HttpPost("Login")]
    public Task<TokenModel> Login(LoginModel model, CancellationToken cancellationToken) =>
        authService.Login(model, cancellationToken);

    [HttpPost("Refresh")]
    public Task<TokenModel> Refresh(RefreshModel model, CancellationToken cancellationToken) =>
        authService.Refresh(model, cancellationToken);

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout(RefreshModel model, CancellationToken cancellationToken)
    {
        await authService.Logout(model, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<UserModel> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub")
            ?? throw new UnauthorizedException("Missing subject claim");

        return await userService.GetByUserId(userId, cancellationToken);
    }
}
