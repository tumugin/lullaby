namespace Lullaby;

using Crawler.Groups;
using Job;
using Quartz;

public static class ConfigureScheduledJobs
{
    public static void Configure(IServiceCollectionQuartzConfigurator quarts)
    {
        // aoseka - 群青の世界
        var aosekaJobKey = new JobKey(AosekaCrawlerJob.JobKey);
        quarts.AddJob<AosekaCrawlerJob>(o => o.WithIdentity(aosekaJobKey));
        quarts.AddTrigger(t => t
            .ForJob(aosekaJobKey)
            .WithIdentity("Aoseka cron trigger")
            .WithCronSchedule(Aoseka.CrawlCronConstant)
        );

        // kolokol - Kolokol
        var kolokolJobKey = new JobKey(KolokolCrawlerJob.JobKey);
        quarts.AddJob<KolokolCrawlerJob>(o => o.WithIdentity(kolokolJobKey));
        quarts.AddTrigger(t => t
            .ForJob(kolokolJobKey)
            .WithIdentity("Kolokol cron trigger")
            .WithCronSchedule(Kolokol.CrawlCronConstant)
        );

        // yosugala - yosugala
        var yosugalaJobKey = new JobKey(YosugalaCrawlerJob.JobKey);
        quarts.AddJob<YosugalaCrawlerJob>(o => o.WithIdentity(yosugalaJobKey));
        quarts.AddTrigger(t => t
            .ForJob(yosugalaJobKey)
            .WithIdentity("yosugala cron trigger")
            .WithCronSchedule(Yosugala.CrawlCronConstant)
        );

        // OSS - OSS
        var ossJobKey = new JobKey(OssCrawlerJob.JobKey);
        quarts.AddJob<OssCrawlerJob>(o => o.WithIdentity(ossJobKey));
        quarts.AddTrigger(t => t
            .ForJob(ossJobKey)
            .WithIdentity("OSS cron trigger")
            .WithCronSchedule(Oss.CrawlCronConstant)
        );
    }
}
