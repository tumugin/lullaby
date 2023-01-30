namespace Lullaby.Tests.Crawler.Scraper.OSS;

using System.Globalization;
using Lullaby.Crawler.Scraper.OSS;
using RestSharp;
using RichardSzalay.MockHttp;

public class OSSSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var schedulePageFileContent =
            await this.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.OSS.oss-test-page.html");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(OSSSchedulePageScraper.SchedulePageUrlConstant)
            .Respond("text/html", schedulePageFileContent);
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        var scraper = new OSSSchedulePageScraper { Client = client };
        var result = await scraper.ScrapeAsync();

        Assert.That(result.Count, Is.EqualTo(8));

        // 【海老原天生誕祭】暴飲暴食
        var testEvent = result.First(v => v.EventName == "【海老原天生誕祭】暴飲暴食");
        Assert.Multiple(() =>
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
        });
    }
}
