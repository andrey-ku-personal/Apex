using Apex.Shared.Core.Exceptions;
using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Auth.Models;
using Apex.Users.Features.Security;
using Apex.Users.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Apex.Users.Features.Modules.Auth;

public sealed class AuthService(
    UserManager<User> users,
    SignInManager<User> signIn,
    UsersDbContext db,
    ITokenService tokens,
    IOptions<AuthOptions> options,
    RegisterModelMapper registerModelMapper) : IAuthService
{
    private readonly AuthOptions _options = options.Value;

    public async Task<TokenModel> Register(RegisterModel model, CancellationToken cancellationToken = default)
    {
        if (await users.FindByEmailAsync(model.Email) is not null)
            throw new ConflictException("Email already exists");

        var user = registerModelMapper.MapToEntity(model);

        var result = await users.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ValidationException(string.Join("; ", result.Errors.Select(x => x.Description)));

        await users.AddToRoleAsync(user, UserRole.Investor);

        return await GenerateToken(user, cancellationToken);
    }

    public async Task<TokenModel> Login(LoginModel model, CancellationToken cancellationToken = default)
    {
        var user = await users.FindByEmailAsync(model.Email)
            ?? throw new UnauthorizedException("Invalid email or password");

        var result = await signIn.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new UnauthorizedException("Invalid email or password");

        return await GenerateToken(user, cancellationToken);
    }

    public async Task<TokenModel> Refresh(RefreshModel model, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var hash = tokens.HashToken(model.RefreshToken);

        var stored = await db.Set<RefreshToken>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);

        if (stored is null)
            throw new UnauthorizedException("Invalid refresh token");

        if (stored.ExpiredAt <= now)
        {
            await db.Set<RefreshToken>()
                .Where(x => x.TokenHash == hash)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ClosedAt, now), cancellationToken);

            throw new UnauthorizedException("Invalid refresh token");
        }

        if (stored.ClosedAt is not null)
        {
            await db.Set<RefreshToken>()
                .Where(x => x.UserId == stored.UserId && x.ClosedAt == null)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ClosedAt, now), cancellationToken);

            throw new UnauthorizedException("Invalid refresh token");
        }

        var closed = await db.Set<RefreshToken>()
            .Where(x => x.TokenHash == hash && x.ClosedAt == null)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ClosedAt, now), cancellationToken);

        if (closed == 0)
            throw new UnauthorizedException("Invalid refresh token");

        return await GenerateToken(stored.User, cancellationToken);
    }

    public async Task Logout(RefreshModel model, CancellationToken cancellationToken = default)
    {
        var hash = tokens.HashToken(model.RefreshToken);

        await db.Set<RefreshToken>()
            .Where(x => x.TokenHash == hash && x.ClosedAt == null)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ClosedAt, DateTime.UtcNow), cancellationToken);
    }

    private async Task<TokenModel> GenerateToken(User user, CancellationToken cancellationToken)
    {
        var roles = (await users.GetRolesAsync(user)).ToList();

        var refreshToken = tokens.GenerateRefreshToken();
        db.Set<RefreshToken>().Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = tokens.HashToken(refreshToken),
            ExpiredAt = DateTime.UtcNow.AddMinutes(_options.RefreshTokenInMinutes)
        });
        await db.SaveChangesAsync(cancellationToken);

        return new TokenModel(tokens.CreateAccessToken(user, roles), refreshToken);
    }
}
