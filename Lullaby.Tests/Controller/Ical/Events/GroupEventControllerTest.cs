﻿namespace Lullaby.Tests.Controller.Ical.Events;

using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Seeder;

public class GroupEventControllerTest : BaseWebTest
{
    [Test]
    public async Task GetTest()
    {
        await new EventSeeder(this.Context).SeedEvent(null, null);
        using var result =
            await this.Client.GetAsync(
                $"ical/events/aoseka"
            );
        var content = await result.Content.ReadAsStringAsync();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(content, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Content.Headers.ContentType?.MediaType, Is.EqualTo("text/calendar"));
        }
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
            $"ical/events/aoseka",
            new Dictionary<string, StringValues>
            {
                { "eventTypes", new StringValues(new[] { "Fes", "Battle", "BattleOrFes" }) },
                { "eventStartsFrom", "2022-10-15 19:30:00+09:00" },
                { "eventEndsAt", "2022-10-15 19:30:00+09:00" }
            }
        );
        using var result = await this.Client.GetAsync(requestUri);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(await result.Content.ReadAsStringAsync(), Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Content.Headers.ContentType?.MediaType, Is.EqualTo("text/calendar"));
        }
    }

    [Test]
    public async Task GetNotFoundTest()
    {
        using var result = await this.Client.GetAsync("ical/events/appare");
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
