namespace Lullaby.Tests.Crawler.Scraper.Kolokol;

using Lullaby.Crawler.Scraper.Kolokol;
using RestSharp;
using RichardSzalay.MockHttp;

public class KolokolSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        // mock html request
        var testFileContent =
            await this.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Kolokol.kolokol-test-page.html");
        var mockHttp = new MockHttpMessageHandler();
        KolokolSchedulePageScraper.SchedulePageUrls.ToList().ForEach(pageUrl =>
        {
            // 全て同じようなモノが入っているので、全部同じ内容でモックする
            mockHttp
                .When(pageUrl)
                .Respond("text/html", testFileContent);
        });
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        var scraper = new KolokolSchedulePageScraper { Client = client };
        var result = await scraper.ScrapeAsync();

        Assert.That(result.Count, Is.EqualTo(40));
    }
}
