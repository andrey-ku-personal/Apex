using Apex.Portfolio.Features.Modules.Bonds.Details;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Details;

public static class Faker
{
    public static string FakeUserId() => $"usr_{Guid.NewGuid():N}";

    public static T FakeModel<T>(int currencyId, int paymentFrequencyId, int id = 0) where T : BondModel, new() => new Bogus.Faker<T>()
        .RuleFor(m => m.Id, _ => id)
        .RuleFor(m => m.UserId, _ => FakeUserId())
        .RuleFor(m => m.Ticker, f => $"BND_{f.Random.String2(8, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .RuleFor(m => m.Issuer, f => f.Company.CompanyName())
        .RuleFor(m => m.CurrencyId, _ => currencyId)
        .RuleFor(m => m.PaymentFrequencyId, _ => paymentFrequencyId)
        .RuleFor(m => m.IssueDate, f => DateOnly.FromDateTime(f.Date.Past(3)))
        .RuleFor(m => m.MaturityDate, f => DateOnly.FromDateTime(f.Date.Future(3)))
        .RuleFor(m => m.PaymentDay, f => f.Random.Int(1, 28))
        .RuleFor(m => m.ParPrice, f => Math.Round(f.Random.Decimal(100, 10_000), 2))
        .RuleFor(m => m.Rate, f => Math.Round(f.Random.Decimal(0, 20), 4))
        .RuleFor(m => m.IsRefinancingLinked, f => f.Random.Bool())
        .Generate();
}
