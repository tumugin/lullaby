namespace Lullaby.Admin.Requests.Events;

using Microsoft.AspNetCore.Mvc;

public class IndexRequest
{
    [FromQuery] public string? GroupKey { get; init; }
    [FromQuery] public string? EventName { get; init; }
    [FromQuery] public string? StartDateTime { get; init; }
    [FromQuery] public string? EndDateTime { get; init; }
    [FromQuery] public int Page { get; init; } = 1;
}
