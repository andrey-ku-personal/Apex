using Apex.Shared.Core.Exceptions;
using Apex.Users.Domain.Entities;
using Apex.Users.Features.IntegrationTests.Modules.Users.Services;
using Shouldly;

namespace Apex.Users.Features.IntegrationTests.Modules.Users.Details;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldCreateUser()
    {
        var model = Faker.FakeCreateUserModel();

        var created = await _dataService.Create(model);

        created.UserId.ShouldNotBeNullOrWhiteSpace();
        created.Email.ShouldBe(model.Email);
        created.FirstName.ShouldBe(model.FirstName);
        created.LastName.ShouldBe(model.LastName);
        created.Roles.ShouldContain(UserRole.Investor);
    }

    [Fact]
    public async Task ShouldReturnUserById()
    {
        var created = await _dataService.Create();

        var result = await _dataService.Get(created.UserId);

        result.UserId.ShouldBe(created.UserId);
        result.Email.ShouldBe(created.Email);
    }

    [Fact]
    public async Task ShouldThrowNotFoundForMissingUserId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Get("usr_missing"));

        exception.Message.ShouldBe("User was not found usr_missing");
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        var created = await _dataService.Create();

        var replacement = Faker.FakeCreateUserModel([UserRole.Admin]);
        var updated = await _dataService.Update(created.UserId, created with
        {
            FirstName = replacement.FirstName,
            LastName = replacement.LastName,
            Roles = replacement.Roles
        });

        updated.FirstName.ShouldBe(replacement.FirstName);
        updated.LastName.ShouldBe(replacement.LastName);
        updated.Roles.ShouldContain(UserRole.Admin);
        updated.Roles.ShouldNotContain(UserRole.Investor);
    }

    [Fact]
    public async Task ShouldThrowConflictWhenDeleteLastAdmin()
    {
        var admin = await _dataService.Create([UserRole.Admin]);

        await Should.ThrowAsync<ConflictException>(() => _dataService.Delete(admin.UserId));
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        var created = await _dataService.Create();

        await _dataService.Delete(created.UserId);

        await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(created.UserId));
    }
}