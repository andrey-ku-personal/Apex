using Apex.Users.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Apex.Users.Features.Modules.Auth.Models;

[Mapper]
public partial class RegisterModelMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012
#pragma warning disable RMG090

    [MapProperty(nameof(RegisterModel.Email), nameof(User.UserName))]
    public partial User MapToEntity(RegisterModel model);

#pragma warning restore RMG020
#pragma warning restore RMG012
#pragma warning restore RMG090
}
