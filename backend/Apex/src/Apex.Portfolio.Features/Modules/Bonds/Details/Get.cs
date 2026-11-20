using System.Linq.Expressions;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class GetHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory)
    : GetHandler<BondModel, BondEntity>(dbFactory)
{
    private readonly BondMapper mapper = mapperFactory.GetMapper<BondMapper>();

    protected override BaseQuery<BondEntity> GetQuery(int id)
        => new BondQuery { Id = id };

    protected override BondModel MapToModel(BondEntity entity)
        => mapper.MapToModel(entity);
}

public class GetController(GetHandler handler) : SharedController
{
    [HttpGet("Bond/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "Bond.Get",
        summary: "Get Bond",
        description: "Get Bond by id")]
    [OpenApiTags("Bond")]
    public async Task<BondModel> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, cancellationToken);
}

public class BondQuery : BaseQuery<BondEntity>
{
    public int Id { get; set; }

    public override Expression<Func<BondEntity, bool>> GetExpression()
        => bond => bond.Id == Id;
}
