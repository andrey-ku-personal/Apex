using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

public static class Faker
{
    public static T FakeModel<T>(MetadataType? type = null, int id = 0) where T : InstrumentMetadataModel, new() => new Bogus.Faker<T>()
        .RuleFor(m => m.Id, _ => id)
        .RuleFor(m => m.Type, f => type ?? f.PickRandom<MetadataType>())
        .RuleFor(m => m.Name, f => $"ATTR_{f.Random.String2(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .Generate();
}
