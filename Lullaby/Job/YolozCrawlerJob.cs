namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class YolozCrawlerJob(IGroupCrawler groupCrawler, Yoloz yoloz) : BaseCrawlerJob(groupCrawler)
{
    public static readonly string JobKey = "YolozCrawlerJob";
    protected override IGroup TargetGroup => yoloz;
}
