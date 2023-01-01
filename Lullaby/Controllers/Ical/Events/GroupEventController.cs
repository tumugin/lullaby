namespace Lullaby.Controllers.Ical.Events;

using Crawler.Groups;
using global::Ical.Net;
using global::Ical.Net.CalendarComponents;
using global::Ical.Net.DataTypes;
using global::Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;
using Requests.Ical.Events;
using Services.Event;
using Swashbuckle.AspNetCore.Annotations;

[Controller]
[Produces("text/calendar")]
[Route("/ical/events/{groupKey}")]
public class GroupEventController : ControllerBase
{
    private GetEventsByGroupKeyService GetEventsByGroupKeyService { get; }

    public GroupEventController(GetEventsByGroupKeyService getEventsByGroupKeyService) =>
        this.GetEventsByGroupKeyService = getEventsByGroupKeyService;

    [HttpGet]
    [SwaggerResponse(200, "The operation was succeeded")]
    [SwaggerResponse(404, "The group was not found")]
    [SwaggerResponse(400, "The request was invalid")]
    public async Task<IActionResult> Get(
        [FromRoute] string groupKey,
        [FromQuery] GroupEventIndexParameters groupEventIndexParameters
    )
    {
        var group = GroupKeys.GetGroupByKey(groupKey);

        if (group == null)
        {
            var notFoundContent = this.Content("", "text/calendar");
            notFoundContent.StatusCode = StatusCodes.Status404NotFound;
            return notFoundContent;
        }

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

        var calendarEvents = events
            .Select(e => new CalendarEvent()
            {
                Start = new CalDateTime(e.EventStarts.UtcDateTime, "UTC"),
                End = e.IsDateTimeDetailed ? new CalDateTime(e.EventEnds.UtcDateTime, "UTC") : null,
                IsAllDay = !e.IsDateTimeDetailed,
                Summary = e.EventName,
                Description = e.EventDescription,
                Location = e.EventPlace,
            });

        var calendar = new Calendar();

        // カレンダーのタイムゾーンはUTCであることを明示的に指定する
        calendar.AddTimeZone("UTC");

        calendarEvents.ToList().ForEach(v => calendar.Events.Add(v));

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return this.Content(serializedCalendar, "text/calendar");
    }
}
