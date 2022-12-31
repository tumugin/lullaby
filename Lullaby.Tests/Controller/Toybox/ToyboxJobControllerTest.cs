namespace Lullaby.Tests.Controller.Toybox;

using System.Net;

public class ToyboxJobControllerTest : BaseWebTest
{
    [Test]
    public async Task TestToyboxJobControllerHidden()
    {
        var response = await this.Client.GetAsync("/toybox/job");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
