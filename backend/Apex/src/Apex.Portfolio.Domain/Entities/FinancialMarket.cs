namespace Apex.Portfolio.Domain.Entities;

public class FinancialMarket
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal RefinancingRate { get; set; }
}