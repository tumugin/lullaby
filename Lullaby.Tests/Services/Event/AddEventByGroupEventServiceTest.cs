namespace Lullaby.Tests.Services.Event;

using System.Globalization;
using Lullaby.Crawler.Events;
using Lullaby.Services.Event;

public class AddEventByGroupEventServiceTest : BaseDatabaseTest
{
    private AddEventByGroupEventService Service { get; set; } = null!;

    [SetUp]
    public void Setup() => this.Service = new AddEventByGroupEventService(this.Context);

    [Test]
    public async Task TestExecuteWithDetailedEventDateTime()
    {
        var result = await this.Service.Execute("aoseka",
            new GroupEvent
            {
                EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.11",
                EventPlace = "Spotify O-nest",
                EventType = EventType.ONE_MAN,
                EventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
                EventDateTime = new DetailedEventDateTime
                {
                    EventStartDateTime = DateTimeOffset.Parse(
                        "2022-11-15 19:30:00+09:00",
                        CultureInfo.InvariantCulture
                    ),
                    EventEndDateTime = DateTimeOffset.Parse(
                        "2022-11-15 21:30:00+09:00",
                        CultureInfo.InvariantCulture
                    )
                }
            }
        );
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.GreaterThan(0L));
            Assert.That(result.EventStarts,
                Is.EqualTo(DateTimeOffset.Parse("2022-11-15 19:30:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(result.EventEnds,
                Is.EqualTo(DateTimeOffset.Parse("2022-11-15 21:30:00+09:00", CultureInfo.InvariantCulture))
            );
        });
    }

    [Test]
    public async Task TestExecuteWithUnDetailedEventDateTime()
    {
        var result = await this.Service.Execute("aoseka",
            new GroupEvent
            {
                EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.11",
                EventPlace = "Spotify O-nest",
                EventType = EventType.ONE_MAN,
                EventDescription = "チケット▶︎ https://t.livepocket.jp/e/221115",
                EventDateTime = new UnDetailedEventDateTime
                {
                    EventStartDate =
                        DateTimeOffset.Parse(
                            "2022-11-15 00:00:00+09:00",
                            CultureInfo.InvariantCulture
                        ),
                    EventEndDate = DateTimeOffset.Parse(
                        "2022-11-15 00:00:00+09:00",
                        CultureInfo.InvariantCulture
                    )
                }
            }
        );
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.GreaterThan(0L));
            Assert.That(result.EventStarts,
                Is.EqualTo(DateTimeOffset.Parse("2022-11-15 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(result.EventEnds,
                Is.EqualTo(DateTimeOffset.Parse("2022-11-15 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
        });
    }
}
