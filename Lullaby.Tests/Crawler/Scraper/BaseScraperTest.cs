namespace Lullaby.Tests.Crawler.Scraper;

public abstract class BaseScraperTest
{
    protected static async Task<string> GetTestFileFromManifest(string manifestPath)
    {
        var testFileStream = typeof(BaseScraperTest)
            .Assembly
            .GetManifestResourceStream(manifestPath);

        if (testFileStream == null)
        {
            throw new InvalidOperationException($"{manifestPath} not found.");
        }

        var testFileContent = await new StreamReader(testFileStream).ReadToEndAsync();
        return testFileContent;
    }
}
