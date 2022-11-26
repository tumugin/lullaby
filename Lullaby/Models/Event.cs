namespace Lullaby.Models;

public class Event
{
    public long ID { get; set; }

    public string GroupKey { get; set; }

    public DateTime EventStarts { get; set; }

    public DateTime EventEnds { get; set; }

    public string EventName { get; set; }

    public string EventDescription { get; set; }

    public string? EventPlace { get; set; }
}
