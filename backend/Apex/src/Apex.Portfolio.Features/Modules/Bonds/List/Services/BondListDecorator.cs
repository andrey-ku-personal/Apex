using Apex.Portfolio.Modules;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.List.Models;
using Apex.Portfolio.Features.Modules.Bonds.List.Services;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Services;

public class BondListDecorator(
    BondListService bondListService,
    IValidationRunner validationRunner
) : IBondListService
{
    private readonly BaseGetListServiceDecorator<BondListModel, BondEntity, BondListFilter, BondListQuery> _decorator =
        new(bondListService, validationRunner);

    public Task<PageDataResponse<BondListModel>> GetList(BondListFilter filter, CancellationToken cancellationToken) =>
        _decorator.GetList(filter, cancellationToken);
}
