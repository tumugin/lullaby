namespace Lullaby.Job;

using Hangfire;

public static class ConfigureScheduledJobs
{
    public static void Configure(IRecurringJobManager recurringJobManager)
    {
        // aoseka - 群青の世界
        recurringJobManager.AddOrUpdate<AosekaCrawlerJob>(
            AosekaCrawlerJob.JobKey,
            // disable for now because group has been suspended
            x => ExecuteForSuspendingGroup(),
            Cron.Hourly()
        );

        // kolokol - Kolokol
        recurringJobManager.AddOrUpdate<KolokolCrawlerJob>(
            KolokolCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // yosugala - yosugala
        recurringJobManager.AddOrUpdate<YosugalaCrawlerJob>(
            YosugalaCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // OSS - OSS
        recurringJobManager.AddOrUpdate<OssCrawlerJob>(
            OssCrawlerJob.JobKey,
            // disable for now because group has been suspended
            x => ExecuteForSuspendingGroup(),
            Cron.Hourly()
        );

        // 手羽先センセーション - tebasen
        recurringJobManager.AddOrUpdate<TebasenCrawlerJob>(
            TebasenCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // Axelight - axelight
        recurringJobManager.AddOrUpdate<AxelightCrawlerJob>(
            AxelightCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // PRSMIN - prsmin
        recurringJobManager.AddOrUpdate<PrsminCrawlerJob>(
            PrsminCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // 天使にはなれない - tenhana
        recurringJobManager.AddOrUpdate<TenhanaCrawlerJob>(
            TenhanaCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // TENRIN - tenrin
        recurringJobManager.AddOrUpdate<TenrinCrawlerJob>(
            TenrinCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // アンスリューム - anthurium
        recurringJobManager.AddOrUpdate<AnthuriumCrawlerJob>(
            AnthuriumCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );

        // NARLOW - narlow
        recurringJobManager.AddOrUpdate<NarlowCrawlerJob>(
            NarlowCrawlerJob.JobKey,
            x => x.Execute(default),
            Cron.Hourly()
        );
    }

    private static Task ExecuteForSuspendingGroup() => Task.CompletedTask;
}
