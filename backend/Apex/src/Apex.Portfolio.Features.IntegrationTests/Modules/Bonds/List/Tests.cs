using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.List;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldReturnsEmpty()
    {
        var result = await _dataService.GetList(new BondListFilter());

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnCreatedBonds()
    {
        var created = await _dataService.SeedData(3);

        var result = await _dataService.GetList(new BondListFilter { PageSize = 10 });

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
    public async Task ShouldReturnTotalCount()
    {
        const int total = 5;
        await _dataService.SeedData(total);

        var result = await _dataService.GetList(new BondListFilter { PageSize = 2 });

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationFirstPage()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new BondListFilter { PageNumber = 0, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationSecondPage()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new BondListFilter { PageNumber = 1, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetListPaginationLastPagePartial()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new BondListFilter { PageNumber = 2, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldPageOutOfRangeReturnsEmpty()
    {
        await _dataService.SeedData(3);

        var result = await _dataService.GetList(new BondListFilter { PageNumber = 10, PageSize = 2 });

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldFilterByCurrencyReturnsOnlySelected()
    {
        var currency = await _dataService.CreateAttribute(MetadataType.Currency);

        await _dataService.Create(currencyId: currency);
        await _dataService.Create(currencyId: currency);
        await _dataService.Create();

        var result = await _dataService.GetList(new BondListFilter { CurrencyId = currency, PageSize = 10 });

        result.TotalCount.ShouldBe(2);
        result.Data.ShouldAllBe(d => d.CurrencyId == currency);
    }

    [Fact]
    public async Task ShouldSortByTickerAscending()
    {
        await _dataService.SeedData(3);

        var result = await _dataService.GetList(new BondListFilter { SortBy = "Ticker", IsAscending = true, PageSize = 10 });

        result.Data.Select(d => d.Ticker).ShouldBeInOrder(SortDirection.Ascending);
    }
}