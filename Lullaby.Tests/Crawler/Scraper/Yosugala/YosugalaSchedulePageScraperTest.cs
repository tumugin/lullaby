namespace Lullaby.Tests.Crawler.Scraper.Yosugala;

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
    }
}
