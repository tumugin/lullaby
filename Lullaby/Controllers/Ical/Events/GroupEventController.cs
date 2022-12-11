namespace Lullaby.Controllers.Ical.Events;

using System.Net;
using AngleSharp.Text;
using Crawler.Groups;
using global::Ical.Net;
using global::Ical.Net.CalendarComponents;
using global::Ical.Net.DataTypes;
using global::Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;
using Requests.Ical.Events;
using Services.Event;

[Controller]
[Route("/ical/events/{groupKey}")]
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
            var notFoundContent = this.Content("", "text/calendar");
            notFoundContent.StatusCode = StatusCodes.Status404NotFound;
            return notFoundContent;
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

        var calendarEvents = events
            .Select(e => new CalendarEvent()
            {
                Start = new CalDateTime(e.EventStarts.UtcDateTime, "UTC"),
                End = new CalDateTime(e.EventEnds.UtcDateTime, "UTC"),
                Name = e.EventName,
                Description = e.EventDescription,
                Location = e.EventPlace,
            });

        var calendar = new Calendar();
        calendar.Name = group.GroupName;
        calendarEvents.ToList().ForEach(v => calendar.Events.Add(v));
        calendar.AddTimeZone("UTC");

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return this.Content(serializedCalendar, "text/calendar");
    }
}
