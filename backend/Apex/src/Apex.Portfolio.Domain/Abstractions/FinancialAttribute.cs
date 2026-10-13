using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Domain.Abstractions;

public abstract class FinancialAttribute
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}
