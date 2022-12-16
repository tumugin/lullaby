namespace Lullaby.Tests.Crawler.Groups;

using System.Globalization;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Groups;
using RestSharp;

public class TestGroup : BaseGroup
{
    public override string GroupKey => "test";
    public override string GroupName => "テスト";
    public override string CrawlCron => "0 * * * *";

    public override async Task<IEnumerable<GroupEvent>> GetEvents(RestClient restClient) =>
        await Task.Run(() => new List<GroupEvent>
        {
            new()
            {
                EventName = "TOKYO IDOL FESTIVAL 2022 supported by にしたんクリニック",
                EventPlace = "お台場・青海周辺エリア",
                EventType = EventType.Fes,
                EventDescription = "day1 test",
                EventDateTime = new DetailedEventDateTime
                {
                    EventStartDateTime = DateTimeOffset.Parse(
                        "2022-08-05 10:00:00+09:00",
                        CultureInfo.InvariantCulture
                    ),
                    EventEndDateTime = DateTimeOffset.Parse(
                        "2022-08-05 20:00:00+09:00",
                        CultureInfo.InvariantCulture
                    )
                }
            },
            new()
            {
                EventName = "TOKYO IDOL FESTIVAL 2022 supported by にしたんクリニック",
                EventPlace = "お台場・青海周辺エリア",
                EventType = EventType.Fes,
                EventDescription = "day2 test",
                EventDateTime = new DetailedEventDateTime
                {
                    EventStartDateTime = DateTimeOffset.Parse(
                        "2022-08-06 10:00:00+09:00",
                        CultureInfo.InvariantCulture
                    ),
                    EventEndDateTime = DateTimeOffset.Parse(
                        "2022-08-06 20:00:00+09:00",
                        CultureInfo.InvariantCulture
                    )
                }
            }
        });
}
