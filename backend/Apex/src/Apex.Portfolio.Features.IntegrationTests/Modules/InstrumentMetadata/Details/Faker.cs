using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;

namespace Apex.Portfolio.Features.IntegrationTests.Modules.InstrumentMetadata.Details;

public static class Faker
{
    public static InstrumentMetadataModel FakeModel(MetadataType? type = null, int id = 0) => new Bogus.Faker<InstrumentMetadataModel>()
        .RuleFor(m => m.Id, _ => id)
        .RuleFor(m => m.Type, f => type ?? f.PickRandom<MetadataType>())
        .RuleFor(m => m.Name, f => $"ATTR_{f.Random.String2(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}")
        .Generate();
}