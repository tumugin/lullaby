namespace Lullaby.Admin.Controllers;

using Common.Groups;
using Microsoft.AspNetCore.Authorization;
using Services;
using Microsoft.AspNetCore.Mvc;
using Requests.Events;
using ViewModels;

public class EventsController(
    IEventSearchService eventSearchService,
    IEnumerable<IGroup> groups,
    IUserInterfaceDateTimeOffsetService userInterfaceDateTimeOffsetService
) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index(
        IndexRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await eventSearchService.SearchEventAsync(
            request.GroupKey,
            request.EventName,
            request.StartDateTime != null
                ? userInterfaceDateTimeOffsetService.ConvertFormInputDateTimeToUtcDateTimeOffset(request.StartDateTime)
                : null,
            request.EndDateTime != null
                ? userInterfaceDateTimeOffsetService.ConvertFormInputDateTimeToUtcDateTimeOffset(request.EndDateTime)
                : null,
            request.Page,
            cancellationToken
        );

        return this.View(new EventsListViewModel { Request = request, Result = result, Groups = groups });
    }
}
