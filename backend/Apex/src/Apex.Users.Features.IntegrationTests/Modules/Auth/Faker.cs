using Apex.Users.Features.Modules.Auth.Models;

namespace Apex.Users.Features.IntegrationTests.Modules.Auth;

public static class Faker
{
    public const string Password = "Password123!";

    public static RegisterModel FakeRegisterModel(string? email = null) => new Bogus.Faker<RegisterModel>()
        .CustomInstantiator(f => new RegisterModel(
            email ?? f.Internet.Email(),
            Password,
            f.Name.FirstName(),
            f.Name.LastName()))
        .Generate();
}