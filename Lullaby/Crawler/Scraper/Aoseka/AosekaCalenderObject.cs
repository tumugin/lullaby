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
            .Parse($"{this.Start} 00:00:00+09:00", CultureInfo.InvariantCulture),
        EventEndDate = DateTimeOffset
            .Parse($"{this.End} 00:00:00+09:00", CultureInfo.InvariantCulture)
    };

    [JsonPropertyName("end")] public required string End { get; init; }

    [JsonPropertyName("title")] public required string Title { get; init; }

    [JsonPropertyName("color")] public required string Color { get; init; }

    [JsonIgnore]
    public EventType EventType =>
        this.Color switch
        {
            // 色で分けるしかないっぽい。どうしてこうなった？？
            // 関東主催ライブ/イベント
            "#6475C5" => EventType.OneMan,
            // 地方主催ライブ/イベント
            "#23A455" => EventType.OneMan,
            // 関東ライブ/イベント
            "#6EC1E4" => EventType.BattleOrFes,
            // 地方ライブ/イベント
            "#99E7A4" => EventType.BattleOrFes,
            _ => EventType.Unknown
        };
}
