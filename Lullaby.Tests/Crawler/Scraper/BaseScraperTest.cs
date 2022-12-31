namespace Lullaby.Tests.Crawler.Scraper;

public abstract class BaseScraperTest
{
    protected async Task<string> GetTestFileFromManifest(string manifestPath)
    {
        var testFileStream = typeof(BaseScraperTest)
            .Assembly
            .GetManifestResourceStream(manifestPath);
        var testFileContent = await new StreamReader(testFileStream).ReadToEndAsync();
        return testFileContent;
    }
}
