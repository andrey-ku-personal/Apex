using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Domain.Abstractions;

public abstract class InstrumentMetadata
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}