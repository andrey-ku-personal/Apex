using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;

public class InstrumentMetadataListModel
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}