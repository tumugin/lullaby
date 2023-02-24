namespace Lullaby.Tests.Seeder;

using System.Globalization;
using Data;
using Lullaby.Crawler.Events;
using Models;

public class EventSeeder
{
    private LullabyContext LullabyContext { get; }

    public EventSeeder(LullabyContext lullabyContext) => this.LullabyContext = lullabyContext;

    public async Task<Event> SeedEvent(Event seedEvent)
    {
        var result = await this.LullabyContext.Events.AddAsync(seedEvent);
        return result.Entity;
    }

    public async Task<Event> SeedEvent(
        DateTimeOffset? eventStarts,
        DateTimeOffset? eventEnds,
        string groupKey = "aoseka",
        string eventName = "群青の世界×MARQUEE 定期公演 青の記録vol.11",
        string eventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
        string eventPlace = "Spotify O-nest",
        EventType eventType = EventType.OneMan
    )
    {
        var result = await this.LullabyContext.Events.AddAsync(new Event
        {
            GroupKey = groupKey,
            EventStarts = eventStarts ?? DateTimeOffset.Parse(
                "2022-11-15 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            EventEnds = eventEnds ?? DateTimeOffset.Parse(
                "2022-11-15 21:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            IsDateTimeDetailed = true,
            EventName = eventName,
            EventDescription = eventDescription,
            EventPlace = eventPlace,
            EventType = eventType,
            UpdatedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        });
        await this.LullabyContext.SaveChangesAsync();
        return result.Entity;
    }
}
