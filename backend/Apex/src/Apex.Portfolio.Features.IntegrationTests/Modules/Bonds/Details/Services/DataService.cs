using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.Bonds.Details;
using Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;
using Apex.Shared.Models.Pagination;
using InstrumentMetadataFeatures = Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details.Services;

public class DataService(SliceFixture fixture)
{
    public Task<PageDataResponse<BondListModel>> GetList(GetListFilter filter) =>
        fixture.UseServiceAsync<GetListHandler, PageDataResponse<BondListModel>>(svc => svc.Handle(filter, CancellationToken.None));

    public Task<BondModel> Get(int id) =>
        fixture.UseServiceAsync<GetHandler, BondModel>(svc => svc.Handle(id, CancellationToken.None));

    public Task<BondModel> Create(CreateRequest model) =>
        fixture.UseServiceAsync<CreateHandler, BondModel>(svc => svc.Handle(model, CancellationToken.None));

    public Task<BondModel> Update(int id, UpdateRequest model) =>
        fixture.UseServiceAsync<UpdateHandler, BondModel>(svc => svc.Handle(id, model, CancellationToken.None));

    public async Task<BondModel> Create(int? currencyId = null, int? paymentFrequencyId = null)
    {
        var resolvedCurrency = currencyId ?? await CreateAttribute(MetadataType.Currency);
        var resolvedFrequency = paymentFrequencyId ?? await CreateAttribute(MetadataType.PaymentFrequency);

        return await Create(Faker.FakeModel<CreateRequest>(resolvedCurrency, resolvedFrequency));
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
        var model = new InstrumentMetadataFeatures.CreateRequest { Type = type, Name = $"ATTR_{Guid.NewGuid():N}" };

        var created = await fixture.UseServiceAsync<InstrumentMetadataFeatures.CreateHandler, InstrumentMetadataFeatures.InstrumentMetadataModel>(
            svc => svc.Handle(model, CancellationToken.None));

        return created.Id;
    }
}
