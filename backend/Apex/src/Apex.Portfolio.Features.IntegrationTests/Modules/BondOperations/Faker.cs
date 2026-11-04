using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.BondOperations;

public static class Faker
{
    public static string FakeUserId() => $"usr_{Guid.NewGuid():N}";

    public static BondOperationModel FakeOperationModel(int bondId, string userId, OperationType type) => new Bogus.Faker<BondOperationModel>()
        .CustomInstantiator(f => new BondOperationModel
        {
            BondId = bondId,
            UserId = userId,
            Type = type,
            Date = DateOnly.FromDateTime(f.Date.Past(2)),
            Count = f.Random.Int(1, 1000),
            Price = Math.Round(f.Random.Decimal(1, 100_000), 2),
            Commission = Math.Round(f.Random.Decimal(0, 500), 2)
        })
        .Generate();
}
