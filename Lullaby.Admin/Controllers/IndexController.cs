namespace Lullaby.Admin.Controllers;

using Lullaby.Admin.ViewModels;
using Lullaby.Common.Groups;
using Microsoft.AspNetCore.Mvc;

public class IndexController(
    IGroupStatisticsService groupStatisticsService,
    IEnumerable<IGroup> groups
) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var groupStatistics = await groupStatisticsService.GetGroupStatisticsAsync(
            groups,
            cancellationToken
        );

        return this.View(new IndexViewModel { GroupStatistics = groupStatistics });
    }
}
