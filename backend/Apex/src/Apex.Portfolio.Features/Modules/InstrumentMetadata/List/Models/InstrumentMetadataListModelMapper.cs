using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Riok.Mapperly.Abstractions;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class InstrumentMetadataListModelMapper :
    IBaseListMapper<InstrumentMetadataListModel, Domain.Abstractions.InstrumentMetadata, InstrumentMetadataListFilter, InstrumentMetadataListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<InstrumentMetadataListModel> MapToModel(List<Domain.Abstractions.InstrumentMetadata> entity);
    public partial InstrumentMetadataListQuery MapToQuery(InstrumentMetadataListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}