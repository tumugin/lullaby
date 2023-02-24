namespace Lullaby.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crawler.Events;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1716

[Index(nameof(GroupKey))]
[Index(nameof(EventStarts))]
[Index(nameof(EventEnds))]
public class Event
{
    public long Id { get; set; }

    public required string GroupKey { get; set; }

    public required DateTimeOffset EventStarts { get; set; }

    public required DateTimeOffset EventEnds { get; set; }

    public required bool IsDateTimeDetailed { get; set; }

    public required string EventName { get; set; }

    public required string EventDescription { get; set; }

    public required string? EventPlace { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

    [ConcurrencyCheck] public required DateTimeOffset UpdatedAt { get; set; }

    [Column(TypeName = "varchar(50)")] public required EventType EventType { get; set; }
}
