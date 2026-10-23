using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.IntegrationTests.Modules.FinancialAttribute.Details;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Services;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.FinancialAttribute.Services;

public class DataService(SliceFixture fixture)
{
    public Task<PageDataResponse<FinancialAttributeListModel>> GetList(FinancialAttributeListFilter filter) =>
        fixture.UseServiceAsync<IFinancialAttributeListService, PageDataResponse<FinancialAttributeListModel>>(svc => svc.GetList(filter, CancellationToken.None));

    public Task<FinancialAttributeModel> Get(int id) =>
        fixture.UseServiceAsync<IFinancialAttributeService, FinancialAttributeModel>(svc => svc.Get(new FinancialAttributeFilter { Id = id }, CancellationToken.None));

    public Task<FinancialAttributeModel> Create(FinancialAttributeModel model) =>
        fixture.UseServiceAsync<IFinancialAttributeService, FinancialAttributeModel>(svc => svc.Create(model, CancellationToken.None));

    public Task<FinancialAttributeModel> Update(int id, FinancialAttributeModel model) =>
        fixture.UseServiceAsync<IFinancialAttributeService, FinancialAttributeModel>(svc => svc.Update(new FinancialAttributeFilter { Id = id }, model, CancellationToken.None));

    public Task Delete(int id) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IFinancialAttributeService>()
            .Delete(new FinancialAttributeFilter { Id = id }, CancellationToken.None));

    public async Task<List<FinancialAttributeModel>> SeedData(int count, MetadataType? type = null)
    {
        var created = new List<FinancialAttributeModel>();

        for (var i = 0; i < count; i++)
            created.Add(await Create(type));

        return created;
    }

    public async Task<FinancialAttributeModel> Create(MetadataType? type = null, string? name = null)
    {
        var model = Faker.FakeModel(type);

        if (name is not null)
            model.Name = name;

        return await Create(model);
    }
}