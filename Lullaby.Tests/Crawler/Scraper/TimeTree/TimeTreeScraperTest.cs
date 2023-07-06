namespace Lullaby.Tests.Crawler.Scraper.TimeTree;

using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.TimeTree;
using RichardSzalay.MockHttp;

public class TimeTreeScraperTest
{
    [Test]
    public async Task TestScrapeAsync()
    {
        var scheduleJson =
            await ScraperTestUtils.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.TimeTree.time-tree-test-json.json");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Fallback
            .Respond("application/json", scheduleJson);

        var timeTreeApiClient = new TimeTreeApiClient(mockHttp.ToHttpClient());
        var testClass = new TimeTreeScraperTestImpl(
            timeTreeApiClient,
            new TimeTreeScheduleGroupEventConverter(new EventTypeDetector())
        );
        var result = await testClass.ScrapeAsync(default);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(5));
            Assert.That(result[0] is
            {
                EventName: "超NATSUZOME2023",
                EventDescription:
                "7/1(土)、7/2(日)\n｢超NATSUZOME2023｣\n@ 海浜幕張公園Gブロック\n\nOPEN 9:00/START 10:00\n\n#てんはな \n\n🎫先行抽選受付中\n一般\nticketvillage.jp/events/12220\nVIP\nr-t.jp/natsuzome2023",
                EventPlace: null,
                EventDateTime:
                {
                    EventStartDateTimeOffset:
                    { Year: 2023, Month: 7, Day: 1, Hour: 0, Minute: 0, Second: 0, Offset.Hours: 0 },
                    EventEndDateTimeOffset:
                    { Year: 2023, Month: 7, Day: 3, Hour: 0, Minute: 0, Second: 0, Offset.Hours: 0 },
                },
            }, Is.True);
        });
    }

    private sealed class TimeTreeScraperTestImpl : TimeTreeScraper
    {
        public TimeTreeScraperTestImpl(
            ITimeTreeApiClient timeTreeApiClient,
            ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter
        ) : base(timeTreeApiClient,
            timeTreeScheduleGroupEventConverter)
        {
        }

        public override string TimeTreePublicCalendarId => "54197";
    }
}
