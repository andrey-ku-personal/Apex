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
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;
using ValidationException = Apex.Shared.Core.Exceptions.ValidationException;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class GetHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory)
{
    private readonly BondMapper mapper = mapperFactory.GetMapper<BondMapper>();

    public async Task<BondModel> Handle(int id, string userId, CancellationToken cancellationToken)
    {
        Validate(id, userId);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await db.Set<BondEntity>().AsNoTracking()
            .ByQuery(new BondQuery { Id = id, UserId = userId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Bond was not found {id}");

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
    [HttpGet("Bond/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Get",
        summary: "Get Bond",
        description: "Get own Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, User.FindFirstValue("sub")!, cancellationToken);
}

public class BondQuery : BaseQuery<BondEntity>
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public override Expression<Func<BondEntity, bool>> GetExpression()
        => bond => bond.Id == Id && bond.UserId == UserId;
}
