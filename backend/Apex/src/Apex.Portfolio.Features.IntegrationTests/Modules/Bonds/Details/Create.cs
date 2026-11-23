using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services;
using Apex.Portfolio.Features.Modules.Bonds.Details;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class CreateTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldCreateBond()
    {
        var created = await _dataService.Create();

        created.Id.ShouldBeGreaterThan(0);
        (await _dataService.Get(created.Id, created.UserId)).Ticker.ShouldBe(created.Ticker);
    }

    [Fact]
    public async Task ShouldThrowWhenCurrencyMissing()
    {
        var frequencyId = await _dataService.CreateAttribute(MetadataType.PaymentFrequency);

        var model = Faker.FakeModel<CreateRequest>(999_999, frequencyId);

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Create(model));
    }

    [Fact]
    public async Task ShouldThrowWhenPaymentFrequencyMissing()
    {
        var currencyId = await _dataService.CreateAttribute(MetadataType.Currency);

        var model = Faker.FakeModel<CreateRequest>(currencyId, 999_999);

        await Should.ThrowAsync<DbUpdateException>(() => _dataService.Create(model));
    }
}
