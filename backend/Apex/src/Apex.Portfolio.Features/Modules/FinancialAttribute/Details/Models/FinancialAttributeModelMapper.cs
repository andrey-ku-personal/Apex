using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Domain.Entities.FinancialAttribute;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;
using Apex.Shared.Core.Exceptions;
using Riok.Mapperly.Abstractions;
using FinancialAttributeEntity = Apex.Portfolio.Domain.Abstractions.FinancialAttribute;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class FinancialAttributeModelMapper : IBaseDetailsMapper<FinancialAttributeModel, FinancialAttributeEntity, FinancialAttributeFilter, FinancialAttributeQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial FinancialAttributeModel MapToModel(FinancialAttributeEntity entity);
    public partial FinancialAttributeFilter MapToFilter(FinancialAttributeEntity entity);
    public partial FinancialAttributeQuery MapToQuery(FinancialAttributeFilter filter);

    public FinancialAttributeEntity MapToEntity(FinancialAttributeModel model)
        => model.Type switch
        {
            MetadataType.Currency => new Currency { Name = model.Name },
            MetadataType.PaymentFrequency => new PaymentFrequency { Name = model.Name },
            _ => throw new BadRequestException($"FinancialAttribute type '{model.Type}' is not supported.")
        };

    public FinancialAttributeEntity MapData(FinancialAttributeModel model, FinancialAttributeEntity entity)
    {
        if (entity.Type != model.Type)
            throw new BadRequestException($"FinancialAttribute type cannot be changed from '{entity.Type}' to '{model.Type}'.");

        entity.Name = model.Name;

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}