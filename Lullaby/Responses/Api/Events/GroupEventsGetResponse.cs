namespace Lullaby.Responses.Api.Events;

using Crawler.Groups;
using Models;
using ViewModels;

public class GroupEventsGetResponse
{
    public required GroupViewModel Group { get; set; }
    public required IEnumerable<Event> Events { get; set; }
}
