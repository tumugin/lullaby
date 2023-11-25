namespace Lullaby.Crawler.Scraper.Anthurium.ApiClient;

using System.Text.Json.Serialization;
using Utils;

public class AnthuriumScheduleItem
{
    public required string ScheduleEventId { get; init; }
    public required string EventName { get; init; }
    public required bool IsAllDay { get; init; }
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset ReleaseDate { get; init; }
    public required string ScheduleCategoryId { get; init; }
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset StartDate { get; init; }
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset EndDate { get; init; }
    public required bool IsRepeat { get; init; }
    public required string EventDetail { get; init; }
}
