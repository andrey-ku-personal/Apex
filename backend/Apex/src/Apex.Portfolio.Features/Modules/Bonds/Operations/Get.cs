using System.Linq.Expressions;
using System.Security.Claims;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class GetHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory)
{
    private readonly BondOperationMapper mapper = mapperFactory.GetMapper<BondOperationMapper>();

    public async Task<BondOperationModel> Handle(int id, string userId, CancellationToken cancellationToken)
    {
        Validate(id, userId);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await db.Set<BondOperationEntity>().AsNoTracking()
            .ByQuery(new BondOperationQuery { Id = id, UserId = userId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"BondOperation was not found {id}");

        return mapper.MapToModel(entity);
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
public class GetController(GetHandler handler) : SharedController
{
    [HttpGet("BondOperation/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "BondOperation.Get",
        summary: "Get BondOperation",
        description: "Get own BondOperation by id")]
    [OpenApiTags("BondOperation")]
    public async Task<BondOperationModel> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, User.FindFirstValue("sub")!, cancellationToken);
}

public class BondOperationQuery : BaseQuery<BondOperationEntity>
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public override Expression<Func<BondOperationEntity, bool>> GetExpression()
        => operation => operation.Id == Id && operation.UserId == UserId;
}
