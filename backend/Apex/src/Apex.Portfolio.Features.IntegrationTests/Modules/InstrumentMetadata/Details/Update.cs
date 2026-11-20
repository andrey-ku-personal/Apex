using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;
using Apex.Shared.Core.Exceptions;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

[Collection(nameof(SliceFixture))]
public class UpdateTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldThrowNotFoundWhenUpdateMissingId()
    {
        var model = Faker.FakeModel<UpdateRequest>(MetadataType.Currency);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Update(999_999, model));

        exception.Message.ShouldBe("InstrumentMetadata was not found 999999");
    }

    [Fact]
    public async Task ShouldRenameAttribute()
    {
        var created = await _dataService.Create(MetadataType.Currency);

        var update = Faker.FakeModel<UpdateRequest>(created.Type, created.Id);
        update.Name = $"RENAMED_{Guid.NewGuid():N}";

        var updated = await _dataService.Update(created.Id, update);

        updated.Id.ShouldBe(created.Id);
        (await _dataService.Get(created.Id)).Name.ShouldBe(update.Name);
    }

    [Fact]
    public async Task ShouldThrowWhenTypeChanged()
    {
        var created = await _dataService.Create(MetadataType.Currency);

        var update = Faker.FakeModel<UpdateRequest>(created.Type, created.Id);
        update.Type = MetadataType.PaymentFrequency;

        var exception = await Should.ThrowAsync<BadRequestException>(() => _dataService.Update(created.Id, update));

        exception.Message.ShouldContain("type cannot be changed");
    }
}
