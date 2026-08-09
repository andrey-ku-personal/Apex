using Apex.Invest.Domain.Entities;
using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Bogus;

namespace Apex.Invest.Features.Tests.Modules.Bonds.Details.Fakers;

public class BondDetailsFaker
{
    public BondDetailsModel FakeModel(FinancePlatform[] platforms, int id = 0) => new Faker<BondDetailsModel>()
        .RuleFor(c => c.Id, f => id)
        .RuleFor(c => c.PlatformId, f => f.PickRandom(platforms).Id)
        .RuleFor(c => c.Ticker, f => $"BOND_{f.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .RuleFor(c => c.Issuer, f => f.Company.CompanyName())
        .RuleFor(c => c.Status, f => f.PickRandom<Status>())
        .RuleFor(c => c.Currency, f => f.PickRandom<Currency>())
        .RuleFor(c => c.ParPrice, f => Math.Round(f.Random.Decimal(100m, 10000m), 2))
        .RuleFor(c => c.CouponRate, f => Math.Round(f.Random.Decimal(1m, 20m), 2))
        .RuleFor(c => c.PaymentFrequence, f => f.PickRandom<PaymentFrequence>())
        .RuleFor(c => c.NextCouponDate, f => f.Random.Int(1, 31))
        .RuleFor(c => c.MaturityDate, f => DateTime.SpecifyKind(f.Date.Between(DateTime.Today.AddYears(1), DateTime.Today.AddYears(5)).Date, DateTimeKind.Utc))
        .RuleFor(c => c.TotalQuantity, _ => 0)
        .RuleFor(c => c.AveragePurchasePrice, _ => 0m)
        .RuleFor(c => c.TotalInvested, _ => 0m)
        .RuleFor(c => c.Operations, f => BondOperationFaker.OperationFaker().Generate(f.Random.Int(20, 100)))
        .Generate();
}