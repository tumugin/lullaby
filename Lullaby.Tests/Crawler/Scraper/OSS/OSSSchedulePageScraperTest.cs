namespace Lullaby.Tests.Crawler.Scraper.OSS;

using System.Globalization;
using AngleSharp;
using Common.Crawler.Events;
using Common.Crawler.Scraper.OSS;
using RestSharp;
using RichardSzalay.MockHttp;

public class OssSchedulePageScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var schedulePageFileContent =
            await ScraperTestUtils.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.OSS.oss-test-page.html");
        using var client = new RestClient(new RestClientOptions
        {
            ConfigureMessageHandler = _ =>
            {
                var mockHttp = new MockHttpMessageHandler();
                mockHttp.When(OssSchedulePageScraper.SchedulePageUrlConstant)
                    .Respond("text/html", schedulePageFileContent);
                return mockHttp;
            }
        });

        using var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        var scraper = new OssSchedulePageScraper(
            client,
            browsingContext,
            new EventTypeDetector()
        );
        var result = await scraper.ScrapeAsync(default);

        Assert.That(result, Has.Count.EqualTo(8));

        // 【海老原天生誕祭】暴飲暴食
        var testEvent = result.First(v => v.EventName == "【海老原天生誕祭】暴飲暴食");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(testEvent.EventName, Is.EqualTo("【海老原天生誕祭】暴飲暴食"));
            Assert.That(
                testEvent.EventDescription,
                Is.EqualTo(@"チケット代: adv ¥2,000+1D
出演: On the treat Super Season

https://t.livepocket.jp/e/g_16b")
            );
            Assert.That(
                testEvent.EventDateTime.EventStartDateTimeOffset.ToString("o", CultureInfo.InvariantCulture),
                Is.EqualTo("2023-02-18T15:15:00.0000000+09:00")
            );
            Assert.That(
                testEvent.EventDateTime.EventEndDateTimeOffset.ToString("o", CultureInfo.InvariantCulture),
                Is.EqualTo("2023-02-18T19:15:00.0000000+09:00")
            );
        }
    }
}
