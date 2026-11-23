using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.Bonds.Details;
using Shouldly;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class GetListTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldReturnsEmpty()
    {
        var result = await _dataService.GetList(new GetListFilter { UserId = Faker.FakeUserId() });

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnCreatedBonds()
    {
        var created = await _dataService.SeedData(3);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageSize = 10 });

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(3);
        result.Data.Count.ShouldBe(3);

        foreach (var expected in created)
        {
            var item = result.Data.Find(d => d.Id == expected.Id);
            item.ShouldNotBeNull();
            item.Ticker.ShouldBe(expected.Ticker);
            item.CurrencyId.ShouldBe(expected.CurrencyId);
        }
    }

    [Fact]
    public async Task ShouldListOnlyOwnBonds()
    {
        var owner = Faker.FakeUserId();
        var stranger = Faker.FakeUserId();

        var own = await _dataService.Create(userId: owner);
        await _dataService.Create(userId: stranger);

        var result = await _dataService.GetList(new GetListFilter { UserId = owner, PageSize = 10 });

        result.TotalCount.ShouldBe(1);
        result.Data.ShouldContain(d => d.Id == own.Id);
    }

    [Fact]
    public async Task ShouldReturnTotalCount()
    {
        const int total = 5;
        var created = await _dataService.SeedData(total);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageSize = 2 });

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationFirstPage()
    {
        var created = await _dataService.SeedData(5);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageNumber = 0, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationSecondPage()
    {
        var created = await _dataService.SeedData(5);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageNumber = 1, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetListPaginationLastPagePartial()
    {
        var created = await _dataService.SeedData(5);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageNumber = 2, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldPageOutOfRangeReturnsEmpty()
    {
        var created = await _dataService.SeedData(3);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, PageNumber = 10, PageSize = 2 });

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldFilterByCurrencyReturnsOnlySelected()
    {
        var currency = await _dataService.CreateAttribute(MetadataType.Currency);
        var userId = Faker.FakeUserId();

        await _dataService.Create(currencyId: currency, userId: userId);
        await _dataService.Create(currencyId: currency, userId: userId);
        await _dataService.Create(userId: userId);

        var result = await _dataService.GetList(new GetListFilter { UserId = userId, CurrencyId = currency, PageSize = 10 });

        result.TotalCount.ShouldBe(2);
        result.Data.ShouldAllBe(d => d.CurrencyId == currency);
    }

    [Fact]
    public async Task ShouldSortByTickerAscending()
    {
        var created = await _dataService.SeedData(3);

        var result = await _dataService.GetList(new GetListFilter { UserId = created[0].UserId, SortBy = "Ticker", IsAscending = true, PageSize = 10 });

        result.Data.Select(d => d.Ticker).ShouldBeInOrder(SortDirection.Ascending);
    }
}
