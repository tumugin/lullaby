namespace Lullaby.Tests.Crawler.Scraper.TimeTree;

using System.Globalization;
using Lullaby.Crawler.Scraper.TimeTree;
using RichardSzalay.MockHttp;

public class TimeTreeApiClientTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var scheduleJson =
            await GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.TimeTree.time-tree-test-json.json");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Fallback
            .Respond("application/json", scheduleJson);

        var timeTreeApiClient = new TimeTreeApiClient(mockHttp.ToHttpClient());
        var result = await timeTreeApiClient.GetEventsAsync(
            "54197",
            DateTimeOffset.Parse("2023-07-01 00:00:00+09:00", CultureInfo.InvariantCulture),
            DateTimeOffset.Parse("2023-07-31 00:00:00+09:00", CultureInfo.InvariantCulture),
            null,
            default
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.HasNextPage, Is.False);
            Assert.That(result.NextPageCursor, Is.Null);
            Assert.That(result.Schedules, Has.Count.EqualTo(5));

            Assert.That(result.Schedules[0] is
            {
                Id: "2185012422837720456",
                Title: "超NATSUZOME2023",
                Overview:
                "7/1(土)、7/2(日)\n｢超NATSUZOME2023｣\n@ 海浜幕張公園Gブロック\n\nOPEN 9:00/START 10:00\n\n#てんはな \n\n🎫先行抽選受付中\n一般\nticketvillage.jp/events/12220\nVIP\nr-t.jp/natsuzome2023",
                ImageUrls: ["https://attachments.timetreeapp.com/public_event/a692/2023-06-01/0-1685641777958.jpg"],
                LocationName: null,
                StartAt: { Year: 2023, Month: 7, Day: 1, Hour: 0, Minute: 0, Second: 0, Offset.Hours: 0 },
                EndAt: { Year: 2023, Month: 7, Day: 2, Hour: 0, Minute: 0, Second: 0, Offset.Hours: 0 },
                UntilAt: { Year: 2023, Month: 7, Day: 2, Hour: 23, Minute: 59, Second: 59, Offset.Hours: 0 },
            }, Is.True);
        });
    }
}
