namespace Lullaby.Tests.Crawler.Scraper.Magmell;

using AngleSharp;
using Common.Crawler.Events;
using Common.Crawler.Scraper.Magmell;
using Common.Crawler.Utility;
using RichardSzalay.MockHttp;

public class MagmellSchedulePageScraperTest
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created")]
    private static MagmellSchedulePageScraper SetUpSchedulePageScraper()
    {
        // mock html request
        var testFileContent = ScraperTestUtils.GetTestFileFromManifest(
            "Lullaby.Tests.Crawler.Scraper.Magmell.magmell-test-page.html"
        ).Result;

        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When("https://lit.link/magmellinfo")
            .Respond("text/html", testFileContent);

        var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var client = mockHttp.ToHttpClient();

        return new MagmellSchedulePageScraper(
            browsingContext,
            client,
            new FullDateGuesser(),
            new FakeTimeProvider(),
            new EventTypeDetector()
        );
    }

    private sealed class FakeTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(2024, 7, 12, 0, 0, 0, TimeSpan.Zero);
    }

    [Test]
    public async Task TestScrapeAsyncHasResults()
    {
        var scraper = SetUpSchedulePageScraper();

        var result = await scraper.ScrapeAsync(default);
        Assert.That(result, Has.Count.EqualTo(21));
    }
}
