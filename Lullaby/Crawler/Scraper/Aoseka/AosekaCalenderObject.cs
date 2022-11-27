namespace Lullaby.Crawler.Scraper.Aoseka;

using System.Globalization;
using System.Text.Json.Serialization;
using Events;

public class AosekaCalenderObject
{
    [JsonPropertyName("description")] public required string Description { get; init; }

    [JsonPropertyName("start")] public required string Start { get; init; }

    [JsonIgnore]
    public UnDetailedEventDateTime ConvertedEventDateTime => new UnDetailedEventDateTime
    {
        // YYYY-MM-DDの形式で入っているので、末尾に時刻とJSTのタイムゾーンを付ける
        EventStartDate = DateTimeOffset
            .Parse($"{Start} 00:00:00+09:00", CultureInfo.InvariantCulture),
        EventEndDate = DateTimeOffset
            .Parse($"{End} 00:00:00+09:00", CultureInfo.InvariantCulture)
    };

    [JsonPropertyName("end")] public required string End { get; init; }

    [JsonPropertyName("title")] public required string Title { get; init; }

    [JsonPropertyName("color")] public required string Color { get; init; }

    [JsonIgnore]
    public EventType EventType => Color switch
    {
        "#6475C5" => EventType.ONE_MAN,
        "#23A455" => EventType.ONE_MAN,
        "#6EC1E4" => EventType.BATTLE_OR_FES,
        "#99E7A4" => EventType.BATTLE_OR_FES,
        _ => EventType.UNKNOWN
    };
}
