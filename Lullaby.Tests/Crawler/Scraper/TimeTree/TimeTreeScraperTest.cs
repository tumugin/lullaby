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
            await ScraperTestUtils.GetTestFileFromManifest(
                "Lullaby.Tests.Crawler.Scraper.TimeTree.time-tree-test-json.json");
        using var mockHttp = new MockHttpMessageHandler();
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
            Assert.That(result, Has.Count.EqualTo(22));

            Assert.That(result[0].EventName, Is.EqualTo("TOKYOGIRLSGIRLSmini!!"));
            Assert.That(result[0].EventDescription,
                Is.EqualTo(
                    "1/29(月)\nTOKYO GIRLS GIRLS mini!!\n\n池袋Studio Mixa\n\n出演時間\ud83c\udfa4 17:25-17:45\n特典会\ud83d\udcf8 17:55-18:55(B)\n\nお目当て特典：TikTok撮影券\ud83d\udc95\n\n\ud83c\udfab\nhttps://t.livepocket.jp/e/tgg__0129"
                )
            );
            Assert.That(result[0].EventPlace, Is.Null);
            Assert.That(result[0].EventDateTime.EventStartDateTimeOffset, Is.EqualTo(
                new DateTimeOffset(2024, 1, 29, 0, 0, 0, TimeSpan.Zero)
            ));
            Assert.That(result[0].EventDateTime.EventEndDateTimeOffset, Is.EqualTo(
                new DateTimeOffset(2024, 1, 30, 0, 0, 0, TimeSpan.Zero)
            ));
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

        public override string TimeTreePublicCalendarId => "tenhana_sj";
    }
}
