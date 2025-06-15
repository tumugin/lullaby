namespace Lullaby.Tests.Crawler.Scraper.Magmell;

using AngleSharp;
using Common.Crawler.Events;
using Common.Crawler.Scraper.Magmell;
using Common.Crawler.Utility;
using Flurl.Util;
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

    [Test]
    public async Task TestScrapeAsyncResultIsCorrect()
    {
        var scraper = SetUpSchedulePageScraper();

        var result = await scraper.ScrapeAsync(default);
        var testEvent = result.First(v => v.EventName == "#ﾆｷﾌﾟﾚ「ミツドモエ。」");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(testEvent.EventName, Is.EqualTo("#ﾆｷﾌﾟﾚ「ミツドモエ。」"));
            Assert.That(testEvent.EventPlace, Is.EqualTo("渋谷近未来会館"));
            Assert.That(testEvent.EventDateTime, Is.InstanceOf<UnDetailedEventDateTime>());
            Assert.That(
                testEvent.EventDateTime.EventStartDateTimeOffset.ToInvariantString(),
                Is.EqualTo("2024-07-31T00:00:00.0000000+09:00")
            );
            Assert.That(
                testEvent.EventDateTime.EventEndDateTimeOffset.ToInvariantString(),
                Is.EqualTo("2024-08-01T00:00:00.0000000+09:00")
            );
            Assert.That(
                testEvent.EventDescription,
                Is.EqualTo(
                    "出演19:40-20:20 / 特典会21:10-22:20【チケット一般販売】〜7/30(火)23:59迄\nhttps://t.pia.jp/pia/ticketInformation.do?eventCd=2423877&rlsCd=001&lotRlsCd="
                )
            );
        }
    }
}
