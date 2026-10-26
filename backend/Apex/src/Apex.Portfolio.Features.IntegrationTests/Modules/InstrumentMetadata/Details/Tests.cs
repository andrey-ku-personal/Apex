using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Services;
using BondsDataService = Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services.DataService;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
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

    [Fact]
    public async Task ShouldThrowNotFoundWhenUpdateMissingId()
    {
        var model = Faker.FakeModel(MetadataType.Currency);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Update(999_999, model));

        exception.Message.ShouldBe("InstrumentMetadata was not found 999999");
    }

    [Fact]
    public async Task ShouldThrowNotFoundWhenDeleteMissingId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Delete(999_999));

        exception.Message.ShouldBe("InstrumentMetadata was not found 999999");
    }

    [Fact]
    public async Task ShouldCreateCurrency()
    {
        var created = await _dataService.Create(MetadataType.Currency);

        created.Id.ShouldBeGreaterThan(0);
        (await _dataService.Get(created.Id)).Type.ShouldBe(MetadataType.Currency);
    }

    [Fact]
    public async Task ShouldCreatePaymentFrequency()
    {
        var created = await _dataService.Create(MetadataType.PaymentFrequency);

        created.Id.ShouldBeGreaterThan(0);
        (await _dataService.Get(created.Id)).Type.ShouldBe(MetadataType.PaymentFrequency);
    }

    [Fact]
    public async Task ShouldRenameAttribute()
    {
        var created = await _dataService.Create(MetadataType.Currency);
        created.Name = $"RENAMED_{Guid.NewGuid():N}";

        var updated = await _dataService.Update(created.Id, created);

        updated.Id.ShouldBe(created.Id);
        (await _dataService.Get(created.Id)).Name.ShouldBe(created.Name);
    }

    [Fact]
    public async Task ShouldThrowWhenTypeChanged()
    {
        var created = await _dataService.Create(MetadataType.Currency);
        created.Type = MetadataType.PaymentFrequency;

        var exception = await Should.ThrowAsync<BadRequestException>(() => _dataService.Update(created.Id, created));

        exception.Message.ShouldContain("type cannot be changed");
    }

    [Fact]
    public async Task ShouldThrowOnDuplicateName()
    {
        await _dataService.Create(MetadataType.Currency, "DUPLICATE");

        var model = Faker.FakeModel(MetadataType.Currency);
        model.Name = "DUPLICATE";

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Create(model));
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

        var bondsDataService = new BondsDataService(fixture);
        await bondsDataService.Create(currencyId: currency.Id);

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Delete(currency.Id));
    }
}