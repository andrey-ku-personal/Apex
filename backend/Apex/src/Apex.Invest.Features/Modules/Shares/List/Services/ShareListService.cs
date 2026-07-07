using Apex.Invest.Domain.Entities;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Shares.List.Filters;
using Apex.Invest.Features.Modules.Shares.List.Mapper;
using Apex.Invest.Features.Modules.Shares.List.Models;
using Apex.Invest.Features.Modules.Shares.List.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Shares.List.Services;

public class ShareListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<ShareListModel, Share, ShareListFilter, ShareListQuery>(dbFactory, mapperFactory.GetMapper<ShareListMapper>()),
    IShareListService;
