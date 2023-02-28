namespace Lullaby.Tests.Controller.Api.Events;

using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Lullaby.Crawler.Groups;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
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
        Assert.That(result!.Events, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetTestWithParams()
    {
        await new EventSeeder(this.Context).SeedEvent(
            DateTimeOffset.Parse(
                "2022-11-15 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            DateTimeOffset.Parse(
                "2022-11-15 21:30:00+09:00",
                CultureInfo.InvariantCulture
            )
        );
        var requestUri = QueryHelpers.AddQueryString(
            $"api/events/{Aoseka.GroupKeyConstant}",
            new Dictionary<string, StringValues>
            {
                { "eventTypes", new StringValues(new[] { "Fes", "Battle", "BattleOrFes" }) },
                { "eventStartsFrom", "2022-10-15 19:30:00+09:00" },
                { "eventEndsAt", "2022-10-15 19:30:00+09:00" }
            }
        );
        var result =
            await this.Client.GetFromJsonAsync<GroupEventsGetResponse>(requestUri);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Events, Is.Empty);
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
        Assert.That(ex!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        return Task.CompletedTask;
    }
}
