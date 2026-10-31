using Apex.Users.Features.Modules.Users.List.Filtering;
using Riok.Mapperly.Abstractions;

namespace Apex.Users.Features.Modules.Users.List.Models;

[Mapper]
public partial class UserListFilterMapper
{
#pragma warning disable RMG060
#pragma warning disable RMG090

    public partial UserListQuery MapToQuery(UserListFilter filter);

#pragma warning restore RMG060
#pragma warning restore RMG090
}