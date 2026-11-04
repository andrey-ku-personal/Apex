using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Services;

public class BondOperationService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<BondOperationModel, BondOperationEntity, BondOperationFilter, BondOperationQuery>(dbFactory, mapperFactory.GetMapper<BondOperationModelMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context is BondOperationFilter filter
            ? $"BondOperation was not found {filter.Id}"
            : "BondOperation was not found";

    protected override Task ValidateCreateAsync(EntitiesDbContext db, BondOperationEntity entity, CancellationToken cancellationToken)
        => ValidatePositionAsync(db, entity.BondId, entity.UserId, replaceId: null, candidate: entity, cancellationToken);

    protected override async Task ValidateUpdateAsync(EntitiesDbContext db, BondOperationEntity entity, BondOperationModel model, CancellationToken cancellationToken)
    {
        await ValidatePositionAsync(db, model.BondId, model.UserId, replaceId: entity.Id,
            candidate: new BondOperationEntity
            {
                Id = entity.Id,
                BondId = model.BondId,
                UserId = model.UserId,
                Type = model.Type,
                Date = model.Date,
                Count = model.Count
            },
            cancellationToken);

        if (entity.BondId != model.BondId || entity.UserId != model.UserId)
            await ValidatePositionAsync(db, entity.BondId, entity.UserId, replaceId: entity.Id, candidate: null, cancellationToken);
    }

    protected override Task ValidateDeleteAsync(EntitiesDbContext db, BondOperationEntity entity, CancellationToken cancellationToken)
        => ValidatePositionAsync(db, entity.BondId, entity.UserId, replaceId: entity.Id, candidate: null, cancellationToken);

    private async Task ValidatePositionAsync(
        EntitiesDbContext db,
        int bondId,
        string userId,
        int? replaceId,
        BondOperationEntity? candidate,
        CancellationToken cancellationToken)
    {
        var history = await db.Set<BondOperationEntity>()
            .AsNoTracking()
            .Where(x => x.BondId == bondId && x.UserId == userId && x.Id != replaceId)
            .ToListAsync(cancellationToken);

        if (candidate is not null)
            history.Add(candidate);

        var balance = 0;
        foreach (var operation in history.OrderBy(x => x.Date).ThenBy(x => x.Id))
        {
            balance += operation.Type == Domain.Enums.OperationType.Buy ? operation.Count : -operation.Count;

            if (balance < 0)
                throw new ConflictException(
                    $"Insufficient position for bond {bondId} on {operation.Date:yyyy-MM-dd}: balance would go negative");
        }
    }
}
