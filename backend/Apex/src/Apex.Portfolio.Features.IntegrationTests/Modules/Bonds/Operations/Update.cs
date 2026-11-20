using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations.Services;
using Apex.Portfolio.Features.Modules.Bonds.Operations;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations;

[Collection(nameof(SliceFixture))]
public class UpdateTests(SliceFixture fixture) : BaseTests(fixture)
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
    public async Task ShouldUpdateOperation()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        var update = Faker.FakeOperation<UpdateRequest>(bondId, userId, sell.Type);
        update.Date = sell.Date;
        update.Price = sell.Price;
        update.Commission = sell.Commission;
        update.Count = 3;

        var updated = await _dataService.Update(sell.Id, userId, update);

        updated.Count.ShouldBe(3);
    }
}
