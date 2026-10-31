using Apex.Users.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Apex.Users.Features.Modules.Users.Details.Models;

[Mapper]
public partial class UserModelMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012
#pragma warning disable RMG090

    [MapProperty(nameof(User.Id), nameof(UserModel.UserId))]
    public partial UserModel MapToModel(User user);

    [MapProperty(nameof(CreateUserModel.Email), nameof(User.UserName))]
    public partial User MapToEntity(CreateUserModel model);

    [MapperIgnoreTarget(nameof(User.CreatedAt))]
    [MapperIgnoreTarget(nameof(User.UpdatedAt))]
    public partial void UpdateEntity(UserModel model, User entity);

#pragma warning restore RMG020
#pragma warning restore RMG012
#pragma warning restore RMG090
}
