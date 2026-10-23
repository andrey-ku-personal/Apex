using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Models;

public class FinancialAttributeListModel
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}
