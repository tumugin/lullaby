namespace Lullaby.Tests.Crawler.Scraper.Aoseka;

using System.Globalization;
using AngleSharp;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Aoseka;
using RestSharp;
using RichardSzalay.MockHttp;

public class AosekaSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task TestScrapeAsync()
    {
        // mock html request
        var testFileContent =
            await GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Aoseka.aoseka-test-page.html");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When(AosekaSchedulePageScraper.SchedulePageUrl)
            .Respond("text/html", testFileContent);
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        var scraper = new AosekaSchedulePageScraper(client, BrowsingContext.New(Configuration.Default.WithDefaultLoader()));

        var result = await scraper.ScrapeAsync(default);

        Assert.That(result, Has.Count.EqualTo(64));

        // TOKYO IDOL FESTIVAL 2022 supported byにしたんクリニック
        var tif = result
            .FirstOrDefault(v =>
                v.EventName == "TOKYO IDOL FESTIVAL 2022 supported byにしたんクリニック"
            );
        Assert.Multiple(() =>
        {
            Assert.That(tif?.EventName, Is.EqualTo("TOKYO IDOL FESTIVAL 2022 supported byにしたんクリニック"));
            Assert.That(tif?.EventDescription,
                Is.EqualTo(@"「TOKYO IDOL FESTIVAL 2022 supported byにしたんクリニック」
□お台場・青海周辺エリア
チケット▶︎https://ticket.rakuten.co.jp/features/tip/

日割りは後日解禁！
詳細▶︎https://official.idolfes.com/s/tif2022/?ima=0914")
            );
            Assert.That(tif?.EventType, Is.EqualTo(EventType.BattleOrFes));
            Assert.That(tif?.EventPlace, Is.Null);
            Assert.That(tif?.EventDateTime, Is.TypeOf(typeof(UnDetailedEventDateTime)));
            Assert.That(
                (tif?.EventDateTime as UnDetailedEventDateTime)?.EventStartDate,
                Is.EqualTo(DateTimeOffset.Parse("2022-08-05 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
            Assert.That(
                (tif?.EventDateTime as UnDetailedEventDateTime)?.EventEndDate,
                Is.EqualTo(DateTimeOffset.Parse("2022-08-06 00:00:00+09:00", CultureInfo.InvariantCulture))
            );
        });
    }
}
