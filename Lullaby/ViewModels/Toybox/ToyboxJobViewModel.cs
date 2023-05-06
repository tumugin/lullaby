namespace Lullaby.ViewModels.Toybox;

using Job;

public class ToyboxJobViewModel
{
    public IReadOnlyList<string> AvailableJobs =>
        new[]
        {
            AosekaCrawlerJob.JobKey, KolokolCrawlerJob.JobKey, YosugalaCrawlerJob.JobKey, OssCrawlerJob.JobKey,
            TebasenCrawlerJob.JobKey, AxelightCrawlerJob.JobKey
        };
}
