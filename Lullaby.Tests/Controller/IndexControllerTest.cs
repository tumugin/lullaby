namespace Lullaby.Tests.Controller;

using System.Net;

public class IndexControllerTest : BaseWebTest
{
    [Test]
    public async Task TestIndex()
    {
        using var result = await this.Client.GetAsync("/");
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}
