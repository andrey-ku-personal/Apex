using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Bonds.Details.Models;

namespace Apex.Invest.Features.Modules.Bonds.List.Models;

public class BondListModel
{
    public int Id { get; set; }
    public string Ticker { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public decimal CouponRate { get; set; }
    public Currency Currency { get; set; }
    public Status Status { get; set; }
    public List<BondOperationModel> Operations { get; set; } = [];
}
