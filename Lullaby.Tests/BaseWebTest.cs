namespace Lullaby.Tests;

public class BaseWebTest : BaseDatabaseTest
{
    protected HttpClient Client { get; set; } = null!;

    [SetUp]
    public void WebServerSetUp()
    {
        this.Client = new TestingWebApplicationFactory().CreateClient();
    }
}
