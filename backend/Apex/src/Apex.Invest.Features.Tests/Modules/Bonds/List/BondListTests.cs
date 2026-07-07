using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Invest.Features.Modules.Bonds.List.Filters;
using Apex.Invest.Features.Modules.Bonds.List.Models;
using Apex.Invest.Features.Modules.Bonds.List.Services;
using Apex.Invest.Features.Tests.Modules.Bonds.Details;
using Apex.Shared.Core.Pagination.Models;
using Shouldly;

namespace Apex.Invest.Features.Tests.Modules.Bonds.List;

[Collection(nameof(SliceFixture))]
public class BondListTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public async Task GetList_Empty_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter(), CancellationToken.None));

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_Returns_Created_Bonds()
    {
        await _fixture.InitializeAsync();

        var created = await SeedBondsAsync(3);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageSize = 10 }, CancellationToken.None));

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(3);
        result.Data.Count.ShouldBe(3);

        foreach (var expected in created)
        {
            var item = result.Data.Find(d => d.Id == expected.Id);
            item.ShouldNotBeNull();
            item.Ticker.ShouldBe(expected.Ticker);
        }
    }

    [Fact]
    public async Task Get_List_TotalCount_Is_Correct()
    {
        await _fixture.InitializeAsync();

        const int total = 5;
        await SeedBondsAsync(total);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_First_Page()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(5);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageNumber = 0, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Second_Page()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(5);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageNumber = 1, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Last_Page_Partial()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(5);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageNumber = 2, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Get_List_Pagination_Page_Beyond_Data_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(3);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageNumber = 10, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_SortById_Ascending()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(3);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { SortBy = "Id", IsAscending = true, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Ascending);
    }

    [Fact]
    public async Task Get_List_SortById_Descending()
    {
        await _fixture.InitializeAsync();

        await SeedBondsAsync(3);

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { SortBy = "Id", IsAscending = false, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Descending);
    }

    [Fact]
    public async Task Get_List_Single_Bond_Returns_One()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);
        await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var result = await _fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(
            svc => svc.GetList(new BondListFilter { PageSize = 10 }, CancellationToken.None));

        result.TotalCount.ShouldBe(1);
        result.Data.Count.ShouldBe(1);
        result.Data[0].Id.ShouldBeGreaterThan(0);
        result.Data[0].Ticker.ShouldNotBeNullOrEmpty();
    }

    private async Task<List<BondDetailsModel>> SeedBondsAsync(int count)
    {
        var bonds = new List<BondDetailsModel>();

        for (int i = 0; i < count; i++)
        {
            var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);
            var created = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
                svc => svc.Upsert(model, CancellationToken.None));
            bonds.Add(created);
        }

        return bonds;
    }
}
