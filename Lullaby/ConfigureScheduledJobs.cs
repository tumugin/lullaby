namespace Lullaby;

using Crawler.Groups;
using Job;
using Quartz;

public static class ConfigureScheduledJobs
{
    public static void Configure(IServiceCollectionQuartzConfigurator quarts)
    {
        // aoseka - 群青の世界
        var aosekaJobKey = new JobKey("AosekaCrawlerJob");
        quarts.AddJob<AosekaCrawlerJob>(o => o.WithIdentity(aosekaJobKey));
        quarts.AddTrigger(t => t
            .ForJob(aosekaJobKey)
            .WithIdentity("Aoseka cron trigger")
            .WithCronSchedule(new Aoseka().CrawlCron)
        );
    }
}
