using Apex.Shared.Core.Exceptions;
using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Users.Details.Models;
using Apex.Users.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apex.Users.Features.Modules.Users.Details.Services;

public sealed class UserService(
    UserManager<User> userManager,
    UsersDbContext db,
    UserModelMapper userModelMapper) : IUserService
{
    public async Task<UserModel> GetByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new NotFoundException($"User was not found {userId}");

        return ToModel(user, await userManager.GetRolesAsync(user));
    }

    public async Task<UserModel> Create(CreateUserModel model, CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByEmailAsync(model.Email) is not null)
            throw new ConflictException("Email already registered");

        var user = userModelMapper.MapToEntity(model);

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ValidationException(string.Join("; ", result.Errors.Select(x => x.Description)));

        var roles = model.Roles.Distinct().ToList();
        await userManager.AddToRolesAsync(user, roles);

        return ToModel(user, roles);
    }

    public async Task<UserModel> Update(string userId, UserModel model, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new NotFoundException($"User was not found {userId}");

        await EnsureNotLastAdminAsync(user, model.Roles, cancellationToken);

        userModelMapper.UpdateEntity(model, user);

        var current = await userManager.GetRolesAsync(user);
        var target = model.Roles.Distinct().ToList();

        await userManager.RemoveFromRolesAsync(user, current.Except(target));
        await userManager.AddToRolesAsync(user, target.Except(current));

        await userManager.UpdateAsync(user);

        return ToModel(user, await userManager.GetRolesAsync(user));
    }

    public async Task Delete(string userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new NotFoundException($"User was not found {userId}");

        await EnsureNotLastAdminAsync(user, [], cancellationToken);

        await userManager.DeleteAsync(user);
    }

    private async Task EnsureNotLastAdminAsync(User user, IReadOnlyCollection<string> targetRoles, CancellationToken cancellationToken)
    {
        var currentRoles = await userManager.GetRolesAsync(user);
        if (!currentRoles.Contains(UserRole.Admin) || targetRoles.Contains(UserRole.Admin))
            return;

        var adminRoleId = await db.Roles
            .Where(x => x.Name == UserRole.Admin)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var otherAdmins = await db.UserRoles
            .CountAsync(x => x.RoleId == adminRoleId && x.UserId != user.Id, cancellationToken);

        if (otherAdmins == 0)
            throw new ConflictException("Cannot remove the last admin");
    }

    private UserModel ToModel(User user, IEnumerable<string> roles) =>
        userModelMapper.MapToModel(user) with { Roles = [..roles] };
}