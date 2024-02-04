namespace Lullaby.Tests.Crawler.Scraper;

public abstract class ScraperTestUtils
{
    public static async Task<string> GetTestFileFromManifest(string manifestPath)
    {
        var testFileStream = typeof(ScraperTestUtils)
            .Assembly
            .GetManifestResourceStream(manifestPath);

        if (testFileStream == null)
        {
            throw new InvalidOperationException($"{manifestPath} not found.");
        }

        using var streamReader = new StreamReader(testFileStream);
        var testFileContent = await streamReader.ReadToEndAsync();
        return testFileContent;
    }
}
