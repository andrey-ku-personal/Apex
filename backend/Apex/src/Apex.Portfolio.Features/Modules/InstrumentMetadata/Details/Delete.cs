using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class DeleteHandler(IDbContextFactory<EntitiesDbContext> dbFactory)
{
    public async Task Handle(int id, CancellationToken cancellationToken)
    {
        Validate(id);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = await db.Set<InstrumentMetadataEntity>()
            .ByQuery(new InstrumentMetadataQuery { Id = id })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"InstrumentMetadata was not found {id}");

        var usedByBonds = await db.Set<BondEntity>()
            .AnyAsync(x => x.CurrencyId == entity.Id || x.PaymentFrequencyId == entity.Id, cancellationToken);

        if (usedByBonds)
            throw new ConflictException("InstrumentMetadata is used by bonds");

        db.Set<InstrumentMetadataEntity>().Remove(entity);

        await db.CommitTransactionAsync(cancellationToken);
    }

    private void Validate(int id)
    {
        if (id < 1)
            throw new ValidationException(new Dictionary<string, string[]> { ["Id"] = ["'Id' must be greater than '0'."] });
    }
}

[Authorize]
public class DeleteController(DeleteHandler handler) : SharedController
{
    [HttpDelete("InstrumentMetadata/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Delete",
        summary: "Delete InstrumentMetadata",
        description: "Delete InstrumentMetadata by id if it is not used by bonds")]
    [OpenApiTags("InstrumentMetadata")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, cancellationToken);
}
