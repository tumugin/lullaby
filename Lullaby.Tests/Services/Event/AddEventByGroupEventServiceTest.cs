namespace Lullaby.Tests.Services.Event;

using Lullaby.Crawler.Events;
using Lullaby.Services.Event;

public class AddEventByGroupEventServiceTest : BaseDatabaseTest
{
    private AddEventByGroupEventService Service { get; set; }

    [SetUp] public void Setup()
    {
        Service = new AddEventByGroupEventService(Context);
    }

    [Test] public async Task TestExecuteWithDetailedEventDateTime()
    {
        var result = await Service.execute("aoseka",
            new GroupEvent
            {
                EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.11",
                EventPlace = "Spotify O-nest",
                EventType = EventType.ONE_MAN,
                EventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
                EventDateTime = new DetailedEventDateTime
                {
                    EventStartDateTime = DateTime.Parse("2022-11-15 19:30:00+09:00"),
                    EventEndDateTime = DateTime.Parse("2022-11-15 21:30:00+09:00")
                }
            }
        );
        Assert.That(result.ID, Is.GreaterThan(0L));
        Assert.That(result.EventStarts, Is.EqualTo(DateTime.Parse("2022-11-15 19:30:00+09:00")));
        Assert.That(result.EventEnds, Is.EqualTo(DateTime.Parse("2022-11-15 21:30:00+09:00")));
    }

    [Test] public async Task TestExecuteWithUnDetailedEventDateTime()
    {
        var result = await Service.execute("aoseka",
            new GroupEvent
            {
                EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.11",
                EventPlace = "Spotify O-nest",
                EventType = EventType.ONE_MAN,
                EventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
                EventDateTime = new UnDetailedEventDateTime
                {
                    EventStartDate = DateTime.Parse("2022-11-15 00:00:00+09:00"),
                    EventEndDate = DateTime.Parse("2022-11-15 00:00:00+09:00")
                }
            }
        );
        Assert.That(result.ID, Is.GreaterThan(0L));
        Assert.That(result.EventStarts, Is.EqualTo(DateTime.Parse("2022-11-15 00:00:00+09:00")));
        Assert.That(result.EventEnds, Is.EqualTo(DateTime.Parse("2022-11-15 00:00:00+09:00")));
    }
}
