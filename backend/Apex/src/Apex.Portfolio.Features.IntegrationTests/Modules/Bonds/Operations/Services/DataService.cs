using Apex.Portfolio.Features.Modules.Bonds.Operations;
using Apex.Shared.Models.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations.Services;

public class DataService(SliceFixture fixture)
{
    public Task<BondOperationModel> Create(CreateRequest model) =>
        fixture.UseServiceAsync<CreateHandler, BondOperationModel>(svc => svc.Handle(model, CancellationToken.None));

    public Task<BondOperationModel> Get(int id, string userId) =>
        fixture.UseServiceAsync<GetHandler, BondOperationModel>(svc => svc.Handle(id, userId, CancellationToken.None));

    public Task<BondOperationModel> Update(int id, string userId, UpdateRequest model)
    {
        model.UserId = userId;
        return fixture.UseServiceAsync<UpdateHandler, BondOperationModel>(svc => svc.Handle(id, userId, model, CancellationToken.None));
    }

    public Task Delete(int id, string userId) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<DeleteHandler>()
            .Handle(id, userId, CancellationToken.None));

    public Task<PageDataResponse<BondOperationListModel>> List(GetListFilter filter) =>
        fixture.UseServiceAsync<GetListHandler, PageDataResponse<BondOperationListModel>>(svc => svc.Handle(filter, CancellationToken.None));
}
