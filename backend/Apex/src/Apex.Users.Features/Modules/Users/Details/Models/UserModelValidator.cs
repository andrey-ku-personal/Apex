using Apex.Users.Domain.Entities;
using FluentValidation;

namespace Apex.Users.Features.Modules.Users.Details.Models;

public class UserModelValidator : AbstractValidator<UserModel>
{
    private static readonly string[] KnownRoles = [UserRole.Admin, UserRole.Investor];

    public UserModelValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Roles).NotNull();
        RuleForEach(x => x.Roles).Must(role => KnownRoles.Contains(role))
            .WithMessage($"Unknown role. Allowed: {UserRole.Admin}, {UserRole.Investor}");
    }
}
