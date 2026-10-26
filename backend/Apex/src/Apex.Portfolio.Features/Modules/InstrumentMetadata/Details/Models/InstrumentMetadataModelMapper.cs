using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Domain.Entities.InstrumentMetadata;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Shared.Core.Exceptions;
using Riok.Mapperly.Abstractions;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class InstrumentMetadataModelMapper : IBaseDetailsMapper<InstrumentMetadataModel, InstrumentMetadataEntity, InstrumentMetadataFilter, InstrumentMetadataQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial InstrumentMetadataModel MapToModel(InstrumentMetadataEntity entity);
    public partial InstrumentMetadataFilter MapToFilter(InstrumentMetadataEntity entity);
    public partial InstrumentMetadataQuery MapToQuery(InstrumentMetadataFilter filter);

    public InstrumentMetadataEntity MapToEntity(InstrumentMetadataModel model)
        => model.Type switch
        {
            MetadataType.Currency => new Currency { Name = model.Name },
            MetadataType.PaymentFrequency => new PaymentFrequency { Name = model.Name },
            _ => throw new BadRequestException($"InstrumentMetadata type '{model.Type}' is not supported.")
        };

    public InstrumentMetadataEntity MapData(InstrumentMetadataModel model, InstrumentMetadataEntity entity)
    {
        if (entity.Type != model.Type)
            throw new BadRequestException($"InstrumentMetadata type cannot be changed from '{entity.Type}' to '{model.Type}'.");

        entity.Name = model.Name;

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}