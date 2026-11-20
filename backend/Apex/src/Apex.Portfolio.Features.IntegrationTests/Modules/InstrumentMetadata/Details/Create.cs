using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;
using Apex.Shared.Core.Exceptions;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

[Collection(nameof(SliceFixture))]
public class CreateTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

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
    public async Task ShouldThrowOnDuplicateName()
    {
        await _dataService.Create(MetadataType.Currency, "DUPLICATE");

        var model = Faker.FakeModel<CreateRequest>(MetadataType.Currency);
        model.Name = "DUPLICATE";

        await Should.ThrowAsync<ConflictException>(() => _dataService.Create(model));
    }
}
