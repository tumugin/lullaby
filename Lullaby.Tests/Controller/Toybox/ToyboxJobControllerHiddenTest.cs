namespace Lullaby.Tests.Controller.Toybox;

using System.Net;

public class ToyboxJobControllerHiddenTest : BaseWebTest
{
    protected override string Environment => "Production";

    [Test]
    public async Task TestToyboxJobControllerHidden()
    {
        var response = await this.Client.GetAsync("/toybox/job");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
