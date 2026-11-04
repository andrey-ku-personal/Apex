using Apex.Portfolio.Domain.Enums;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Models;

public class BondOperationListModel
{
    public int Id { get; set; }
    public int BondId { get; set; }
    public OperationType Type { get; set; }
    public DateOnly Date { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal Commission { get; set; }
}
