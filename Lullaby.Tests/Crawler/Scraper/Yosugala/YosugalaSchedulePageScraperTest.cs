namespace Lullaby.Tests.Crawler.Scraper.Yosugala;

using System.Globalization;
using Lullaby.Crawler.Scraper.Yosugala;
using RestSharp;
using RichardSzalay.MockHttp;

public class YosugalaSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        var schedulePageFileContent =
            await this.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Yosugala.yosugala-test-page.html");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(YosugalaSchedulePageScraper.SchedulePageUrl)
            .Respond("text/html", schedulePageFileContent);
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        var scraper = new YosugalaSchedulePageScraper { Client = client };
        var result = await scraper.ScrapeAsync();

        Assert.That(result.Count, Is.EqualTo(9));

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
