namespace Lullaby.Responses.Api.Events;

using Crawler.Groups;
using Models;
using ViewModels;

public class GroupEventsGetResponse
{
    /// <summary>
    /// Group details of the specified group
    /// </summary>
    public required GroupViewModel Group { get; set; }

    /// <summary>
    /// Events of the specified group
    /// </summary>
    public required IReadOnlyList<EventViewModel> Events { get; set; }
}
