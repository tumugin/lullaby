namespace Lullaby.Tests.Crawler.Scraper.Aoseka;

using System.Globalization;
using AngleSharp;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Aoseka;
using Lullaby.Crawler.Utility;
using RestSharp;
using RichardSzalay.MockHttp;

public class AosekaSchedulePageScraperTest
{
    private AosekaSchedulePageScraper aosekaSchedulePageScraper = null!;

    [SetUp]
    public void SetUp()
    {
        // mock html request
        var testFileContent = ScraperTestUtils.GetTestFileFromManifest(
            "Lullaby.Tests.Crawler.Scraper.Aoseka.aoseka-test-page.html"
        ).Result;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When(AosekaSchedulePageScraper.SchedulePageUrl)
            .Respond("text/html", testFileContent);
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        this.aosekaSchedulePageScraper = new AosekaSchedulePageScraper(
            client,
            BrowsingContext.New(Configuration.Default.WithDefaultLoader()),
            new FullDateGuesser(),
            new EventTypeDetector()
        );
    }

    [Test]
    public async Task TestScrapeAsyncHasResults()
    {
        var result = await this.aosekaSchedulePageScraper.ScrapeAsync(default);
        Assert.That(result, Has.Count.EqualTo(14));
    }

    [Test]
    public async Task TestScrapeAsync()
    {
        var result = await this.aosekaSchedulePageScraper.ScrapeAsync(default);

        // 定期公演『BLUE』vol.5
        var testEvent = result
            .FirstOrDefault(v =>
                v.EventName == "定期公演『BLUE』vol.5"
            );

        Assert.Multiple(() =>
        {
            Assert.That(testEvent?.EventName, Is.EqualTo("定期公演『BLUE』vol.5"));
            Assert.That(testEvent?.EventDescription,
                Is.EqualTo(@"")
            );
            Assert.That(testEvent?.EventType, Is.EqualTo(EventType.Unknown));
            Assert.That(testEvent?.EventPlace, Is.EqualTo("Spotify O-Crest"));
            Assert.That(testEvent?.EventDateTime, Is.TypeOf(typeof(UnDetailedEventDateTime)));
            Assert.That(
                testEvent?.EventDateTime.EventStartDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2022-08-05 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(
                testEvent?.EventDateTime.EventEndDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2022-08-06 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
        });
    }
}
