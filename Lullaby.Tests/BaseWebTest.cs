namespace Lullaby.Tests;

public class BaseWebTest : BaseDatabaseTest
{
    protected HttpClient Client { get; set; } = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.Client = new TestingWebApplicationFactory().CreateClient();
    }
}
