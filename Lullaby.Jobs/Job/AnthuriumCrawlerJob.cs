namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class AnthuriumCrawlerJob : BaseCrawlerJob
{
    public static string JobKey => "AnthuriumCrawlerJob";
    private readonly Anthurium anthurium;

    public AnthuriumCrawlerJob(IGroupCrawlerService groupCrawlerService, Anthurium anthurium) : base(groupCrawlerService) =>
        this.anthurium = anthurium;

    protected override IGroup TargetGroup => this.anthurium;
}
