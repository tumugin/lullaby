namespace Lullaby.Responses.Api.Events;

using Crawler.Groups;
using Models;

public class GroupEventsGetResponse
{
    public required IGroup Group { get; set; }
    public required IEnumerable<Event> Events { get; set; }
}
