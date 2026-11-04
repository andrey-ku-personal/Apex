using Apex.Portfolio.Modules;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.List.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Services;

public class BondOperationListDecorator(
    BondOperationListService bondOperationListService,
    IValidationRunner validationRunner
) : IBondOperationListService
{
    private readonly BaseGetListServiceDecorator<BondOperationListModel, BondOperationEntity, BondOperationListFilter, BondOperationListQuery> _decorator =
        new(bondOperationListService, validationRunner);

    public Task<PageDataResponse<BondOperationListModel>> GetList(BondOperationListFilter filter, CancellationToken cancellationToken) =>
        _decorator.GetList(filter, cancellationToken);
}
