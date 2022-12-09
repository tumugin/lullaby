namespace Lullaby.Tests.Services.Event;

using System.Globalization;
using Lullaby.Crawler.Events;
using Lullaby.Services.Event;

public class FindDuplicateEventServiceTest : BaseDatabaseTest
{
    private FindDuplicateEventService FindDuplicateEventService { get; set; } = null!;

    private AddEventByGroupEventService AddEventByGroupEventService { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.FindDuplicateEventService = new FindDuplicateEventService(this.Context);
        this.AddEventByGroupEventService = new AddEventByGroupEventService(this.Context);
    }

    [Test]
    public async Task TestExecute()
    {
        var groupEvent = new GroupEvent
        {
            EventName = "【LIVE】群青の世界×MARQUEE 定期公演 青の記録vol.11",
            EventPlace = "Spotify O-nest",
            EventType = EventType.OneMan,
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
        };
        await this.AddEventByGroupEventService.Execute("aoseka", groupEvent);
        var result = await this.FindDuplicateEventService.Execute(
            new List<FindDuplicateEventService.EventSearchQueryData>
            {
                new()
                {
                    groupKey = "aoseka",
                    eventName = groupEvent.EventName,
                    startDateTime = ((DetailedEventDateTime)groupEvent.EventDateTime).EventStartDateTime,
                    endDateTime = ((DetailedEventDateTime)groupEvent.EventDateTime).EventEndDateTime,
                }
            }
        );

        Assert.That(result.FirstOrDefault(), Is.Not.Null);
    }
}
