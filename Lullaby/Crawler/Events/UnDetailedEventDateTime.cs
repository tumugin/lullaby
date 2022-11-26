namespace Lullaby.Crawler.Events;

public class UnDetailedEventDateTime : IEventDateTime
{
    public required DateTime EventStartDate { get; init; }

    public required DateTime EventEndDate { get; init; }
}
