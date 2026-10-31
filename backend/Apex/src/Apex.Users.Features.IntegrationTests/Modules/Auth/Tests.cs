using Apex.Shared.Core.Exceptions;
using Apex.Users.Domain.Entities;
using Apex.Users.Features.IntegrationTests.Modules.Auth.Services;
using Apex.Users.Features.Modules.Auth.Models;
using Apex.Users.Features.Security;
using Apex.Users.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Apex.Users.Features.IntegrationTests.Modules.Auth;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldRegisterAndIssueTokenPair()
    {
        var model = Faker.FakeRegisterModel();

        var tokens = await _dataService.Register(model);

        tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
        tokens.RefreshToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ShouldThrowConflictOnDuplicateEmail()
    {
        var model = Faker.FakeRegisterModel();

        await _dataService.Register(model);

        await Should.ThrowAsync<ConflictException>(() => _dataService.Register(model));
    }

    [Fact]
    public async Task ShouldThrowUnauthorizedOnInvalidPassword()
    {
        var model = Faker.FakeRegisterModel();
        await _dataService.Register(model);

        var exception = await Should.ThrowAsync<UnauthorizedException>(() =>
            _dataService.Login(new LoginModel(model.Email, "WrongPassword123!")));

        exception.Message.ShouldBe("Invalid email or password");
    }

    [Fact]
    public async Task ShouldLoginWithValidCredentials()
    {
        var model = Faker.FakeRegisterModel();
        await _dataService.Register(model);

        var tokens = await _dataService.Login(new LoginModel(model.Email, Faker.Password));

        tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
        tokens.RefreshToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ShouldRotateRefreshToken()
    {
        var tokens = await _dataService.Register();

        var rotated = await _dataService.Refresh(tokens.RefreshToken);

        rotated.RefreshToken.ShouldNotBe(tokens.RefreshToken);
    }

    [Fact]
    public async Task ShouldBurnAllSessionsOnRefreshReplay()
    {
        var first = await _dataService.Register();

        var second = await _dataService.Refresh(first.RefreshToken);

        await Should.ThrowAsync<UnauthorizedException>(() => _dataService.Refresh(first.RefreshToken));
        await Should.ThrowAsync<UnauthorizedException>(() => _dataService.Refresh(second.RefreshToken));
    }

    [Fact]
    public async Task ShouldBurnRefreshTokenOnLogout()
    {
        var tokens = await _dataService.Register();

        await _dataService.Logout(tokens.RefreshToken);

        await Should.ThrowAsync<UnauthorizedException>(() => _dataService.Refresh(tokens.RefreshToken));
    }

    [Fact]
    public async Task ShouldAllowLoginAfterMultipleFailedAttempts()
    {
        var model = Faker.FakeRegisterModel();
        await _dataService.Register(model);

        for (var i = 0; i < 6; i++)
            await Should.ThrowAsync<UnauthorizedException>(() =>
                _dataService.Login(new LoginModel(model.Email, "WrongPassword123!")));

        var tokens = await _dataService.Login(new LoginModel(model.Email, Faker.Password));

        tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ShouldCloseExpiredRefreshTokenOnRefreshAttempt()
    {
        var tokens = await _dataService.Register();

        await fixture.ExecuteScopeAsync(async sp =>
        {
            var db = sp.GetRequiredService<UsersDbContext>();
            var hash = sp.GetRequiredService<ITokenService>().HashToken(tokens.RefreshToken);

            var row = await db.Set<RefreshToken>().SingleAsync(x => x.TokenHash == hash);
            row.ExpiredAt = DateTime.UtcNow.AddMinutes(-1);
            await db.SaveChangesAsync();
        });

        await Should.ThrowAsync<UnauthorizedException>(() => _dataService.Refresh(tokens.RefreshToken));

        await fixture.ExecuteScopeAsync(async sp =>
        {
            var db = sp.GetRequiredService<UsersDbContext>();
            var hash = sp.GetRequiredService<ITokenService>().HashToken(tokens.RefreshToken);

            var row = await db.Set<RefreshToken>().SingleAsync(x => x.TokenHash == hash);
            row.ClosedAt.ShouldNotBeNull();
        });
    }
}