namespace Lullaby.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Crawler.Events;

public class Event
{
    public long Id { get; set; }

    public required string GroupKey { get; set; }

    public required DateTimeOffset EventStarts { get; set; }

    public required DateTimeOffset EventEnds { get; set; }

    public required string EventName { get; set; }

    public required string EventDescription { get; set; }

    public required string? EventPlace { get; set; }

    [Column(TypeName = "varchar(50)")] public required EventType EventType { get; set; }

    [NotMapped] public EventType EstimatedEventType => (new EventTypeDetector()).DetectEventTypeByTitle(this.EventName);
}
