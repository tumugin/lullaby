namespace Lullaby.Tests.Crawler.Scraper.Kolokol;

public class KolokolSchedulePageScraperTest : BaseScraperTest
{
    [Test]
    public async Task ScrapeAsyncTest()
    {
        // mock html request
        var testFileContent =
            await this.GetTestFileFromManifest("Lullaby.Tests.Crawler.Scraper.Aoseka.aoseka-test-page.html");
    }
}
