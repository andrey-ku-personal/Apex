using Apex.Users.Features.Modules.Users.Details.Models;

namespace Apex.Users.Features.Modules.Users.Details.Services;

public interface IUserService
{
    Task<UserModel> GetByUserId(string userId, CancellationToken cancellationToken = default);
    Task<UserModel> Create(CreateUserModel model, CancellationToken cancellationToken = default);
    Task<UserModel> Update(string userId, UserModel model, CancellationToken cancellationToken = default);
    Task Delete(string userId, CancellationToken cancellationToken = default);
}
