namespace Lullaby.Common.Crawler.Events;

public class UnDetailedEventDateTime : IEventDateTime
{
    public required DateTimeOffset EventStartDate { get; init; }

    public required DateTimeOffset EventEndDate { get; init; }
}
