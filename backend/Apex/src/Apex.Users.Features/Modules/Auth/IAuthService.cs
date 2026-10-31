using Apex.Users.Features.Modules.Auth.Models;

namespace Apex.Users.Features.Modules.Auth;

public interface IAuthService
{
    Task<TokenModel> Register(RegisterModel model, CancellationToken cancellationToken = default);
    Task<TokenModel> Login(LoginModel model, CancellationToken cancellationToken = default);
    Task<TokenModel> Refresh(RefreshModel model, CancellationToken cancellationToken = default);
    Task Logout(RefreshModel model, CancellationToken cancellationToken = default);
}
