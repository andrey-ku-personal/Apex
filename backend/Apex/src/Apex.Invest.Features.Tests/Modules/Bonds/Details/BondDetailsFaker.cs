using Apex.Invest.Domain.Entities;
using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Bogus;

namespace Apex.Invest.Features.Tests.Modules.Bonds.Details;

public class BondDetailsFaker
{
    public BondDetailsModel FakeModel(FinancePlatform[] platforms, int id = 0) => new Faker<BondDetailsModel>()
        .RuleFor(c => c.Id, f => id)
        .RuleFor(c => c.PlatformId, f => f.PickRandom(platforms).Id)
        .RuleFor(c => c.Ticker, f => $"BOND_{f.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .RuleFor(c => c.Issuer, f => f.Company.CompanyName())
        .RuleFor(c => c.PurchaseDate, f => f.Date.Past(1).ToUniversalTime())
        .RuleFor(c => c.Status, f => f.PickRandom<Status>())
        .RuleFor(c => c.InterestRate, f => Math.Round(f.Random.Decimal(1m, 20m), 2))
        .RuleFor(c => c.Currency, f => f.PickRandom<Currency>())
        .RuleFor(c => c.Frequence, f => f.PickRandom<PaymentFrequence>())
        .RuleFor(c => c.NominalPrice, f => Math.Round(f.Random.Decimal(100m, 10000m), 2))
        .RuleFor(c => c.MarketPrice, f => Math.Round(f.Random.Decimal(90m, 110m), 2))
        .RuleFor(c => c.Quantity, f => f.Random.Int(1, 1000))
        .RuleFor(c => c.BrokerCommission, f => Math.Round(f.Random.Decimal(0m, 50m), 2))
        .RuleFor(c => c.FirstPaymentDate, f => f.Date.Between(DateTime.Today.AddYears(2), DateTime.Today.AddYears(5)).ToUniversalTime())
        .RuleFor(c => c.MaturityDate, (f, o) => o.FirstPaymentDate.AddDays(f.Random.Int(31, 365)))
        .RuleFor(c => c.SaleDate, _ => null)
        .Generate();
}
