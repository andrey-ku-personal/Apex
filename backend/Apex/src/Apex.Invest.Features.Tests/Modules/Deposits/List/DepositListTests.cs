using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Services;
using Apex.Invest.Features.Modules.Deposits.List.Filters;
using Apex.Invest.Features.Modules.Deposits.List.Models;
using Apex.Invest.Features.Modules.Deposits.List.Services;
using Apex.Invest.Features.Tests.Modules.Deposits.Details;
using Apex.Shared.Core.Pagination.Models;
using Shouldly;

namespace Apex.Invest.Features.Tests.Modules.Deposits.List;

[Collection(nameof(SliceFixture))]
public class DepositListTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public async Task GetList_Empty_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter(), CancellationToken.None));

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_Returns_Created_Deposits()
    {
        await _fixture.InitializeAsync();

        var created = await SeedDepositsAsync(3);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageSize = 10 }, CancellationToken.None));

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
        await SeedDepositsAsync(total);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_First_Page()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(5);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageNumber = 0, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Second_Page()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(5);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageNumber = 1, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Last_Page_Partial()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(5);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageNumber = 2, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Get_List_Pagination_Page_Beyond_Data_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(3);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageNumber = 10, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_SortById_Ascending()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(3);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { SortBy = "Id", IsAscending = true, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Ascending);
    }

    [Fact]
    public async Task Get_List_SortById_Descending()
    {
        await _fixture.InitializeAsync();

        await SeedDepositsAsync(3);

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { SortBy = "Id", IsAscending = false, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Descending);
    }

    [Fact]
    public async Task Get_List_Single_Deposit_Returns_One()
    {
        await _fixture.InitializeAsync();

        var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
        await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var result = await _fixture.UseServiceAsync<IDepositListService, PageDataResponse<DepositListModel>>(
            svc => svc.GetList(new DepositListFilter { PageSize = 10 }, CancellationToken.None));

        result.TotalCount.ShouldBe(1);
        result.Data.Count.ShouldBe(1);
        result.Data[0].Id.ShouldBeGreaterThan(0);
        result.Data[0].Ticker.ShouldNotBeNullOrEmpty();
    }

    private async Task<List<DepositDetailsModel>> SeedDepositsAsync(int count)
    {
        var deposits = new List<DepositDetailsModel>();

        for (int i = 0; i < count; i++)
        {
            var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
            var created = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
                svc => svc.Upsert(model, CancellationToken.None));
            deposits.Add(created);
        }

        return deposits;
    }
}
