using Apex.Invest.Domain.Enums;

namespace Apex.Invest.Features.Modules.Bonds.Details.Models;

public class BondDetailsModel
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;

    public Currency Currency { get; set; }
    public decimal ParPrice { get; set; }
    public decimal CouponRate { get; set; }
    public int NextCouponDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public PaymentFrequence PaymentFrequence { get; set; }
    public Status Status { get; set; }

    public int TotalQuantity { get; set; }
    public decimal AveragePurchasePrice { get; set; }
    public decimal TotalInvested { get; set; }

    public List<BondOperationModel>? Operations { get; set; }
}
