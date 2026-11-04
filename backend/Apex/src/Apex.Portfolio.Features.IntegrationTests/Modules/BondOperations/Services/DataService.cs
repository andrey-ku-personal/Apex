using Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.Details;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Services;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.List.Models;
using Apex.Portfolio.Features.Modules.BondOperations.List.Services;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations.Services;

public class DataService(SliceFixture fixture)
{
    public Task<BondOperationModel> Create(BondOperationModel model) =>
        fixture.UseServiceAsync<IBondOperationService, BondOperationModel>(svc => svc.Create(model, CancellationToken.None));

    public Task<BondOperationModel> Get(int id, string userId) =>
        fixture.UseServiceAsync<IBondOperationService, BondOperationModel>(svc =>
            svc.Get(new BondOperationFilter { Id = id, UserId = userId }, CancellationToken.None));

    public Task<BondOperationModel> Update(int id, string userId, BondOperationModel model) =>
        fixture.UseServiceAsync<IBondOperationService, BondOperationModel>(svc =>
            svc.Update(new BondOperationFilter { Id = id, UserId = userId }, model, CancellationToken.None));

    public Task Delete(int id, string userId) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IBondOperationService>()
            .Delete(new BondOperationFilter { Id = id, UserId = userId }, CancellationToken.None));

    public Task<PageDataResponse<BondOperationListModel>> List(BondOperationListFilter filter) =>
        fixture.UseServiceAsync<IBondOperationListService, PageDataResponse<BondOperationListModel>>(svc => svc.GetList(filter, CancellationToken.None));
}
