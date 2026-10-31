using Apex.Shared.Models.Pagination;
using Apex.Users.Features.IntegrationTests.Modules.Users.Details;
using Apex.Users.Features.Modules.Users.Details.Models;
using Apex.Users.Features.Modules.Users.Details.Services;
using Apex.Users.Features.Modules.Users.List.Models;
using Apex.Users.Features.Modules.Users.List.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Users.Features.IntegrationTests.Modules.Users.Services;

public class DataService(SliceFixture fixture)
{
    public Task<UserModel> Create(CreateUserModel model) =>
        fixture.UseServiceAsync<IUserService, UserModel>(svc => svc.Create(model, CancellationToken.None));

    public Task<UserModel> Create(IReadOnlyCollection<string>? roles = null) =>
        Create(Faker.FakeCreateUserModel(roles));

    public Task<UserModel> Get(string userId) =>
        fixture.UseServiceAsync<IUserService, UserModel>(svc => svc.GetByUserId(userId, CancellationToken.None));

    public Task<UserModel> Update(string userId, UserModel model) =>
        fixture.UseServiceAsync<IUserService, UserModel>(svc => svc.Update(userId, model, CancellationToken.None));

    public Task Delete(string userId) =>
        fixture.ExecuteScopeAsync(async sp => await sp.GetRequiredService<IUserService>()
            .Delete(userId, CancellationToken.None));

    public Task<PageDataResponse<UserListModel>> List(UserListFilter filter) =>
        fixture.UseServiceAsync<IUserListService, PageDataResponse<UserListModel>>(svc => svc.List(filter, CancellationToken.None));
}