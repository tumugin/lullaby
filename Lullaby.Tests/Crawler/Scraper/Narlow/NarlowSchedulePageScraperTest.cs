namespace Lullaby.Tests.Crawler.Scraper.Narlow;

using System.Globalization;
using AngleSharp;
using Database.Enums;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Narlow;
using RichardSzalay.MockHttp;

public class NarlowSchedulePageScraperTest
{
    private NarlowSchedulePageScraper NarlowSchedulePageScraper = null!;

    [SetUp]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created")]
    public void SetUp()
    {
        // setup mock
        var monthPageFileContent = ScraperTestUtils.GetTestFileFromManifest(
            "Lullaby.Tests.Crawler.Scraper.Narlow.narlow-month-test-page.html"
        ).Result;
        var detailPageFileContent = ScraperTestUtils.GetTestFileFromManifest(
            "Lullaby.Tests.Crawler.Scraper.Narlow.narlow-schedule-detail-test-page.html"
        ).Result;

        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When("https://narlow.net/SCHEDULE")
            .Respond("text/html", monthPageFileContent);
        mockHttp
            .When("https://narlow.net/SCHEDULE/*")
            .Respond("text/html", detailPageFileContent);

        var client = mockHttp.ToHttpClient();
        var browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        this.NarlowSchedulePageScraper = new NarlowSchedulePageScraper(
            new EventTypeDetector(),
            browsingContext,
            client
        );
    }

    [Test]
    public async Task TestScrapeAsyncHasResults()
    {
        var result = await this.NarlowSchedulePageScraper.ScrapeAsync(default);
        Assert.That(result, Has.Count.EqualTo(15));
    }

    [Test]
    public async Task TestScrapeAsync()
    {
        var result = await this.NarlowSchedulePageScraper.ScrapeAsync(default);

        // アイドル甲子園 in 新宿BLAZE
        var testEvent = result
            .FirstOrDefault(v =>
                v.EventName == "12/17（日）【対バン】「アイドル甲子園 in 新宿BLAZE」@新宿BLAZE"
            );

        Assert.Multiple(() =>
        {
            Assert.That(testEvent?.EventName, Is.EqualTo("12/17（日）【対バン】「アイドル甲子園 in 新宿BLAZE」@新宿BLAZE"));
            Assert.That(testEvent?.EventPlace, Is.Null);
            Assert.That(
                testEvent?.EventDateTime.EventStartDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2023-12-17 09:35:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(
                testEvent?.EventDateTime.EventEndDateTimeOffset,
                Is.EqualTo(DateTimeOffset.Parse("2023-12-17 13:35:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(testEvent?.EventDescription,
                Is.EqualTo(
                    "12/17（日）\n「アイドル甲子園 in 新宿BLAZE」\n@ 新宿BLAZE\nOPEN 9:35 \u00a0START 9:50\n\ud83c\udfab 10/13（金）20:00\uff5e 販売開始\nhttps://t.livepocket.jp/e/idolkoushien-1217-\n\u25c9チケット価格 （※各+1D）\n・前方エリアチケット \u00a57,000\n・一般チケット \u00a53,500\n・当日券 \u00a54,500"));
            Assert.That(testEvent?.EventType, Is.EqualTo(EventType.Battle));
        });
    }
}
