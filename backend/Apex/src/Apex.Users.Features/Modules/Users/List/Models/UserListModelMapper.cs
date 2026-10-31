using Apex.Users.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Apex.Users.Features.Modules.Users.List.Models;

[Mapper]
public partial class UserListModelMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG090

    public partial List<UserListModel> MapToModels(List<User> users);

    [MapProperty(nameof(User.Id), nameof(UserListModel.UserId))]
    private partial UserListModel MapToModel(User user);

#pragma warning restore RMG020
#pragma warning restore RMG090
}
