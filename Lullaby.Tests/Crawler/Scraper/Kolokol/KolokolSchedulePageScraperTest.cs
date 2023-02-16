namespace Lullaby.Tests.Crawler.Scraper.Kolokol;

using System.Globalization;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper.Kolokol;
using RestSharp;
using RichardSzalay.MockHttp;

public class KolokolSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        // mock html request
        var testFutureFileContent =
            await GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Kolokol.kolokol-test-page.html");
        var testPastFileContent =
            await GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Kolokol.kolokol-past-schedule-test-page.html");
        var mockHttp = new MockHttpMessageHandler();
        KolokolSchedulePageScraper.SchedulePageUrls.ToList().ForEach(pageUrl =>
        {
            if (pageUrl.Contains("/past/"))
            {
                mockHttp
                    .When(pageUrl)
                    .Respond("text/html", testPastFileContent);
            }
            else
            {
                mockHttp
                    .When(pageUrl)
                    .Respond("text/html", testFutureFileContent);
            }
        });
        var client = new RestClient(new RestClientOptions { ConfigureMessageHandler = _ => mockHttp });

        var scraper = new KolokolSchedulePageScraper { Client = client };
        var result = await scraper.ScrapeAsync();

        Assert.That(result.Count, Is.EqualTo(48));
        var kinoSaki = result.FirstOrDefault(e => e.EventName == "きのさき生誕2023");
        Assert.Multiple(() =>
        {
            Assert.That(kinoSaki?.EventName, Is.EqualTo("きのさき生誕2023"));
            Assert.That(kinoSaki?.EventPlace, Is.EqualTo("梅田BananaHall"));
            Assert.That(kinoSaki?.EventDescription, Is.EqualTo(@"INFO
きのさき生誕祭！！
【チケット】
https://eplus.jp/sf/detail/3773180001-P0030001
一般先着発売
12/6(火) 20:00～
優先チケット5,000円
一般チケット3,000円
VENUE
梅田BananaHall
大阪府大阪市北区堂山町1-21
TIME
開場 / 18:00 開演 / 18:30
TICKET
前売 ¥ 3,000 / 当日 ¥ 3,500
ACT
Kolokol"));
            Assert.That(
                (kinoSaki?.EventDateTime as DetailedEventDateTime)!
                .EventStartDateTime
                .ToString(CultureInfo.InvariantCulture),
                Is.EqualTo("01/22/2023 18:00:00 +09:00")
            );
            Assert.That(
                (kinoSaki.EventDateTime as DetailedEventDateTime)!
                .EventEndDateTime
                .ToString(CultureInfo.InvariantCulture),
                Is.EqualTo("01/22/2023 22:00:00 +09:00")
            );
        });
    }
}
