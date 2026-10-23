using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using FinancialAttributeEntity = Apex.Portfolio.Domain.Abstractions.FinancialAttribute;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;

public class FinancialAttributeService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<FinancialAttributeModel, FinancialAttributeEntity, FinancialAttributeFilter, FinancialAttributeQuery>(dbFactory, mapperFactory.GetMapper<FinancialAttributeModelMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            FinancialAttributeQuery q => $"FinancialAttribute was not found {q.Id}",
            FinancialAttributeModel m => $"FinancialAttribute was not found {m.Id}",
            FinancialAttributeFilter f => $"FinancialAttribute was not found {f.Id}",
            _ => "FinancialAttribute was not found"
        };
}
