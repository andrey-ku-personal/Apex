using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;
using Apex.Shared.Core.Exceptions;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

[Collection(nameof(SliceFixture))]
public class GetTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldReturnAttributeById()
    {
        var created = await _dataService.Create(MetadataType.Currency, "USD");

        var result = await _dataService.Get(created.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(created.Id);
        result.Name.ShouldBe("USD");
        result.Type.ShouldBe(MetadataType.Currency);
    }

    [Fact]
    public async Task ShouldThrowNotFoundForMissingId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(999_999));

        exception.Message.ShouldBe("InstrumentMetadata was not found 999999");
    }
}
