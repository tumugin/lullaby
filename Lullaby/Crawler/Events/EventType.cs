namespace Lullaby.Crawler.Events;

public enum EventType
{
    /**
	 * 不明
	 */
    UNKNOWN,

    /**
	 * 主催ワンマン
	 */
    ONE_MAN,

    /**
	 * 対バン
	 */
    BATTLE,

    /**
	 * フェス
	 */
    FES,

    /**
     * 対バンもしくはフェス
     */
    BATTLE_OR_FES,
}
