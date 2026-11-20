using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.Bonds.Operations;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.Bonds.Operations;

public static class Faker
{
    public static string FakeUserId() => $"usr_{Guid.NewGuid():N}";

    public static T FakeOperation<T>(int bondId, string userId, OperationType type) where T : BondOperationModel, new() => new Bogus.Faker<T>()
        .RuleFor(o => o.BondId, _ => bondId)
        .RuleFor(o => o.UserId, _ => userId)
        .RuleFor(o => o.Type, _ => type)
        .RuleFor(o => o.Date, f => DateOnly.FromDateTime(f.Date.Past(2)))
        .RuleFor(o => o.Count, f => f.Random.Int(1, 1000))
        .RuleFor(o => o.Price, f => Math.Round(f.Random.Decimal(1, 100_000), 2))
        .RuleFor(o => o.Commission, f => Math.Round(f.Random.Decimal(0, 500), 2))
        .Generate();
}
