using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.List.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Services;

public class BondOperationListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<BondOperationListModel, BondOperationEntity, BondOperationListFilter, BondOperationListQuery>(dbFactory, mapperFactory.GetMapper<BondOperationListModelMapper>()),
    IBondOperationListService;
