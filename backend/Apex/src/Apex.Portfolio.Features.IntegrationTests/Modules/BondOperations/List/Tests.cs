using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.Services;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.List;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);
    private readonly BondsDataService _bondsDataService = new(fixture);

    private async Task<(int BondId, int OperationId)> CreateOperationAsync(
        int bondId, string userId, OperationType type, DateOnly date, int count = 10)
    {
        var model = Faker.FakeOperationModel(bondId, userId, type);
        model.Date = date;
        model.Count = count;

        return (bondId, (await _dataService.Create(model)).Id);
    }

    private async Task<int> CreateBondAsync()
    {
        var bond = await _bondsDataService.Create();
        return bond.Id;
    }

    private static BondOperationListFilter Filter(string userId) => new()
    {
        UserId = userId,
        PageNumber = 0,
        PageSize = 10,
        SortBy = "Date",
        IsAscending = true
    };

    [Fact]
    public async Task ShouldListOnlyOwnOperations()
    {
        var owner = Faker.FakeUserId();
        var stranger = Faker.FakeUserId();
        var bondId = await CreateBondAsync();

        var first = await CreateOperationAsync(bondId, owner, OperationType.Buy, new DateOnly(2026, 1, 10));
        var second = await CreateOperationAsync(bondId, owner, OperationType.Sell, new DateOnly(2026, 1, 12));
        await CreateOperationAsync(bondId, stranger, OperationType.Buy, new DateOnly(2026, 1, 11));

        var page = await _dataService.List(Filter(owner));

        page.TotalCount.ShouldBe(2);
        page.Data.ShouldContain(x => x.Id == first.OperationId);
        page.Data.ShouldContain(x => x.Id == second.OperationId);
    }

    [Fact]
    public async Task ShouldFilterByType()
    {
        var userId = Faker.FakeUserId();
        var bondId = await CreateBondAsync();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 1, 10));
        await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 2, 10));
        await CreateOperationAsync(bondId, userId, OperationType.Sell, new DateOnly(2026, 3, 10));

        var filter = Filter(userId);
        filter.Type = OperationType.Buy;

        var buys = await _dataService.List(filter);

        buys.TotalCount.ShouldBe(2);
        buys.Data.ShouldAllBe(x => x.Type == OperationType.Buy);
    }

    [Fact]
    public async Task ShouldFilterByDateRange()
    {
        var userId = Faker.FakeUserId();
        var bondId = await CreateBondAsync();

        await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 1, 10));
        await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 2, 10));
        await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 3, 10));

        var filter = Filter(userId);
        filter.DateFrom = new DateOnly(2026, 1, 15);
        filter.DateTo = new DateOnly(2026, 2, 15);

        var page = await _dataService.List(filter);

        page.TotalCount.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldPaginateOperations()
    {
        var userId = Faker.FakeUserId();
        var bondId = await CreateBondAsync();

        for (var i = 0; i < 3; i++)
            await CreateOperationAsync(bondId, userId, OperationType.Buy, new DateOnly(2026, 1, 10).AddDays(i));

        var filter = Filter(userId);
        filter.PageSize = 2;

        var page = await _dataService.List(filter);

        page.TotalCount.ShouldBe(3);
        page.Data.Count.ShouldBe(2);
    }
}
