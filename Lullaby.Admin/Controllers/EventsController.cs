namespace Lullaby.Admin.Controllers;

using Services;
using Microsoft.AspNetCore.Mvc;
using Requests.Events;
using ViewModels;

public class EventsController(IEventSearchService eventSearchService) : Controller
{
    public async Task<IActionResult> Index(
        IndexRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await eventSearchService.SearchEventAsync(
            request.GroupKey,
            request.EventName,
            request.StartDateTime,
            request.EndDateTime,
            request.Page,
            cancellationToken
        );

        return this.View(new EventsListViewModel { Request = request, Result = result });
    }
}
