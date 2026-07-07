using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Services;
using Apex.Invest.Features.Modules.Shares.List.Filters;
using Apex.Invest.Features.Modules.Shares.List.Models;
using Apex.Invest.Features.Modules.Shares.List.Services;
using Apex.Invest.Features.Tests.Modules.Shares.Details;
using Apex.Shared.Core.Pagination.Models;
using Shouldly;

namespace Apex.Invest.Features.Tests.Modules.Shares.List;

[Collection(nameof(SliceFixture))]
public class ShareListTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public async Task GetList_Empty_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter(), CancellationToken.None));

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(0);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_Returns_Created_Shares()
    {
        await _fixture.InitializeAsync();

        var created = await SeedSharesAsync(3);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageSize = 10 }, CancellationToken.None));

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
        await SeedSharesAsync(total);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(total);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_First_Page()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(5);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageNumber = 0, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Second_Page()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(5);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageNumber = 1, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Get_List_Pagination_Last_Page_Partial()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(5);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageNumber = 2, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(5);
        result.Data.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Get_List_Pagination_Page_Beyond_Data_Returns_Empty()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(3);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageNumber = 10, PageSize = 2 }, CancellationToken.None));

        result.TotalCount.ShouldBe(3);
        result.Data.ShouldBeEmpty();
    }

    [Fact]
    public async Task Get_List_SortById_Ascending()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(3);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { SortBy = "Id", IsAscending = true, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Ascending);
    }

    [Fact]
    public async Task Get_List_SortById_Descending()
    {
        await _fixture.InitializeAsync();

        await SeedSharesAsync(3);

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { SortBy = "Id", IsAscending = false, PageSize = 10 }, CancellationToken.None));

        result.Data.Select(d => d.Id).ShouldBeInOrder(SortDirection.Descending);
    }

    [Fact]
    public async Task Get_List_Single_Share_Returns_One()
    {
        await _fixture.InitializeAsync();

        var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);
        await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var result = await _fixture.UseServiceAsync<IShareListService, PageDataResponse<ShareListModel>>(
            svc => svc.GetList(new ShareListFilter { PageSize = 10 }, CancellationToken.None));

        result.TotalCount.ShouldBe(1);
        result.Data.Count.ShouldBe(1);
        result.Data[0].Id.ShouldBeGreaterThan(0);
        result.Data[0].Ticker.ShouldNotBeNullOrEmpty();
    }

    private async Task<List<ShareDetailsModel>> SeedSharesAsync(int count)
    {
        var shares = new List<ShareDetailsModel>();

        for (int i = 0; i < count; i++)
        {
            var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);
            var created = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
                svc => svc.Upsert(model, CancellationToken.None));
            shares.Add(created);
        }

        return shares;
    }
}
