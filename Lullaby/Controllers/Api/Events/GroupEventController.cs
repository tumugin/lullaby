namespace Lullaby.Controllers.Api.Events;

using Crawler.Groups;
using Microsoft.AspNetCore.Mvc;
using Requests.Api.Events;
using Responses;
using Responses.Api.Events;
using Services.Event;

[ApiController]
[Route("/api/events/{groupKey:string}")]
public class GroupEventController : ControllerBase
{
    private GetEventsByGroupKeyService GetEventsByGroupKeyService { get; }

    public GroupEventController(GetEventsByGroupKeyService getEventsByGroupKeyService) =>
        this.GetEventsByGroupKeyService = getEventsByGroupKeyService;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromRoute] string groupKey,
        GroupEventIndexParameters groupEventIndexParameters)
    {
        if (!GroupKeys.AvailableGroupKeys.Contains(groupKey))
        {
            return this.NotFound(new BaseErrorResponse { ErrorType = ApiErrorTypes.GroupNotFound });
        }

        var group = GroupKeys.GetGroupByKey(groupKey);

        var events = groupEventIndexParameters switch
        {
            { } g when g.EventEndsAt.HasValue && g.EventStartsFrom.HasValue =>
                await this.GetEventsByGroupKeyService.Execute(
                    groupKey,
                    groupEventIndexParameters.EventTypes,
                    g.EventStartsFrom.Value,
                    g.EventEndsAt.Value
                ),
            _ => await this.GetEventsByGroupKeyService.Execute(
                groupKey,
                groupEventIndexParameters.EventTypes
            )
        };

        return this.Ok(new GroupEventsGetResponse { Group = group, Events = events });
    }
}
