namespace Lullaby.Tests.Controller;

using System.Net;

public class IndexControllerTest : BaseWebTest
{
    [Test]
    public async Task TestIndex()
    {
        var result = await this.Client.GetAsync("/");
        Assert.Multiple(async () =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}
