namespace Lullaby.Tests.Controller.Ical.Events;

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
            await this.Client.GetAsync(
                $"ical/events/{Aoseka.GroupKeyConstant}"
            );
        Assert.Multiple(async () =>
        {
            Assert.That(await result.Content.ReadAsStringAsync(), Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(
                result.Headers.First(v => v.Key == "Content-Type").Value.First(),
                Is.EqualTo("text/calendar")
            );
        });
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
            $"ical/events/{Aoseka.GroupKeyConstant}",
            new Dictionary<string, StringValues>
            {
                { "eventTypes", new StringValues(new[] { "Fes", "Battle", "BattleOrFes" }) },
                { "eventStartsFrom", "2022-10-15 19:30:00+09:00" },
                { "eventEndsAt", "2022-10-15 19:30:00+09:00" }
            }
        );
        var result = await this.Client.GetAsync(requestUri);

        Assert.Multiple(async () =>
        {
            Assert.That(await result.Content.ReadAsStringAsync(), Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(
                result.Headers.First(v => v.Key == "Content-Type").Value.First(),
                Is.EqualTo("text/calendar")
            );
        });
    }

    [Test]
    public Task GetNotFoundTest()
    {
        var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await this.Client.GetFromJsonAsync<GroupEventsGetResponse>(
                $"ical/events/appare"
            );
        });
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        return Task.CompletedTask;
    }
}
