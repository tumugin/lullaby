namespace Lullaby.Common.Crawler.Scraper.Anthurium.ApiClient;

using System.Text.Json.Serialization;
using Utils;

public class AnthuriumScheduleItem
{
    [JsonPropertyName("eventName")] public required string EventName { get; init; }

    [JsonPropertyName("isAllDay")] public required bool IsAllDay { get; init; }

    [JsonPropertyName("startDate")]
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset StartDate { get; init; }

    [JsonPropertyName("endDate")]
    [JsonConverter(typeof(UnixMillisecondsDateTimeOffsetConverter))]
    public required DateTimeOffset EndDate { get; init; }

    [JsonPropertyName("eventDetail")] public string? EventDetail { get; init; }
}
