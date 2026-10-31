using FluentValidation;

namespace Apex.Users.Features.Modules.Auth.Models;

public class RefreshModelValidator : AbstractValidator<RefreshModel>
{
    public RefreshModelValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
    }
}
