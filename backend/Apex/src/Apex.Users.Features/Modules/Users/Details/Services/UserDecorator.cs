using Apex.Shared.Core.Services;
using Apex.Users.Features.Modules.Users.Details.Models;

namespace Apex.Users.Features.Modules.Users.Details.Services;

public sealed class UserDecorator(UserService userService, IValidationRunner validationRunner) : IUserService
{
    public Task<UserModel> GetByUserId(string userId, CancellationToken cancellationToken = default) =>
        userService.GetByUserId(userId, cancellationToken);

    public async Task<UserModel> Create(CreateUserModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        return await userService.Create(model, cancellationToken);
    }

    public async Task<UserModel> Update(string userId, UserModel model, CancellationToken cancellationToken = default)
    {
        await validationRunner.ValidateAsync(model, cancellationToken);
        return await userService.Update(userId, model, cancellationToken);
    }

    public Task Delete(string userId, CancellationToken cancellationToken = default) =>
        userService.Delete(userId, cancellationToken);
}
