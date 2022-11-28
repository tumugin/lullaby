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
