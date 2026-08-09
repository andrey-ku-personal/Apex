using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Domain.Entities.Bond;

public class BondOperation
{
    public int Id { get; set; }
    public int BondId { get; set; }
    public OperationType Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }

    public virtual Bond Bond { get; set; } = default!;
}
