namespace Lullaby.Crawler.Scraper.Anthurium.ApiClient;

using System.Text.Json.Serialization;
using Utils;

public class AnthuriumScheduleItem
{
    [JsonPropertyName("scheduleEventId")] public required string ScheduleEventId { get; init; }

    [JsonPropertyName("eventName")] public required string EventName { get; init; }

    [JsonPropertyName("isAllDay")] public required bool IsAllDay { get; init; }

    [JsonPropertyName("releaseDate")]
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset ReleaseDate { get; init; }

    [JsonPropertyName("scheduleCategoryId")]
    public required string ScheduleCategoryId { get; init; }

    [JsonPropertyName("startDate")]
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset StartDate { get; init; }

    [JsonPropertyName("endDate")]
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset EndDate { get; init; }

    [JsonPropertyName("isRepeat")] public required bool IsRepeat { get; init; }

    [JsonPropertyName("eventDetail")] public string? EventDetail { get; init; }
}
