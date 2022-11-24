namespace lullaby.Crawler.Events;

public class DetailedEventDateTime : IEventDateTime
{
	public required DateTime EventStartDateTime { get; init; }

	public required DateTime EventEndDateTime { get; init; }
}
