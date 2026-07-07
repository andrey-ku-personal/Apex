using Apex.Invest.Domain.Entities;
using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Bogus;

namespace Apex.Invest.Features.Tests.Modules.Deposits.Details;

public class DepositDetailsFaker
{
    public DepositDetailsModel FakeModel(FinancePlatform[] platforms, int id = 0) => new Faker<DepositDetailsModel>()
        .RuleFor(c => c.Id, f => id)
        .RuleFor(c => c.PlatformId, f => f.PickRandom(platforms).Id)
        .RuleFor(c => c.Ticker, f => $"DEP_{f.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .RuleFor(c => c.Issuer, f => f.Company.CompanyName())
        .RuleFor(c => c.PurchaseDate, f => f.Date.Past(1).ToUniversalTime())
        .RuleFor(c => c.Status, f => f.PickRandom<Status>())
        .RuleFor(c => c.InterestRate, f => Math.Round(f.Random.Decimal(1m, 20m), 2))
        .RuleFor(c => c.Frequence, f => f.PickRandom<PaymentFrequence>())
        .RuleFor(c => c.IsReplenishable, f => f.Random.Bool())
        .RuleFor(c => c.FirstPaymentDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddDays(20)).ToUniversalTime())
        .RuleFor(c => c.MaturityDate, (f, o) => o.FirstPaymentDate.AddDays(f.Random.Int(31, 365)))
        .RuleFor(c => c.SaleDate, _ => null)
        .RuleFor(c => c.IsTaxFree, f => f.Random.Bool())
        .RuleFor(c => c.Capitalization, f => f.Random.Bool())
        .Generate();
}
