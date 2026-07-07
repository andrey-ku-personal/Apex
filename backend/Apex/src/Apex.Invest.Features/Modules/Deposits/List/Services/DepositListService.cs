using Apex.Invest.Domain.Entities;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Deposits.List.Filters;
using Apex.Invest.Features.Modules.Deposits.List.Mapper;
using Apex.Invest.Features.Modules.Deposits.List.Models;
using Apex.Invest.Features.Modules.Deposits.List.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Deposits.List.Services;

public class DepositListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<DepositListModel, Deposit, DepositListFilter, DepositListQuery>(dbFactory, mapperFactory.GetMapper<DepositListMapper>()),
    IDepositListService;
