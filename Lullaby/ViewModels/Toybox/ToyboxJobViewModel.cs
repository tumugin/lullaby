namespace Lullaby.ViewModels.Toybox;

using Job;

public class ToyboxJobViewModel
{
    public readonly string[] AvaliableJobs = { AosekaCrawlerJob.JobKey, KolokolCrawlerJob.JobKey };
}
