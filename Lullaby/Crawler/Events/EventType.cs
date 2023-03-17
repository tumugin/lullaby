namespace Lullaby.Crawler.Events;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventType
{
    /**
	 * 不明
	 */
    Unknown,

    /**
	 * 主催ワンマン
	 */
    OneMan,

    /**
	 * 対バン
	 */
    Battle,

    /**
	 * フェス
	 */
    Fes,

    /**
     * 対バンもしくはフェス
     */
    BattleOrFes,
}

public static class EventTypeExt
{
    public static IReadOnlyCollection<EventType> AllTypes() => new[]
    {
        EventType.Unknown, EventType.OneMan, EventType.Battle, EventType.Fes, EventType.BattleOrFes,
    };
}
