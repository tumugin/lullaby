namespace Lullaby.Crawler.Events;

public class Event
{
	public required string EventName { get; init; }

	public required string EventPlace { get; init; }

	public required IEventDateTime EventDateTime { get; init; }

	public required EventType EventType { get; init; }

	public required string EventDescription { get; init; }
}
