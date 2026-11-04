using Apex.Portfolio.Domain.Abstractions;
using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Domain.Entities.Bond;

public class BondOperation : AnalyticalEntity
{
    public int Id { get; set; }
    public int BondId { get; set; }
    public string UserId { get; set; } = null!;
    public OperationType Type { get; set; }
    public DateOnly Date { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal Commission { get; set; }

    public virtual Bond Bond { get; set; } = default!;
}
