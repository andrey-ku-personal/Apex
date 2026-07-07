using Apex.Invest.Domain.Entities;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Bonds.List.Filters;
using Apex.Invest.Features.Modules.Bonds.List.Mapper;
using Apex.Invest.Features.Modules.Bonds.List.Models;
using Apex.Invest.Features.Modules.Bonds.List.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Bonds.List.Services;

public class BondListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<BondListModel, Bond, BondListFilter, BondListQuery>(dbFactory, mapperFactory.GetMapper<BondListMapper>()),
    IBondListService;
