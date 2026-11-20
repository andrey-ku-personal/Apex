using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details.Services;

public class DataService(SliceFixture fixture)
{
    public Task<PageDataResponse<InstrumentMetadataListModel>> GetList(GetListFilter filter) =>
        fixture.UseServiceAsync<GetListHandler, PageDataResponse<InstrumentMetadataListModel>>(svc => svc.Handle(filter, CancellationToken.None));

    public Task<InstrumentMetadataModel> Get(int id) =>
        fixture.UseServiceAsync<GetHandler, InstrumentMetadataModel>(svc => svc.Handle(id, CancellationToken.None));

    public Task<InstrumentMetadataModel> Create(CreateRequest model) =>
        fixture.UseServiceAsync<CreateHandler, InstrumentMetadataModel>(svc => svc.Handle(model, CancellationToken.None));

    public Task<InstrumentMetadataModel> Update(int id, UpdateRequest model) =>
        fixture.UseServiceAsync<UpdateHandler, InstrumentMetadataModel>(svc => svc.Handle(id, model, CancellationToken.None));

    public Task Delete(int id) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<DeleteHandler>()
            .Handle(id, CancellationToken.None));

    public async Task<List<InstrumentMetadataModel>> SeedData(int count, MetadataType? type = null)
    {
        var created = new List<InstrumentMetadataModel>();

        for (var i = 0; i < count; i++)
            created.Add(await Create(type));

        return created;
    }

    public async Task<InstrumentMetadataModel> Create(MetadataType? type = null, string? name = null)
    {
        var model = Faker.FakeModel<CreateRequest>(type);

        if (name is not null)
            model.Name = name;

        return await Create(model);
    }
}
