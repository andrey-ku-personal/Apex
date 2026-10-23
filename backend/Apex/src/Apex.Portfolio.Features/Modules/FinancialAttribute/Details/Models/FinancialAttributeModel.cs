using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

public class FinancialAttributeModel
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}
