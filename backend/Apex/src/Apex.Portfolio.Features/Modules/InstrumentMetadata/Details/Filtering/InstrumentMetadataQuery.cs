using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;

public class InstrumentMetadataQuery : BaseQuery<Domain.Abstractions.InstrumentMetadata>
{
    public int Id { get; set; }

    public override Expression<Func<Domain.Abstractions.InstrumentMetadata, bool>> GetExpression()
        => metadata => metadata.Id == Id;
}