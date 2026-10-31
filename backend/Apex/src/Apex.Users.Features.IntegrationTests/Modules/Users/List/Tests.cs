using Apex.Users.Features.IntegrationTests.Modules.Users.Services;
using Apex.Users.Features.Modules.Users.List.Models;
using Shouldly;

namespace Apex.Users.Features.IntegrationTests.Modules.Users.List;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    private static UserListFilter Filter(string? lastName = null) => new()
    {
        LastName = lastName,
        PageNumber = 0,
        PageSize = 10,
        SortBy = "CreatedAt",
        IsAscending = true
    };

    [Fact]
    public async Task ShouldListUsers()
    {
        var first = await _dataService.Create();
        var second = await _dataService.Create();

        var page = await _dataService.List(Filter());

        page.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
        page.Data.ShouldContain(x => x.UserId == first.UserId);
        page.Data.ShouldContain(x => x.UserId == second.UserId);
    }

    [Fact]
    public async Task ShouldFilterByLastNameIgnoringCase()
    {
        var created = await _dataService.Create();

        var page = await _dataService.List(Filter(created.LastName.ToLowerInvariant()));

        page.Data.ShouldContain(x => x.UserId == created.UserId);
        page.Data.ShouldAllBe(x => x.LastName.ToLowerInvariant().Contains(created.LastName.ToLowerInvariant()));
    }
}