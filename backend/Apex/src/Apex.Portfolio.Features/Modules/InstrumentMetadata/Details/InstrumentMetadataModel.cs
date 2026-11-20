using Apex.Portfolio.Domain.Enums;
using Riok.Mapperly.Abstractions;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class InstrumentMetadataModel
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class InstrumentMetadataMapper
{
    public partial InstrumentMetadataModel MapToModel(InstrumentMetadataEntity entity);
}
