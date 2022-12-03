namespace Lullaby.Crawler.Events;

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
    public static EventType[] AllTypes() => new[]
    {
        EventType.OneMan, EventType.Battle, EventType.Fes, EventType.BattleOrFes,
    };
}
