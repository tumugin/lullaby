namespace Lullaby.Tests.Crawler.Scraper.Kolokol;

using System.Globalization;
using AngleSharp;
using Common.Crawler.Events;
using Common.Crawler.Scraper.Kolokol;
using RestSharp;
using RichardSzalay.MockHttp;

public class KolokolSchedulePageScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        // mock html request
        var testFutureFileContent =
            await ScraperTestUtils.GetTestFileFromManifest(
                "Lullaby.Tests.Crawler.Scraper.Kolokol.kolokol-test-page.html");
        var testPastFileContent =
            await ScraperTestUtils.GetTestFileFromManifest(
                "Lullaby.Tests.Crawler.Scraper.Kolokol.kolokol-past-schedule-test-page.html");
        using var client = new RestClient(new RestClientOptions
        {
            ConfigureMessageHandler = _ =>
            {
                var mockHttp = new MockHttpMessageHandler();
                KolokolSchedulePageScraper.SchedulePageUrls.ToList().ForEach(pageUrl =>
                {
                    mockHttp
                        .When(pageUrl)
                        .Respond("text/html", pageUrl.Contains("/past/") ? testPastFileContent : testFutureFileContent);
                });
                return mockHttp;
            }
        });

        using var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        var scraper = new KolokolSchedulePageScraper(
            client,
            browsingContext,
            new EventTypeDetector()
        );
        var result = await scraper.ScrapeAsync(default);

        Assert.That(result, Has.Count.EqualTo(50));
        var soul = result.FirstOrDefault(e => e.EventName == "「SOUL」");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(soul?.EventName, Is.EqualTo("「SOUL」"));
            Assert.That(soul?.EventPlace, Is.EqualTo("SHIBUYA CLUB QUATTRO"));
            Assert.That(
                (soul?.EventDateTime as DetailedEventDateTime)!
                .EventStartDateTime
                .ToString(CultureInfo.InvariantCulture),
                Is.EqualTo("04/22/2026 12:45:00 +09:00")
            );
            Assert.That(
                (soul.EventDateTime as DetailedEventDateTime)!
                .EventEndDateTime
                .ToString(CultureInfo.InvariantCulture),
                Is.EqualTo("04/22/2026 16:45:00 +09:00")
            );
        }
    }
}
