using Apex.Invest.Domain.Enums;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Bogus;

namespace Apex.Invest.Features.Tests.Modules.Bonds.Details.Fakers;

public class BondOperationFaker
{
    public static Faker<BondOperationModel> OperationFaker() => new Faker<BondOperationModel>()
        .RuleFor(o => o.Type, f => f.PickRandom<OperationType>())
        .RuleFor(o => o.Date, f => DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-f.IndexFaker % 365)))
        .RuleFor(o => o.Price, f => Math.Round(f.Random.Decimal(90m, 110m), 2))
        .RuleFor(o => o.Count, f => f.Random.Int(1, 100));
}