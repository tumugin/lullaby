namespace Lullaby.Tests.Controller;

using System.Net;

public class SwaggerControllerTest : BaseWebTest
{
    [Test]
    public async Task TestSwaggerEndpoint()
    {
        var result = await this.Client.GetAsync("/swagger");
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task TestSwaggerApiEndpoint()
    {
        var result = await this.Client.GetAsync("/swagger/v1/swagger.json");
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
