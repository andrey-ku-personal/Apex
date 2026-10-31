using FluentValidation;

namespace Apex.Users.Features.Modules.Users.List.Models;

public class UserListFilterValidator : AbstractValidator<UserListFilter>
{
    public UserListFilterValidator()
    {
        RuleFor(x => x.FirstName).MaximumLength(64);
        RuleFor(x => x.LastName).MaximumLength(64);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
