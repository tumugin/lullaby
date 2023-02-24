namespace Lullaby.Controllers.Api.Events;

using Crawler.Groups;
using Microsoft.AspNetCore.Mvc;
using Requests.Api.Events;
using Responses;
using Responses.Api.Events;
using Services.Events;
using Swashbuckle.AspNetCore.Annotations;
using ViewModels;

[ApiController]
[Route("/api/events/{groupKey}")]
[Produces("application/json")]
public class GroupEventController : ControllerBase
{
    private IGetEventsByGroupKeyService GetEventsByGroupKeyService { get; }

    public GroupEventController(IGetEventsByGroupKeyService getEventsByGroupKeyService) =>
        this.GetEventsByGroupKeyService = getEventsByGroupKeyService;

    /// <summary>
    /// Get events of the specified group
    /// </summary>
    /// <param name="groupKey">The key of group(ex. aoseka)</param>
    /// <param name="groupEventIndexParameters">Options to get events</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "The operation was succeeded", typeof(GroupEventsGetResponse))]
    [SwaggerResponse(404, "The group was not found", typeof(ProblemDetails))]
    [SwaggerResponse(400, "The request was invalid", typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Get(
        [FromRoute] string groupKey,
        [FromQuery] GroupEventIndexParameters groupEventIndexParameters,
        CancellationToken cancellationToken
    )
    {
        var group = GroupKeys.GetGroupByKey(groupKey);

        if (group == null)
        {
            return this.NotFound();
        }

        var events = groupEventIndexParameters switch
        {
            { EventEndsAt: { }, EventStartsFrom: { } } =>
                await this.GetEventsByGroupKeyService.Execute(
                    groupKey,
                    groupEventIndexParameters.EventTypes,
                    groupEventIndexParameters.EventStartsFrom.Value,
                    groupEventIndexParameters.EventEndsAt.Value, cancellationToken),
            _ => await this.GetEventsByGroupKeyService.Execute(
                groupKey,
                groupEventIndexParameters.EventTypes, cancellationToken)
        };

        return this.Ok(
            new GroupEventsGetResponse
            {
                Group = GroupViewModel.FromGroup(group), Events = events.Select(EventViewModel.FromEvent)
            }
        );
    }
}
