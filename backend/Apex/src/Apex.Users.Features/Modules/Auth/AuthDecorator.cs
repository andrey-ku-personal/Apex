using Apex.Shared.Core.Services;
using Apex.Users.Features.Modules.Auth.Models;

namespace Apex.Users.Features.Modules.Auth;

public sealed class AuthDecorator(AuthService authService, IValidationRunner validationRunner) : IAuthService
{
    public async Task<TokenModel> Register(RegisterModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        return await authService.Register(model, cancellationToken);
    }

    public async Task<TokenModel> Login(LoginModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        return await authService.Login(model, cancellationToken);
    }

    public async Task<TokenModel> Refresh(RefreshModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        return await authService.Refresh(model, cancellationToken);
    }

    public async Task Logout(RefreshModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        await authService.Logout(model, cancellationToken);
    }
}
