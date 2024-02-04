namespace Lullaby.Tests.Crawler.Scraper.Yosugala;

using System.Globalization;
using AngleSharp;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Yosugala;
using RestSharp;
using RichardSzalay.MockHttp;

public class YosugalaSchedulePageScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var schedulePageFileContent =
            await ScraperTestUtils.GetTestFileFromManifest(
                "Lullaby.Tests.Crawler.Scraper.Yosugala.yosugala-test-page.html");

        using var client = new RestClient(new RestClientOptions
        {
            ConfigureMessageHandler = _ =>
            {
                var mockHttp = new MockHttpMessageHandler();
                mockHttp.When(YosugalaSchedulePageScraper.SchedulePageUrlConstant)
                    .Respond("text/html", schedulePageFileContent);
                return mockHttp;
            }
        });

        using var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        var scraper = new YosugalaSchedulePageScraper(
            client,
            browsingContext,
            new EventTypeDetector()
        );
        var result = await scraper.ScrapeAsync(default);

        Assert.That(result, Has.Count.EqualTo(9));

        // 【yosugalaワンマンライヴ】「1 coin plus 1drink」
        var testEvent = result.First(v => v.EventName == "【yosugalaワンマンライヴ】「1 coin plus 1drink」");
        Assert.Multiple(() =>
        {
            Assert.That(testEvent.EventName, Is.EqualTo("【yosugalaワンマンライヴ】「1 coin plus 1drink」"));
            Assert.That(
                testEvent.EventDescription,
                Is.EqualTo(@"チケット代: 前売り¥500+1D
出演: yosugala

https://t.livepocket.jp/e/1c_p_1d")
            );
            Assert.That(
                testEvent.EventDateTime.EventStartDateTimeOffset.ToString("o", CultureInfo.InvariantCulture),
                Is.EqualTo("2023-01-03T19:15:00.0000000+09:00")
            );
            Assert.That(
                testEvent.EventDateTime.EventEndDateTimeOffset.ToString("o", CultureInfo.InvariantCulture),
                Is.EqualTo("2023-01-03T23:15:00.0000000+09:00")
            );
        });
    }
}
