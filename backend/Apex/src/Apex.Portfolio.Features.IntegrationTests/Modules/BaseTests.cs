namespace Apex.Portfolio.Features.IntegrationTests.Modules;

public class BaseTests(SliceFixture fixture) : IAsyncLifetime
{
    protected SliceFixture Fixture => fixture;

    public virtual Task InitializeAsync() => fixture.InitializeAsync();
    public virtual Task DisposeAsync() => Task.CompletedTask;
}