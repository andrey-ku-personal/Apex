using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination.Models;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;

public class InstrumentMetadataListFilter : PageFilter
{
    public MetadataType? Type { get; set; }
}