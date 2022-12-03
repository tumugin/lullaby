namespace Lullaby.Tests.Controller.Api.Events;

using System.Net.Http.Json;
using Lullaby.Crawler.Groups;
using Responses.Api.Events;

public class GroupEventControllerTest : BaseWebTest
{
    [Test]
    public async Task GetTest()
    {
        var result =
            await this.Client.GetFromJsonAsync<GroupEventsGetResponse>(
                $"api/events/{Aoseka.GroupKeyConstant}"
            );
        Assert.That(result, Is.Not.Null);
    }
}
