using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services;
using Apex.Portfolio.Features.Modules.Bonds.Details;
using Apex.Shared.Core.Exceptions;
using Shouldly;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class UpdateTests(SliceFixture fixture) : BaseTests(fixture)
{
    private readonly DataService _dataService = new(fixture);

    [Fact]
    public async Task ShouldThrowNotFoundWhenUpdateMissingId()
    {
        var model = Faker.FakeModel<UpdateRequest>(1, 1);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _dataService.Update(999_999, Faker.FakeUserId(), model));

        exception.Message.ShouldBe("Bond was not found 999999");
    }

    [Fact]
    public async Task ShouldUpdateBond()
    {
        var created = await _dataService.Create();

        var update = Faker.FakeModel<UpdateRequest>(created.CurrencyId, created.PaymentFrequencyId, created.Id);
        update.Ticker = $"UPD_{Guid.NewGuid():N}";
        update.Rate = 9.99m;

        var updated = await _dataService.Update(created.Id, created.UserId, update);

        updated.Id.ShouldBe(created.Id);
        (await _dataService.Get(created.Id, created.UserId)).Ticker.ShouldBe(update.Ticker);
        (await _dataService.Get(created.Id, created.UserId)).Rate.ShouldBe(9.99m);
    }
}
