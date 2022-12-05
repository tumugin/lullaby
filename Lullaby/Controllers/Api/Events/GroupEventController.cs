namespace Lullaby.Controllers.Api.Events;

using Crawler.Groups;
using Microsoft.AspNetCore.Mvc;
using Requests.Api.Events;
using Responses;
using Responses.Api.Events;
using Services.Event;
using ViewModels;

[ApiController]
[Route("/api/events/{groupKey}")]
public class GroupEventController : ControllerBase
{
    private GetEventsByGroupKeyService GetEventsByGroupKeyService { get; }

    public GroupEventController(GetEventsByGroupKeyService getEventsByGroupKeyService) =>
        this.GetEventsByGroupKeyService = getEventsByGroupKeyService;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromRoute] string groupKey,
        [FromQuery] GroupEventIndexParameters groupEventIndexParameters
    )
    {
        if (!GroupKeys.AvailableGroupKeys.Contains(groupKey))
        {
            return this.NotFound(new BaseErrorResponse { ErrorType = ApiErrorTypes.GroupNotFound });
        }

        var group = GroupKeys.GetGroupByKey(groupKey);

        var events = groupEventIndexParameters switch
        {
            { EventEndsAt: { }, EventStartsFrom: { } } =>
                await this.GetEventsByGroupKeyService.Execute(
                    groupKey,
                    groupEventIndexParameters.EventTypes,
                    groupEventIndexParameters.EventStartsFrom.Value,
                    groupEventIndexParameters.EventEndsAt.Value
                ),
            _ => await this.GetEventsByGroupKeyService.Execute(
                groupKey,
                groupEventIndexParameters.EventTypes
            )
        };

        return this.Ok(
            new GroupEventsGetResponse { Group = GroupViewModel.FromGroup(group), Events = events }
        );
    }
}
