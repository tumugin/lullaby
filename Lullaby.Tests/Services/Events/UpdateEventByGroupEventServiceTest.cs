namespace Lullaby.Tests.Services.Events;

using System.Globalization;
using Lullaby.Crawler.Events;
using Lullaby.Services.Events;
using Lullaby.Tests.Seeder;
using Microsoft.EntityFrameworkCore;

public class UpdateEventByGroupEventServiceTest : BaseDatabaseTest
{
    private UpdateEventByGroupEventService UpdateEventByGroupEventService { get; set; } = null!;
    private EventSeeder EventSeeder { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.UpdateEventByGroupEventService =
            new UpdateEventByGroupEventService(this.Context);
        this.EventSeeder = new EventSeeder(this.Context);
    }

    [Test]
    public async Task TestExecute()
    {
        var seededEvent = await this.EventSeeder.SeedEvent(
            eventStarts: DateTimeOffset.Parse(
                "2022-11-30 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            eventEnds: DateTimeOffset.Parse(
                "2022-12-01 00:30:00+09:00",
                CultureInfo.InvariantCulture
            )
        );
        var groupEvent = new GroupEvent
        {
            EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.12",
            EventPlace = "Spotify O-nest",
            EventType = EventType.OneMan,
            EventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
            EventDateTime = new DetailedEventDateTime
            {
                EventStartDateTime = DateTimeOffset.Parse(
                    "2022-12-15 19:30:00+09:00",
                    CultureInfo.InvariantCulture
                ),
                EventEndDateTime = DateTimeOffset.Parse(
                    "2022-12-15 21:30:00+09:00",
                    CultureInfo.InvariantCulture
                )
            }
        };
        await this.UpdateEventByGroupEventService.Execute(
            seededEvent,
            groupEvent,
            default
        );

        var updatedEvent = await this.Context.Events
            .Where(v => v.Id == seededEvent.Id)
            .FirstAsync();

        Assert.That(updatedEvent.EventName, Is.EqualTo("【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.12"));
    }
}
