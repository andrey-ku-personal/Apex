using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.FinancialAttribute.Details;

public static class Faker
{
    public static FinancialAttributeModel FakeModel(MetadataType? type = null, int id = 0) => new Bogus.Faker<FinancialAttributeModel>()
        .RuleFor(m => m.Id, _ => id)
        .RuleFor(m => m.Type, f => type ?? f.PickRandom<MetadataType>())
        .RuleFor(m => m.Name, f => $"ATTR_{f.Random.String2(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .Generate();
}