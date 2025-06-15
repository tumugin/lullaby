namespace Lullaby.Tests.Crawler.Scraper.TimeTree;

using System.Globalization;
using Common.Crawler.Scraper.TimeTree;
using RichardSzalay.MockHttp;

public class TimeTreeApiClientTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var scheduleJson =
            await ScraperTestUtils.GetTestFileFromManifest(
                "Lullaby.Tests.Crawler.Scraper.TimeTree.time-tree-test-json.json");
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Fallback
            .Respond("application/json", scheduleJson);
        using var httpClient = mockHttp.ToHttpClient();

        var timeTreeApiClient = new TimeTreeApiClient(httpClient);
        var result = await timeTreeApiClient.GetEventsAsync(
            "tenhana_sj",
            DateTimeOffset.Parse("2024-01-01 00:00:00+09:00", CultureInfo.InvariantCulture),
            DateTimeOffset.Parse("2024-01-31 00:00:00+09:00", CultureInfo.InvariantCulture),
            null,
            default
        );
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.HasNextPage, Is.False);
            Assert.That(result.NextPageCursor, Is.Null);
            Assert.That(result.Schedules, Has.Count.EqualTo(22));

            Assert.That(result.Schedules[0].Id, Is.EqualTo("2341961480453499250"));
            Assert.That(result.Schedules[0].Title, Is.EqualTo("TOKYOGIRLSGIRLSmini!!"));
            Assert.That(result.Schedules[0].Description,
                Is.EqualTo(
                    "1/29(月)\nTOKYO GIRLS GIRLS mini!!\n\n池袋Studio Mixa\n\n出演時間\ud83c\udfa4 17:25-17:45\n特典会\ud83d\udcf8 17:55-18:55(B)\n\nお目当て特典：TikTok撮影券\ud83d\udc95\n\n\ud83c\udfab\nhttps://t.livepocket.jp/e/tgg__0129"
                )
            );
            Assert.That(result.Schedules[0].ImageUrls, Has.Count.EqualTo(2));
            Assert.That(result.Schedules[0].LocationName, Is.Null);
            Assert.That(result.Schedules[0].StartAt, Is.EqualTo(
                new DateTimeOffset(2024, 1, 29, 0, 0, 0, TimeSpan.Zero)
            ));
            Assert.That(result.Schedules[0].EndAt, Is.EqualTo(
                new DateTimeOffset(2024, 1, 29, 0, 0, 0, TimeSpan.Zero)
            ));
            Assert.That(result.Schedules[0].UntilAt, Is.EqualTo(
                new DateTimeOffset(2024, 1, 29, 23, 59, 59, TimeSpan.Zero)
            ));
        }
    }
}
