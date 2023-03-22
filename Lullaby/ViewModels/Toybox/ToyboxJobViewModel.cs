namespace Lullaby.ViewModels.Toybox;

using Job;

public class ToyboxJobViewModel
{
    public IReadOnlyList<string> AvaliableJobs =>
        new[] { AosekaCrawlerJob.JobKey, KolokolCrawlerJob.JobKey, YosugalaCrawlerJob.JobKey, OssCrawlerJob.JobKey };
}
