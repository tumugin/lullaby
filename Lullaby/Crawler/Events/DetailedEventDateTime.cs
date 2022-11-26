namespace Lullaby.Crawler.Events;

public class DetailedEventDateTime : IEventDateTime
{
    public required DateTimeOffset EventStartDateTime { get; init; }

    public required DateTimeOffset EventEndDateTime { get; init; }
}
