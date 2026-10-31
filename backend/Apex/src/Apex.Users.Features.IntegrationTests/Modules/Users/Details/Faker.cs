using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Users.Details.Models;

namespace Apex.Users.Features.IntegrationTests.Modules.Users.Details;

public static class Faker
{
    public const string Password = "Password123!";

    public static CreateUserModel FakeCreateUserModel(IReadOnlyCollection<string>? roles = null) => new Bogus.Faker<CreateUserModel>()
        .CustomInstantiator(f => new CreateUserModel
        {
            FirstName = f.Name.FirstName(),
            LastName = f.Name.LastName(),
            Email = f.Internet.Email(),
            Password = Password,
            Roles = roles ?? [UserRole.Investor]
        })
        .Generate();
}