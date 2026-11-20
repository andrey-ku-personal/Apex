namespace Apex.Users.Features.IntegrationTests.Modules;

public class BaseTests(SliceFixture fixture) : IAsyncLifetime
{
    public SliceFixture Fixture => fixture;

    public virtual Task InitializeAsync() => fixture.InitializeAsync();
    public virtual Task DisposeAsync() => Task.CompletedTask;
}