namespace Apex.Portfolio.Features.IntegrationTests.Modules;

public class BaseTests(SliceFixture fixture) : IAsyncLifetime
{
    public virtual Task InitializeAsync() => fixture.InitializeAsync();
    public virtual Task DisposeAsync() => Task.CompletedTask;
}