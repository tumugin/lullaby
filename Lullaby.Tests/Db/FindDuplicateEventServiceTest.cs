namespace Lullaby.Tests.Db;

using System.Globalization;
using Common.Crawler.Events;
using Common.Enums;
using Jobs.Db;
using Jobs.Services.Crawler.Events;

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
        await this.AddEventByGroupEventService.Execute("aoseka", groupEvent, default);
        var result = await this.FindDuplicateEventService.Execute(
            new List<IFindDuplicateEventService.EventSearchQueryData>
            {
                new()
                {
                    GroupKey = "aoseka",
                    EventName = groupEvent.EventName,
                    StartDateTime = ((DetailedEventDateTime)groupEvent.EventDateTime).EventStartDateTime,
                    EndDateTime = ((DetailedEventDateTime)groupEvent.EventDateTime).EventEndDateTime,
                }
            },
            default
        );

        Assert.That(result[0], Is.Not.Null);
    }
}
