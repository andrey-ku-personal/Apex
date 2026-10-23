using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.FinancialAttribute.Services;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filters;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.FinancialAttribute.List;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldReturnsEmpty()
    {
        var result = await _dataService.GetList(new FinancialAttributeListFilter());

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnCreatedAttributes()
    {
        var created = await _dataService.SeedData(3);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageSize = 10 });

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(3);
        result.Data.Count.ShouldBe(3);

        foreach (var expected in created)
        {
            var item = result.Data.Find(d => d.Id == expected.Id);
            item.ShouldNotBeNull();
            item.Name.ShouldBe(expected.Name);
            item.Type.ShouldBe(expected.Type);
        }
    }

    [Fact]
    public async Task ShouldReturnTotalCount()
    {
        const int total = 5;
        await _dataService.SeedData(total);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageSize = 2 });

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationFirstPage()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageNumber = 0, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldPaginationSecondPage()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageNumber = 1, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetListPaginationLastPagePartial()
    {
        await _dataService.SeedData(5);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageNumber = 2, PageSize = 2 });

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldPageOutOfRangeReturnsEmpty()
    {
        await _dataService.SeedData(3);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { PageNumber = 10, PageSize = 2 });

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldFilterByTypeReturnsOnlySelectedType()
    {
        await _dataService.Create(MetadataType.Currency);
        await _dataService.Create(MetadataType.Currency);
        await _dataService.Create(MetadataType.PaymentFrequency);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { Type = MetadataType.Currency, PageSize = 10 });

        result.TotalCount.ShouldBe(2);
        result.Data.ShouldAllBe(d => d.Type == MetadataType.Currency);
    }

    [Fact]
    public async Task ShoudFilterByName()
    {
        var name = $"FREETEXT_{Guid.NewGuid():N}";
        await _dataService.Create(MetadataType.Currency, name);
        await _dataService.Create(MetadataType.Currency);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { FreeText = name, PageSize = 10 });

        result.TotalCount.ShouldBe(1);
        result.Data.Single().Name.ShouldBe(name);
    }

    [Fact]
    public async Task ShouldSortByAscending()
    {
        await _dataService.SeedData(3);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { SortBy = "Id", IsAscending = true, PageSize = 10 });
        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Ascending);

        var result2 = await _dataService.GetList(new FinancialAttributeListFilter { SortBy = "Name", IsAscending = true, PageSize = 10 });
        result2.Data.Select(d => d.Name).ShouldBeInOrder(SortDirection.Ascending);
    }

    [Fact]
    public async Task ShouldSortByDescending()
    {
        await _dataService.SeedData(3);

        var result = await _dataService.GetList(new FinancialAttributeListFilter { SortBy = "Id", IsAscending = false, PageSize = 10 });
        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Descending);

        var result2 = await _dataService.GetList(new FinancialAttributeListFilter { SortBy = "Name", IsAscending = false, PageSize = 10 });
        result2.Data.Select(d => d.Name).ShouldBeInOrder(SortDirection.Descending);
    }
}