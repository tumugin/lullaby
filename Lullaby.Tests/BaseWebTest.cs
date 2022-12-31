namespace Lullaby.Tests;

public class BaseWebTest : BaseDatabaseTest
{
    protected HttpClient Client { get; set; } = null!;
    protected virtual string Environment => "Testing";

    [SetUp]
    public void WebServerSetUp()
        => this.Client = new TestingWebApplicationFactory(this.Environment).CreateClient();
}
