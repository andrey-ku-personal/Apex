using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;
using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using Apex.Portfolio.Features.Modules.Bonds.Details.Services;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.List.Models;
using Apex.Portfolio.Features.Modules.Bonds.List.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Services;

public class DataService(SliceFixture fixture)
{
    public Task<PageDataResponse<BondListModel>> GetList(BondListFilter filter) =>
        fixture.UseServiceAsync<IBondListService, PageDataResponse<BondListModel>>(svc => svc.GetList(filter, CancellationToken.None));

    public Task<BondModel> Get(int id) =>
        fixture.UseServiceAsync<IBondService, BondModel>(svc => svc.Get(new BondFilter { Id = id }, CancellationToken.None));

    public Task<BondModel> Create(BondModel model) =>
        fixture.UseServiceAsync<IBondService, BondModel>(svc => svc.Create(model, CancellationToken.None));

    public Task<BondModel> Update(int id, BondModel model) =>
        fixture.UseServiceAsync<IBondService, BondModel>(svc => svc.Update(new BondFilter { Id = id }, model, CancellationToken.None));

    public Task Delete(int id) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IBondService>()
            .Delete(new BondFilter { Id = id }, CancellationToken.None));

    public async Task<BondModel> Create(int? currencyId = null, int? paymentFrequencyId = null)
    {
        var resolvedCurrency = currencyId ?? await CreateAttribute(MetadataType.Currency);
        var resolvedFrequency = paymentFrequencyId ?? await CreateAttribute(MetadataType.PaymentFrequency);

        return await Create(Faker.FakeModel(resolvedCurrency, resolvedFrequency));
    }

    public async Task<List<BondModel>> SeedData(int count)
    {
        var currency = await CreateAttribute(MetadataType.Currency);
        var frequency = await CreateAttribute(MetadataType.PaymentFrequency);

        var created = new List<BondModel>();

        for (var i = 0; i < count; i++)
            created.Add(await Create(currency, frequency));

        return created;
    }

    public async Task<int> CreateAttribute(MetadataType type)
    {
        var model = new InstrumentMetadataModel { Type = type, Name = $"ATTR_{Guid.NewGuid():N}" };

        var created = await fixture.UseServiceAsync<IInstrumentMetadataService, InstrumentMetadataModel>(
            svc => svc.Create(model, CancellationToken.None));

        return created.Id;
    }
}