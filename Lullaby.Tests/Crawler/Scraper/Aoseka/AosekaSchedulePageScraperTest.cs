namespace Lullaby.Tests.Crawler.Scraper.Aoseka;

using System.Globalization;
using AngleSharp;
using Database.Enums;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Aoseka;
using Lullaby.Crawler.Utility;
using RestSharp;
using RichardSzalay.MockHttp;

public class AosekaSchedulePageScraperTest
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created")]
    private static AosekaSchedulePageScraper SetUpAosekaSchedulePageScraper()
    {
        // mock html request
        var testFileContent = ScraperTestUtils.GetTestFileFromManifest(
            "Lullaby.Tests.Crawler.Scraper.Aoseka.aoseka-test-page.html"
        ).Result;

        var client = new RestClient(new RestClientOptions
        {
            ConfigureMessageHandler = _ =>
            {
                var mockHttp = new MockHttpMessageHandler();
                mockHttp
                    .When(AosekaSchedulePageScraper.SchedulePageUrl)
                    .Respond("text/html", testFileContent);
                return mockHttp;
            }
        });

        var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        return new AosekaSchedulePageScraper(
            client,
            browsingContext,
            new FullDateGuesser(),
            new EventTypeDetector()
        );
    }

    [Test]
    public async Task TestScrapeAsyncHasResults()
    {
        var scraper = SetUpAosekaSchedulePageScraper();
        var result = await scraper.ScrapeAsync(default);
        Assert.That(result, Has.Count.EqualTo(14));
    }

    [Test]
    public async Task TestScrapeAsync()
    {
        var scraper = SetUpAosekaSchedulePageScraper();
        var result = await scraper.ScrapeAsync(default);

        // 定期公演『BLUE』vol.5
        var testEvent = result
            .FirstOrDefault(v =>
                v.EventName == "定期公演『BLUE』vol.5"
            );

        Assert.Multiple(() =>
        {
            Assert.That(testEvent?.EventName, Is.EqualTo("定期公演『BLUE』vol.5"));
            Assert.That(testEvent?.EventDescription,
                Is.EqualTo(@"https://twitter.com/_official/status/1699407246718099548?s=46&t=6eY06r32ieFIBkEEOeEbdQ
📍Spotify O-Crest 🕖OPEN 19:00/START 19:30")
            );
            Assert.That(testEvent?.EventType, Is.EqualTo(EventType.Unknown));
            Assert.That(testEvent?.EventPlace, Is.EqualTo("Spotify O-Crest"));
            Assert.That(testEvent?.EventDateTime, Is.TypeOf(typeof(DetailedEventDateTime)));
            Assert.That(
                testEvent?.EventDateTime.EventStartDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2023-09-28 19:00:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(
                testEvent?.EventDateTime.EventEndDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2023-09-28 23:00:00+09:00", CultureInfo.InvariantCulture))
            );
        });
    }
}
