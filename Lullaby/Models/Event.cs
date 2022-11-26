namespace Lullaby.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Crawler.Events;

public class Event
{
    public long ID { get; set; }

    public required string GroupKey { get; set; }

    public required DateTimeOffset EventStarts { get; set; }

    public required DateTimeOffset EventEnds { get; set; }

    public required string EventName { get; set; }

    public required string EventDescription { get; set; }

    public required string? EventPlace { get; set; }

    [NotMapped] public EventType EventType => (new EventTypeDetector()).DetectEventTypeByTitle(EventName);
}
