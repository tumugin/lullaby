namespace Lullaby.Tests.Controller;

using System.Net;

public class ScalarControllerTest : BaseWebTest
{
    [Test]
    public async Task TestSwaggerEndpoint()
    {
        using var result = await this.Client.GetAsync("/scalar");
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task TestSwaggerApiEndpoint()
    {
        using var result = await this.Client.GetAsync("/openapi/v1.json");
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
