using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class Tests(SliceFixture fixture) : BaseTests(fixture)
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
    public async Task ShouldThrowNotFoundWhenUpdateMissingId()
    {
        var model = Faker.FakeModel(1, 1);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Update(999_999, model));

        exception.Message.ShouldBe("Bond was not found 999999");
    }

    [Fact]
    public async Task ShouldThrowNotFoundWhenDeleteMissingId()
    {
        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Delete(999_999));

        exception.Message.ShouldBe("Bond was not found 999999");
    }

    [Fact]
    public async Task ShouldCreateBond()
    {
        var created = await _dataService.Create();

        created.Id.ShouldBeGreaterThan(0);
        (await _dataService.Get(created.Id)).Ticker.ShouldBe(created.Ticker);
    }

    [Fact]
    public async Task ShouldUpdateBond()
    {
        var created = await _dataService.Create();
        created.Ticker = $"UPD_{Guid.NewGuid():N}";
        created.Rate = 9.99m;

        var updated = await _dataService.Update(created.Id, created);

        updated.Id.ShouldBe(created.Id);
        (await _dataService.Get(created.Id)).Ticker.ShouldBe(created.Ticker);
        (await _dataService.Get(created.Id)).Rate.ShouldBe(9.99m);
    }

    [Fact]
    public async Task ShouldThrowWhenCurrencyMissing()
    {
        var frequencyId = await _dataService.CreateAttribute(MetadataType.PaymentFrequency);

        var model = Faker.FakeModel(999_999, frequencyId);

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Create(model));
    }

    [Fact]
    public async Task ShouldThrowWhenPaymentFrequencyMissing()
    {
        var currencyId = await _dataService.CreateAttribute(MetadataType.Currency);

        var model = Faker.FakeModel(currencyId, 999_999);

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Create(model));
    }

    [Fact]
    public async Task ShouldDeleteBond()
    {
        var created = await _dataService.Create();

        await _dataService.Delete(created.Id);

        await Should.ThrowAsync<NotFoundException>(() => _dataService.Get(created.Id));
    }
}