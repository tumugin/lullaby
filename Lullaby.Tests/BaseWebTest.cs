namespace Lullaby.Tests;

public abstract class BaseWebTest : BaseDatabaseTest
{
    protected HttpClient Client { get; private set; } = null!;
    protected virtual string Environment => "Testing";

    [SetUp]
    public void WebServerSetUp()
        => this.Client =
            new TestingWebApplicationFactory(this.Environment, builder => BuildDbContextOptions(builder))
                .CreateClient();

    [TearDown]
    public void TearDownWebServer() => this.Client.Dispose();
}
