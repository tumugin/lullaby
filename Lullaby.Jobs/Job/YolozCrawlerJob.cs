namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class YolozCrawlerJob(IGroupCrawlerService groupCrawlerService, Yoloz yoloz) : BaseCrawlerJob(groupCrawlerService)
{
    public static readonly string JobKey = "YolozCrawlerJob";
    protected override IGroup TargetGroup => yoloz;
}
