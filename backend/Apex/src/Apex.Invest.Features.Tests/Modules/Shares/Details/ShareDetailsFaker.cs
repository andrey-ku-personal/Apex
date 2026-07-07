using Apex.Invest.Domain.Entities;
using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Bogus;

namespace Apex.Invest.Features.Tests.Modules.Shares.Details;

public class ShareDetailsFaker
{
    public ShareDetailsModel FakeModel(FinancePlatform[] platforms, int id = 0) => new Faker<ShareDetailsModel>()
        .RuleFor(c => c.Id, f => id)
        .RuleFor(c => c.PlatformId, f => f.PickRandom(platforms).Id)
        .RuleFor(c => c.Ticker, f => $"SHARE_{f.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .RuleFor(c => c.Issuer, f => f.Company.CompanyName())
        .RuleFor(c => c.PurchaseDate, f => f.Date.Past(1).ToUniversalTime())
        .RuleFor(c => c.Status, f => f.PickRandom<Status>())
        .RuleFor(c => c.Currency, f => f.PickRandom<Currency>())
        .RuleFor(c => c.MarketPrice, f => Math.Round(f.Random.Decimal(100m, 10000m), 2))
        .RuleFor(c => c.Quantity, f => f.Random.Int(1, 1000))
        .RuleFor(c => c.BrokerCommission, f => Math.Round(f.Random.Decimal(0m, 50m), 2))
        .RuleFor(c => c.DividendRate, f => Math.Round(f.Random.Decimal(0m, 10m), 2))
        .Generate();
}
