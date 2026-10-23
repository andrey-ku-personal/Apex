using Apex.Portfolio.Domain.Entities.FinancialAttribute;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Mapper;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Apex.Portfolio.Persistance.Extensions;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using FinancialAttributeEntity = Apex.Portfolio.Domain.Abstractions.FinancialAttribute;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;

public class FinancialAttributeService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<FinancialAttributeModel, FinancialAttributeEntity, FinancialAttributeFilter, FinancialAttributeQuery>(dbFactory, mapperFactory.GetMapper<FinancialAttributeManageMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            FinancialAttributeQuery q => $"FinancialAttribute was not found {q.Id}",
            FinancialAttributeModel m => $"FinancialAttribute was not found {m.Id}",
            FinancialAttributeFilter f => $"FinancialAttribute was not found {f.Id}",
            _ => "FinancialAttribute was not found"
        };

    protected override async Task<FinancialAttributeEntity> UpsertData(EntitiesDbContext db, FinancialAttributeModel model, CancellationToken cancellationToken)
        => await db.Set<FinancialAttributeEntity>()
            .Upsert(model)
            .OnKey(m => [m.Id])
            .CreateWhen(m => m.Id == 0)
            .CreateWith(CreateEntity)
            .ExecuteAsync((m, e) =>
            {
                if (e.Type != m.Type)
                    throw new BadRequestException($"FinancialAttribute type cannot be changed from '{e.Type}' to '{m.Type}'.");

                Mapper.MapData(m, e);
            }, cancellationToken);

    protected override async Task DeleteData(EntitiesDbContext db, FinancialAttributeFilter filter, CancellationToken cancellationToken)
    {
        var entity = await db.Set<FinancialAttributeEntity>()
            .FindAsync([filter.Id], cancellationToken)
                ?? throw new NotFoundException(GetNotFoundMessage(filter));

        db.Set<FinancialAttributeEntity>().Remove(entity);
    }

    private static FinancialAttributeEntity CreateEntity(FinancialAttributeModel model)
        => model.Type switch
        {
            MetadataType.Currency => new Currency(),
            MetadataType.PaymentFrequency => new PaymentFrequency(),
            _ => throw new BadRequestException($"FinancialAttribute type '{model.Type}' is not supported.")
        };
}
