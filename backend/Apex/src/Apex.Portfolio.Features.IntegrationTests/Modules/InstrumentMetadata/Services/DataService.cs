using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Services;

public class DataService(SliceFixture fixture)
{
    public Task<PageDataResponse<InstrumentMetadataListModel>> GetList(InstrumentMetadataListFilter filter) =>
        fixture.UseServiceAsync<IInstrumentMetadataListService, PageDataResponse<InstrumentMetadataListModel>>(svc => svc.GetList(filter, CancellationToken.None));

    public Task<InstrumentMetadataModel> Get(int id) =>
        fixture.UseServiceAsync<IInstrumentMetadataService, InstrumentMetadataModel>(svc => svc.Get(new InstrumentMetadataFilter { Id = id }, CancellationToken.None));

    public Task<InstrumentMetadataModel> Create(InstrumentMetadataModel model) =>
        fixture.UseServiceAsync<IInstrumentMetadataService, InstrumentMetadataModel>(svc => svc.Create(model, CancellationToken.None));

    public Task<InstrumentMetadataModel> Update(int id, InstrumentMetadataModel model) =>
        fixture.UseServiceAsync<IInstrumentMetadataService, InstrumentMetadataModel>(svc => svc.Update(new InstrumentMetadataFilter { Id = id }, model, CancellationToken.None));

    public Task Delete(int id) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IInstrumentMetadataService>()
            .Delete(new InstrumentMetadataFilter { Id = id }, CancellationToken.None));

    public async Task<List<InstrumentMetadataModel>> SeedData(int count, MetadataType? type = null)
    {
        var created = new List<InstrumentMetadataModel>();

        for (var i = 0; i < count; i++)
            created.Add(await Create(type));

        return created;
    }

    public async Task<InstrumentMetadataModel> Create(MetadataType? type = null, string? name = null)
    {
        var model = Faker.FakeModel(type);

        if (name is not null)
            model.Name = name;

        return await Create(model);
    }
}