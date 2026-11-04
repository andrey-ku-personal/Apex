using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.Services;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using Apex.Shared.Core.Exceptions;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.Details;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private static readonly DateOnly BuyDate = new(2026, 1, 10);
    private static readonly DateOnly LaterDate = new(2026, 1, 12);

    private readonly DataService _dataService = new(fixture);
    private readonly BondsDataService _bondsDataService = new(fixture);

    private async Task<BondOperationModel> CreateOperationAsync(
        int bondId, string userId, OperationType type, DateOnly date, int count)
    {
        var model = Faker.FakeOperationModel(bondId, userId, type);
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
    public async Task ShouldThrowConflictWhenSellMoreThanOwned()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);

        await Should.ThrowAsync<ConflictException>(() =>
            CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 15));
    }

    [Fact]
    public async Task ShouldThrowConflictWhenSellBeforeFirstBuy()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, LaterDate, 10);

        await Should.ThrowAsync<ConflictException>(() =>
            CreateOperationAsync(bondId, userId, OperationType.Sell, BuyDate, 5));
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

    [Fact]
    public async Task ShouldNotSharePositionBetweenUsers()
    {
        var bondId = await CreateBondAsync();
        var first = Faker.FakeUserId();
        var second = Faker.FakeUserId();

        await CreateOperationAsync(bondId, first, OperationType.Buy, BuyDate, 10);

        await Should.ThrowAsync<ConflictException>(() =>
            CreateOperationAsync(bondId, second, OperationType.Sell, LaterDate, 5));
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
    public async Task ShouldUpdateOperationWithinPosition()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        sell.Count = 3;
        var updated = await _dataService.Update(sell.Id, userId, sell);

        updated.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldThrowConflictWhenUpdateMakesPositionNegative()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        sell.Count = 12;
        await Should.ThrowAsync<ConflictException>(() => _dataService.Update(sell.Id, userId, sell));
    }

    [Fact]
    public async Task ShouldThrowConflictWhenDeleteBreaksPosition()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        var buy = await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        await Should.ThrowAsync<ConflictException>(() => _dataService.Delete(buy.Id, userId));
    }

    [Fact]
    public async Task ShouldDeleteOperationToFixMistake()
    {
        var bondId = await CreateBondAsync();
        var userId = Faker.FakeUserId();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, BuyDate, 10);
        var sell = await CreateOperationAsync(bondId, userId, OperationType.Sell, LaterDate, 5);

        await _dataService.Delete(sell.Id, userId);

        await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(sell.Id, userId));
    }

    [Fact]
    public async Task ShouldThrowNotFoundForMissingOperation()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            _dataService.Get(999_999, Faker.FakeUserId()));

        exception.Message.ShouldContain("999999");
    }
}
