using Apex.Users.Features.Modules.Auth;
using Apex.Users.Features.Modules.Auth.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Users.Features.IntegrationTests.Modules.Auth.Services;

public class DataService(SliceFixture fixture)
{
    public Task<TokenModel> Register(RegisterModel model) =>
        fixture.UseServiceAsync<IAuthService, TokenModel>(svc => svc.Register(model, CancellationToken.None));

    public Task<TokenModel> Register(string? email = null) => Register(Faker.FakeRegisterModel(email));

    public Task<TokenModel> Login(LoginModel model) =>
        fixture.UseServiceAsync<IAuthService, TokenModel>(svc => svc.Login(model, CancellationToken.None));

    public Task<TokenModel> Refresh(string refreshToken) =>
        fixture.UseServiceAsync<IAuthService, TokenModel>(svc => svc.Refresh(new RefreshModel(refreshToken), CancellationToken.None));

    public Task Logout(string refreshToken) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IAuthService>()
            .Logout(new RefreshModel(refreshToken), CancellationToken.None));
}