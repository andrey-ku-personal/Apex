using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Bonds.Details.Models;

public class BondOperationModel
{
    public int Id { get; set; }
    public OperationType Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
}
