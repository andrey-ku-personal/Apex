using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.List.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Services;

public class BondListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<BondListModel, BondEntity, BondListFilter, BondListQuery>(dbFactory, mapperFactory.GetMapper<BondListModelMapper>()),
    IBondListService;