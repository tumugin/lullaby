namespace Lullaby.ViewModels.Toybox;

using Job;

public class ToyboxJobViewModel
{
    public string[] AvaliableJobs =>
        new[] { AosekaCrawlerJob.JobKey, KolokolCrawlerJob.JobKey, YosugalaCrawlerJob.JobKey, OSSCrawlerJob.JobKey };
}
