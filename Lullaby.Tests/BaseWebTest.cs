namespace Lullaby.Tests;

public class BaseWebTest : BaseDatabaseTest
{
    protected HttpClient Client { get; set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.Client = new TestingWebApplicationFactory().CreateClient();
    }
}
