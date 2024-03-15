namespace Lullaby.Admin.Controllers;

using ViewModels;
using Common.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

public class IndexController(
    IGroupStatisticsService groupStatisticsService,
    IEnumerable<IGroup> groups
) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var groupStatistics = await groupStatisticsService.GetGroupStatisticsAsync(
            groups,
            cancellationToken
        );

        return this.View(new IndexViewModel { GroupStatistics = groupStatistics });
    }
}
