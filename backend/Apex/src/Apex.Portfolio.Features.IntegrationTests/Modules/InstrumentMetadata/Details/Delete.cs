using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;
using Apex.Shared.Core.Exceptions;
using Shouldly;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services.DataService;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

[Collection(nameof(SliceFixture))]
public class DeleteTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldThrowNotFoundWhenDeleteMissingId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Delete(999_999));

        exception.Message.ShouldBe("InstrumentMetadata was not found 999999");
    }

    [Fact]
    public async Task ShouldDeleteAttribute()
    {
        var created = await _dataService.Create(MetadataType.PaymentFrequency);

        await _dataService.Delete(created.Id);

        await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(created.Id));
    }

    [Fact]
    public async Task ShouldThrowWhenDeleteCurrencyUsedByBond()
    {
        var currency = await _dataService.Create(MetadataType.Currency);

        var bondsDataService = new BondsDataService(Fixture);
        await bondsDataService.Create(currencyId: currency.Id);

        await Should.ThrowAsync<ConflictException>(() => _dataService.Delete(currency.Id));
    }
}
