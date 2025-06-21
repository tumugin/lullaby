namespace Lullaby.Controllers.Ical.Events;

using Common.Groups;
using global::Ical.Net;
using global::Ical.Net.CalendarComponents;
using global::Ical.Net.DataTypes;
using global::Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;
using Requests.Ical.Events;
using Services.Events;
using Swashbuckle.AspNetCore.Annotations;

[Controller]
[Produces("text/calendar")]
[Route("/ical/events/{groupKey}")]
public class GroupEventController : ControllerBase
{
    public GroupEventController(IGetEventsByGroupKeyService getEventsByGroupKeyService) =>
        this.GetEventsByGroupKeyService = getEventsByGroupKeyService;

    private IGetEventsByGroupKeyService GetEventsByGroupKeyService { get; }

    /// <summary>
    ///     Get events of the specified group in iCal format
    /// </summary>
    /// <param name="groupKey">The key of group(ex. aoseka)</param>
    /// <param name="groupEventIndexParameters">Options to get events</param>
    /// <param name="groupKeys">Instance of <see cref="IGroupKeys" /></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "The operation was succeeded")]
    [SwaggerResponse(404, "The group was not found")]
    [SwaggerResponse(400, "The request was invalid")]
    public async Task<IActionResult> Get(
        [FromRoute] string groupKey,
        [FromQuery] GroupEventIndexParameters groupEventIndexParameters,
        [FromServices] IGroupKeys groupKeys,
        CancellationToken cancellationToken
    )
    {
        var group = groupKeys.GetGroupByKey(groupKey);

        if (group == null)
        {
            var notFoundContent = this.Content("", "text/calendar");
            notFoundContent.StatusCode = StatusCodes.Status404NotFound;
            return notFoundContent;
        }

        var events = groupEventIndexParameters switch
        {
            { EventEndsAt: not null, EventStartsFrom: not null } =>
                await this.GetEventsByGroupKeyService.Execute(
                    groupKey,
                    groupEventIndexParameters.EventTypes,
                    groupEventIndexParameters.EventStartsFrom.Value,
                    groupEventIndexParameters.EventEndsAt.Value,
                    cancellationToken
                ),
            _ => await this.GetEventsByGroupKeyService.Execute(
                groupKey,
                groupEventIndexParameters.EventTypes,
                cancellationToken
            )
        };

        var asiaTokyoTimezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        var calendarEvents = events
            .Select(e => new CalendarEvent
            {
                Start = new CalDateTime(
                    TimeZoneInfo.ConvertTime(e.EventStarts, asiaTokyoTimezone).DateTime,
                    "Asia/Tokyo",
                    hasTime: e.IsDateTimeDetailed
                ),
                End = new CalDateTime(
                    TimeZoneInfo.ConvertTime(
                        e.EventEnds,
                        asiaTokyoTimezone
                    ).DateTime,
                    "Asia/Tokyo",
                    hasTime: e.IsDateTimeDetailed
                ),
                Summary = e.EventName,
                Description = e.EventDescription,
                Location = e.EventPlace
            })
            .ToArray();

        var calendar = new Calendar();

        // カレンダーのタイムゾーンはAsia/Tokyoであることを明示的に指定する
        // NOTE: iCalはAllDayなEventにおいて正しくタイムゾーンを扱えないため、UTC時間における日付としてカレンダーに追加されてしまう
        // FIXME: タイムゾーンがAsia/Tokyo以外のグループを扱うことになったら対応を考える
        calendar.AddTimeZone("Asia/Tokyo");

        foreach (var calendarEvent in calendarEvents)
        {
            calendar.Events.Add(calendarEvent);
        }

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return this.Content(serializedCalendar ?? string.Empty, "text/calendar");
    }
}
