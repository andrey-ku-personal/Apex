using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations.Services;
using Apex.Portfolio.Features.Modules.Bonds.Operations;
using Apex.Shared.Core.Exceptions;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations;

[Collection(nameof(SliceFixture))]
public class GetTests(SliceFixture fixture) : BaseTests(fixture)
{
    private static readonly DateOnly BuyDate = new(2026, 1, 10);

    private readonly DataService _dataService = new(fixture);
    private readonly BondsDataService _bondsDataService = new(fixture);

    private async Task<BondOperationModel> CreateOperationAsync(
        int bondId, string userId, OperationType type, DateOnly date, int count)
    {
        var model = Faker.FakeOperation<CreateRequest>(bondId, userId, type);
        model.Date = date;
        model.Count = count;
        return await _dataService.Create(model);
    }

    private async Task<int> CreateBondAsync()
    {
        var bond = await _bondsDataService.Create();
        return bond.Id;
    }

    [Fact]
    public async Task ShouldNotSeeOperationOfAnotherUser()
    {
        var bondId = await CreateBondAsync();
        var owner = Faker.FakeUserId();
        var stranger = Faker.FakeUserId();

        var created = await CreateOperationAsync(bondId, owner, OperationType.Buy, BuyDate, 10);

        await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(created.Id, stranger));
    }

    [Fact]
    public async Task ShouldThrowNotFoundForMissingOperation()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            _dataService.Get(999_999, Faker.FakeUserId()));

        exception.Message.ShouldContain("999999");
    }
}
