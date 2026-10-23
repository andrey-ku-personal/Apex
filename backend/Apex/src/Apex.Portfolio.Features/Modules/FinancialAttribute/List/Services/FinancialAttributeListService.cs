using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Mapper;
using Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Services;

public class FinancialAttributeListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<
        FinancialAttributeListModel,
        Domain.Abstractions.FinancialAttribute,
        FinancialAttributeListFilter,
        FinancialAttributeListQuery
    >(dbFactory, mapperFactory.GetMapper<FinancialAttributeListMapper>()), IFinancialAttributeListService;
