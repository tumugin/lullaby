namespace Lullaby.Crawler.Events;

public class EventTypeDetector
{
	private readonly string[] _battleEventNames =
	{
		"HYPE", "Funpal", "MAWA LOOP", "アイドルアラモード", "MARQUEE祭", "dot yell", "TOKYO GIRLS GIRLS"
	};

	private readonly string[] _fesEventNames =
	{
		"LEADING PREMIUM",
		"TIF",
		"TOKYO IDOL FESTIVAL",
		"NATSUZOME",
		"エンドレスサマー",
		"@JAM",
		"SEKIGAHARA IDOL WARS",
		"関ケ原唄姫合戦",
		"SPARK",
		"IDORISE"
	};

	public EventType DetectEventTypeByTitle(string eventTitle) =>
		eventTitle switch
		{
			not null when _battleEventNames.Any(eventTitle.Contains) => EventType.BATTLE,
			not null when _fesEventNames.Any(eventTitle.Contains) => EventType.FES,
			not null when eventTitle.Contains("対バン") => EventType.BATTLE,
			_ => EventType.NOT_DEFINED
		};
}
