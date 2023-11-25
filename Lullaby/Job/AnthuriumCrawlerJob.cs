namespace Lullaby.Job;

using Crawler;
using Groups;

public class AnthuriumCrawlerJob : BaseCrawlerJob
{
    public static string JobKey => "AnthuriumCrawlerJob";
    private readonly Anthurium anthurium;

    public AnthuriumCrawlerJob(IGroupCrawler groupCrawler, Anthurium anthurium) : base(groupCrawler) =>
        this.anthurium = anthurium;

    protected override IGroup TargetGroup => this.anthurium;
}
