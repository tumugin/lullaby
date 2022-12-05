namespace Lullaby.Tests.Controller.Api.Events;

using System.Net;
using System.Net.Http.Json;
using Lullaby.Crawler.Groups;
using Responses.Api.Events;
using Seeder;

public class GroupEventControllerTest : BaseWebTest
{
    [Test]
    public async Task GetTest()
    {
        await new EventSeeder(this.Context).SeedEvent(null, null);
        var result =
            await this.Client.GetFromJsonAsync<GroupEventsGetResponse>(
                $"api/events/{Aoseka.GroupKeyConstant}"
            );
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Events.Count, Is.EqualTo(1));
    }

    [Test]
    public Task GetNotFoundTest()
    {
        var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await this.Client.GetFromJsonAsync<GroupEventsGetResponse>(
                $"api/events/appare"
            );
        });
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        return Task.CompletedTask;
    }
}
