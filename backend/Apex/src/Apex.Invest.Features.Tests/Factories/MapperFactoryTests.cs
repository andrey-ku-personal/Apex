using Shouldly;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Bonds.Details.Mapper;

namespace Apex.Invest.Features.Tests.Factories;

public class MapperFactoryTests
{
    [Fact]
    public void Register_Mapper_Should_Register_Mapper_For_Type()
    {
        var factory = new MapperFactory();

        var mapperInstance = new BondDetailsMapper();

        factory.RegisterMapper(() => mapperInstance);

        var result = factory.GetMapper<BondDetailsMapper>();

        result.ShouldBe(mapperInstance);
    }

    [Fact]
    public void Get_Mapper_Should_Create_New_Instance_Each_Time()
    {
        var factory = new MapperFactory();

        factory.RegisterMapper(() => new BondDetailsMapper());

        var mapper1 = factory.GetMapper<BondDetailsMapper>();
        var mapper2 = factory.GetMapper<BondDetailsMapper>();

        mapper1.ShouldNotBeNull();
        mapper2.ShouldNotBeNull();
        mapper1.ShouldNotBeSameAs(mapper2);
    }

    [Fact]
    public void GetMapper_Should_Throw_When_Not_Registered()
    {
        var factory = new MapperFactory();

        var exception = Should.Throw<InvalidOperationException>(factory.GetMapper<BondDetailsMapper>);
    }

    [Fact]
    public void Register_Mapper_Should_Update_Existing_Registration()
    {
        var factory = new MapperFactory();

        var firstMapper = new BondDetailsMapper();
        var secondMapper = new BondDetailsMapper();

        factory.RegisterMapper(() => firstMapper);
        factory.RegisterMapper(() => secondMapper);

        var result = factory.GetMapper<BondDetailsMapper>();

        result.ShouldBe(secondMapper);
        result.ShouldNotBe(firstMapper);
    }

    [Fact]
    public async Task Get_Mapper_Should_Be_Thread_Safe()
    {
        var factory = new MapperFactory();
        factory.RegisterMapper(() => new BondDetailsMapper());
        var tasks = new List<Task<BondDetailsMapper>>();

        for (int i = 0; i < 50; i++)
            tasks.Add(Task.Run(factory.GetMapper<BondDetailsMapper>));

        await Task.WhenAll([.. tasks]);

        tasks.All(t => t.Result != null).ShouldBeTrue();
    }
}
