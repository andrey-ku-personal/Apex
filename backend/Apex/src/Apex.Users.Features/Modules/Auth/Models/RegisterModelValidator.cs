using FluentValidation;

namespace Apex.Users.Features.Modules.Auth.Models;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(64);
    }
}
