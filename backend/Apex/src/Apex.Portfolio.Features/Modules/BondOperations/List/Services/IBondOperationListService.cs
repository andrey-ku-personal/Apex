using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.List.Models;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Services;

public interface IBondOperationListService : IBaseListService<BondOperationListModel, BondOperationEntity, BondOperationListFilter, BondOperationListQuery>;
