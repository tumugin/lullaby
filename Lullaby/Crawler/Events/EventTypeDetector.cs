namespace Lullaby.Crawler.Events;

public class EventTypeDetector
{
	private readonly string[] BattleEventNames = { "HYPE", "Funpal", "MAWA LOOP", "SPARK" };

	public EventType detectEventTypeByTitle(string eventTitle) =>
		eventTitle switch
		{
			not null when eventTitle.Contains("対バン") => EventType.BATTLE,
			not null when BattleEventNames.Any(eventTitle.Contains) => EventType.BATTLE,
			_ => EventType.NOT_DEFINED
		};
}
