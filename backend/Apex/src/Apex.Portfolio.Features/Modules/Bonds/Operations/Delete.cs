using System.Security.Claims;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class DeleteHandler(IDbContextFactory<EntitiesDbContext> dbFactory)
{
    public async Task Handle(int id, string userId, CancellationToken cancellationToken)
    {
        Validate(id, userId);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = await db.Set<BondOperationEntity>()
            .ByQuery(new BondOperationQuery { Id = id, UserId = userId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"BondOperation was not found {id}");

        db.Set<BondOperationEntity>().Remove(entity);

        await db.CommitTransactionAsync(cancellationToken);
    }

    private void Validate(int id, string userId)
    {
        var errors = new Dictionary<string, string[]>();

        if (id < 1)
            errors["Id"] = ["'Id' must be greater than '0'."];
        if (string.IsNullOrWhiteSpace(userId))
            errors["UserId"] = ["'UserId' must not be empty."];
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}

[Authorize]
public class DeleteController(DeleteHandler handler) : SharedController
{
    [HttpDelete("BondOperation/Delete/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Delete",
        summary: "Delete BondOperation",
        description: "Delete own operation by id to fix a mistake")]
    [OpenApiTags("BondOperation")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, User.FindFirstValue("sub")!, cancellationToken);
}
