using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations.Services;
using Apex.Portfolio.Features.Modules.Bonds.Operations;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations;

[Collection(nameof(SliceFixture))]
public class CreateTests(SliceFixture fixture) : BaseTests(fixture)
{
    private static readonly DateOnly BuyDate = new(2026, 1, 10);
    private static readonly DateOnly LaterDate = new(2026, 1, 12);

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
    public async Task ShouldCreateBuyOperation()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        var created = await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);

        created.Id.ShouldBeGreaterThan(0);
        created.UserId.ShouldBe(userId);
        created.Type.ShouldBe(OperationType.Buy);
        (await _dataService.Get(created.Id, userId)).Count.ShouldBe(10);
    }

    [Fact]
    public async Task ShouldCreateSellWithinPosition()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        sell.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task ShouldSellFullPosition()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 10);

        sell.Id.ShouldBeGreaterThan(0);
    }
}
