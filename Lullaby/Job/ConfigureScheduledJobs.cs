namespace Lullaby.Job;

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
            .WithCronSchedule("0 0 * ? * * *")
        );

        // kolokol - Kolokol
        var kolokolJobKey = new JobKey(KolokolCrawlerJob.JobKey);
        quarts.AddJob<KolokolCrawlerJob>(o => o.WithIdentity(kolokolJobKey));
        quarts.AddTrigger(t => t
            .ForJob(kolokolJobKey)
            .WithIdentity("Kolokol cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );

        // yosugala - yosugala
        var yosugalaJobKey = new JobKey(YosugalaCrawlerJob.JobKey);
        quarts.AddJob<YosugalaCrawlerJob>(o => o.WithIdentity(yosugalaJobKey));
        quarts.AddTrigger(t => t
            .ForJob(yosugalaJobKey)
            .WithIdentity("yosugala cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );

        // OSS - OSS
        var ossJobKey = new JobKey(OssCrawlerJob.JobKey);
        quarts.AddJob<OssCrawlerJob>(o => o.WithIdentity(ossJobKey));
        quarts.AddTrigger(t => t
            .ForJob(ossJobKey)
            .WithIdentity("OSS cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );

        // 手羽先センセーション - tebasen
        var tebasenJobKey = new JobKey(TebasenCrawlerJob.JobKey);
        quarts.AddJob<TebasenCrawlerJob>(o => o.WithIdentity(tebasenJobKey));
        quarts.AddTrigger(t => t
            .ForJob(tebasenJobKey)
            .WithIdentity("tebasen cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );

        // Axelight - axelight
        var axelightJobKey = new JobKey(AxelightCrawlerJob.JobKey);
        quarts.AddJob<AxelightCrawlerJob>(o => o.WithIdentity(axelightJobKey));
        quarts.AddTrigger(t => t
            .ForJob(axelightJobKey)
            .WithIdentity("axelight cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );

        // PRSMIN - prsmin
        var prsminJobKey = new JobKey(PrsminCrawlerJob.JobKey);
        quarts.AddJob<PrsminCrawlerJob>(o => o.WithIdentity(prsminJobKey));
        quarts.AddTrigger(t => t
            .ForJob(prsminJobKey)
            .WithIdentity("prsmin cron trigger")
            .WithCronSchedule("0 0 * ? * * *")
        );
    }
}
