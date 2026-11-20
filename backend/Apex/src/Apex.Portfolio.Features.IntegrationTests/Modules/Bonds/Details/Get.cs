using System.Net;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services;
using Apex.Shared.Core.Exceptions;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class GetTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldReturnBondById()
    {
        var created = await _dataService.Create();

        var result = await _dataService.Get(created.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(created.Id);
        result.Ticker.ShouldBe(created.Ticker);
        result.Issuer.ShouldBe(created.Issuer);
        result.CurrencyId.ShouldBe(created.CurrencyId);
        result.PaymentFrequencyId.ShouldBe(created.PaymentFrequencyId);
        result.Rate.ShouldBe(created.Rate);
        result.IsRefinancingLinked.ShouldBe(created.IsRefinancingLinked);
    }

    [Fact]
    public async Task ShouldThrowNotFoundForMissingId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(999_999));

        exception.Message.ShouldBe("Bond was not found 999999");
    }

    [Fact]
    public async Task ShouldNotExposeDeleteEndpoint()
    {
        var client = Fixture.CreateClient();

        var response = await client.DeleteAsync("api/v1/Bond/Delete/5");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
